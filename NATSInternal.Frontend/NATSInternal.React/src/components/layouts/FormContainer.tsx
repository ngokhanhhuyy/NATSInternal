import React, { useRef, useCallback, useEffect } from "react";
import { useBlocker, type BlockerFunction } from "react-router";

// Child component.
import MainContainer from "./MainContainer";
import { Form } from "@/components/form";
import { ConfirmationModal, YesNoModal, type ConfirmationModalHandler, type YesNoModalHandler } from "@/components/ui";
import { ExclamationCircleIcon, ExclamationTriangleIcon, CheckCircleIcon } from "@heroicons/react/24/outline";

// Props.
type FormContainerProps<TUpsertResult> = {
  isModelDirty?: boolean;
} & ComponentProps<typeof Form<TUpsertResult>> & ComponentProps<typeof MainContainer>;

// Component.
export default function FormContainer<TUpsertResult>(props: FormContainerProps<TUpsertResult>): React.ReactNode {
  // Blocker.
  const shouldBlock = useCallback<BlockerFunction>(() => {
    return props.isModelDirty === true && !isSubmissionSucceeded.current;
  }, [props.isModelDirty]);
  const blocker = useBlocker(shouldBlock);

  // States.
  const isSubmissionSucceeded = useRef<boolean>(false);
  const dirtyModelConfirmationModelRef = useRef<YesNoModalHandler>(null!);
  const formSubmissionSucceededModalRef = useRef<ConfirmationModalHandler>(null!);
  const formSubmissionFailedModalRef = useRef<ConfirmationModalHandler>(null!);

  // Callbacks.
  const handleUpsertingSucceeded = useCallback((result: TUpsertResult) => {
    isSubmissionSucceeded.current = true;
    formSubmissionSucceededModalRef.current
      .confirmAsync()
      .then(() => props.onUpsertingSucceeded?.(result));
  }, [props.onUpsertingSucceeded]);

  const handleUpsertingFailed = useCallback((error: Error, isErrorHandled: boolean) => {
    formSubmissionFailedModalRef.current
      .confirmAsync()
      .then(() => props.onUpsertingFailed?.(error, isErrorHandled));
  }, [props.onUpsertingFailed]);

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
    <MainContainer description={props.description}>
      <Form
        className="flex flex-col gap-3"
        upsertAction={props.upsertAction}
        onUpsertingSucceeded={handleUpsertingSucceeded}
        onUpsertingFailed={handleUpsertingFailed}
        deleteAction={props.deleteAction}
        onDeletionSucceeded={props.onDeletionSucceeded}
        onDeletionFailed={props.onDeletionFailed}
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
        yesButtonClassName="danger"
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
        ref={formSubmissionFailedModalRef}
        title="Dữ liệu không hợp lệ"
        IconComponent={ExclamationTriangleIcon}
        iconClassName="stroke-red-600"
        informationContent={["Dữ liệu đã nhập không hợp lệ.", "Vui lòng kiểm tra lại."]}
      />
    </MainContainer>
  );
}