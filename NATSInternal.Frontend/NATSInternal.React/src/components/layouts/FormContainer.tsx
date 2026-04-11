import React, { useState, useRef, useCallback, useEffect, createContext, type ComponentProps } from "react";
import { useBlocker, type BlockerFunction } from "react-router";
import { OperationError, ValidationError } from "@/api";
import { useTsxHelper } from "@/helpers";

// Child component.
import MainContainer from "./MainContainer";
import { Form } from "@/components/form";
import { ConfirmationModal, YesNoModal, type ConfirmationModalHandler, type YesNoModalHandler } from "@/components/ui";
import { ExclamationCircleIcon, ExclamationTriangleIcon, CheckCircleIcon } from "@heroicons/react/24/outline";

// Context payload.
type FormContainerContextPayload = {
  isDeleting: boolean;
  handleDeletionAsync?(): Promise<void>;
};

// Context.
export const FormContainerContext = createContext<FormContainerContextPayload>({
  isDeleting: false
});

// Props.
type FormContainerProps<TUpsertResult> = {
  deleteAction?: () => Promise<void>;
  onDeletionSucceeded?: () => any;
  onDeletionFailed?: (error: Error, errorHandled: boolean) => any;
  isModelDirty?: boolean;
} & ComponentProps<typeof Form<TUpsertResult>> & ComponentProps<typeof MainContainer>;

// Component.
export default function FormContainer<TUpsertResult>(props: FormContainerProps<TUpsertResult>): React.ReactNode {
  // Dependencies.
  const { joinClassName, compute } = useTsxHelper();

  // Blocker.
  const shouldBlock = useCallback<BlockerFunction>(() => {
    return props.isModelDirty === true && !isSubmissionSucceeded.current;
  }, [props.isModelDirty]);
  const blocker = useBlocker(shouldBlock);

  // States.
  const [deletionState, setDeletionState] = useState<"notDeleting" | "deleting" | "deletionSucceeded">("notDeleting");
  const isSubmissionSucceeded = useRef<boolean>(false);
  const dirtyModelConfirmationModelRef = useRef<YesNoModalHandler>(null!);
  const formSubmissionSucceededModalRef = useRef<ConfirmationModalHandler>(null!);
  const formSubmissionFailedModalRef = useRef<ConfirmationModalHandler>(null!);
  const formDeletionConfirmationModalRef = useRef<YesNoModalHandler>(null!);
  const formDeletionSuceededModalRef = useRef<ConfirmationModalHandler>(null!);
  const formDeletionFailedModalRef = useRef<ConfirmationModalHandler>(null!);
  const [formUpsertingErrorMessages, setFormUpsertingErrorMessages] = useState<string[] | null>(null);

  // Callbacks.
  function handleUpsertingSucceeded(result: TUpsertResult): void {
    isSubmissionSucceeded.current = true;
    formSubmissionSucceededModalRef.current
      .confirmAsync()
      .then(() => props.onUpsertingSucceeded?.(result));
  }

  function handleUpsertingFailed(error: Error, isErrorHandled: boolean): void {
    formSubmissionFailedModalRef.current
      .confirmAsync()
      .then(() => {
        if (error instanceof ValidationError || error instanceof OperationError) {
          setFormUpsertingErrorMessages(Object.values(error.errors));
        }

        window.scrollTo({ top: 0, behavior: "smooth" });
        props.onUpsertingFailed?.(error, isErrorHandled);
      });
  }

  async function handleDeleteAsync(): Promise<void> {
    if (!props.deleteAction) {
      return;
    }

    const answer = await formDeletionConfirmationModalRef.current.getAnswerAsync();
    if (!answer) {
      return;
    }

    try {
      setDeletionState("deleting");
      await props.deleteAction?.();
      setDeletionState("deletionSucceeded");
      await formDeletionSuceededModalRef.current.confirmAsync();
      props.onDeletionSucceeded?.();
    } catch (error) {
      setDeletionState("notDeleting");
      await formDeletionFailedModalRef.current.confirmAsync();
      if (error instanceof ValidationError || error instanceof OperationError) {
        props.onDeletionFailed?.(error, true);
        return;
      }

      props.onDeletionFailed?.(error as Error, false);
      throw error;
    }
  }

  // Payload.
  const contextPayload = compute<FormContainerContextPayload>(() => ({
    isDeleting: deletionState === "deleting",
    handleDeletionAsync: handleDeleteAsync
  }));

  // Effect.
  useEffect(() => {
    if (blocker.state === "blocked") {
      dirtyModelConfirmationModelRef.current
        .getAnswerAsync()
        .then(answer => {
          if (answer) {
            blocker.proceed?.();
            return;
          }

          blocker.reset?.();
        });
    }
  }, [blocker.state]);

  // Template.
  return (
    <FormContainerContext.Provider value={contextPayload}>
      <MainContainer>
        {formUpsertingErrorMessages && (
          <div className={joinClassName(
            "bg-red-500/5 border border-red-500 dark:border-red-400/50 text-red-500 dark:text-red-400",
            "flex items-center gap-3 rounded-lg px-4 py-2"
          )}>
            <ExclamationCircleIcon className="size-7" />
            <div className="flex flex-col">
              {formUpsertingErrorMessages.map((message, index) => (
                <span key={index}>{message}</span>
              ))}
            </div>
          </div>
        )}

        <Form
          className="flex flex-col gap-3"
          upsertAction={props.upsertAction}
          onUpsertingSucceeded={handleUpsertingSucceeded}
          onUpsertingFailed={handleUpsertingFailed}
          isModelDirty={props.isModelDirty}
        >
          {props.children}
        </Form>

        <YesNoModal
          ref={dirtyModelConfirmationModelRef}
          IconComponent={ExclamationCircleIcon}
          iconClassName="stroke-yellow-500 dark:stroke-yellow-600"
          title="Xác nhận chuyển hướng"
          questionContent={["Dữ liệu thay đổi chưa được lưu. ", "Bạn có chắc chắn muốn chuyển hướng?"]}
          yesButtonText="Chắc chắn"
          yesButtonClassName="btn-danger"
          noButtonText="Quay lại"
        />

        <ConfirmationModal
          ref={formSubmissionSucceededModalRef}
          title="Lưu thành công"
          IconComponent={CheckCircleIcon}
          iconClassName="stroke-emerald-600"
          informationContent="Dữ liệu đã được lưu thành công."
        />

        <ConfirmationModal
          ref={formDeletionSuceededModalRef}
          title="Xoá thành công"
          IconComponent={CheckCircleIcon}
          iconClassName="stroke-emerald-600"
          informationContent="Dữ liệu đã được xoá thành công."
        />

        <YesNoModal
          ref={formDeletionConfirmationModalRef}
          IconComponent={ExclamationCircleIcon}
          iconClassName="stroke-yellow-500 dark:stroke-yellow-600"
          title="Xác nhận xoá dữ liệu"
          questionContent={[
            "Dữ liệu sau khi xoá có thể sẽ không thể khôi phục được.",
            "Bạn có chắc chắn muốn xoá không?"
          ]}
          yesButtonText="Chắc chắn"
          yesButtonClassName="btn-danger"
          noButtonText="Quay lại"
        />

        <ConfirmationModal
          ref={formSubmissionFailedModalRef}
          title="Dữ liệu không hợp lệ"
          IconComponent={ExclamationTriangleIcon}
          iconClassName="stroke-red-600"
          informationContent={["Dữ liệu đã nhập không hợp lệ.", "Vui lòng kiểm tra lại."]}
        />

        <ConfirmationModal
          ref={formDeletionFailedModalRef}
          title="Dữ liệu không hợp lệ"
          IconComponent={ExclamationTriangleIcon}
          iconClassName="stroke-red-600"
          informationContent={[
            "Không thể xoá dữ liệu, vui lòng thử lại.",
            "Nếu lỗi này vẫn tiếp tục xảy ra, xin hãy thông báo cho nhà phát triển."
          ]}
        />
      </MainContainer>
    </FormContainerContext.Provider>
  );
}