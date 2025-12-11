import React, { useState, useCallback } from "react";
import { useBlocker, type BlockerFunction } from "react-router";

// Child component.
import MainContainer from "./MainContainer";
import { Form } from "@/components/form";
import { FormSubmissionSucceededModal, FormSubmissionFailedModal, YesNoModal } from "@/components/ui";
import { ExclamationTriangleIcon } from "@heroicons/react/24/outline";

// Props.
type FormContainerProps<TUpsertResult> = {
  isModelDirty?: boolean;
} & ComponentProps<typeof Form<TUpsertResult>> & ComponentProps<typeof MainContainer>;

// Component.
export default function FormContainer<TUpsertResult>(props: FormContainerProps<TUpsertResult>): React.ReactNode {
  // Blocker.
  const shouldBlock = useCallback<BlockerFunction>(() => {
    return props.isModelDirty != null && props.isModelDirty;
  }, [props.isModelDirty]);
  const blocker = useBlocker(shouldBlock);

  // States.
  const [isSucceededModalVisible, setIsSucceededModalVisible] = useState<boolean>(false);
  const [isFailedModalVisible, setIsFailedModalVisible] = useState<boolean>(false);
  const [onSucceededModalHidden, setOnSucceededModalHidden] = useState<(() => void) | undefined>(undefined);
  const [onFailedModalHidden, setOnFailedModalHidden] = useState<(() => void) | undefined>(undefined);

  // Callbacks.
  const handleUpsertingSucceeded = useCallback((result: TUpsertResult) => {
    setIsSucceededModalVisible(true);
    setOnSucceededModalHidden(() => props.onUpsertingSucceeded?.(result));
  }, [props.onUpsertingSucceeded]);

  const handleSucceededModalHidden = useCallback(() => {
    setIsSucceededModalVisible(false);
    setTimeout(() => onSucceededModalHidden?.(), 200);
  }, []);

  const handleFormFailed = useCallback((error: Error, errorHandled: boolean) => {
    setIsFailedModalVisible(true);
    if (props.onUpsertingFailed != null) {
      setOnFailedModalHidden(props.onUpsertingFailed(error, errorHandled));
    }
  }, []);

  const handleSubmissionFailedModalHidden = useCallback(() => {
    setIsFailedModalVisible(false);
  }, []);

  const handleSubmissionFailedModalHidden = useCallback(() => {
    setIsFailedModalVisible(false);
  }, []);

  const handleNavigationConfirmationAnswered = useCallback((answer: boolean) => {
    if (answer) {
      blocker.proceed?.();
      return;
    }

    blocker.reset?.();
  }, [blocker]);


    // Template.
  return (
    <MainContainer description={props.description}>
      <Form
        className="flex flex-col gap-3"
        upsertAction={props.upsertAction}
        onUpsertingSucceeded={handleUpsertingSucceeded}
        onUpsertingFailed={handleFormFailed}
        deleteAction={props.deleteAction}
        onDeletionSucceeded={props.onDeletionSucceeded}
        onDeletionFailed={props.onDeletionFailed}
        isModelDirty={props.isModelDirty}
      >
        {props.children}
      </Form>

      <YesNoModal
        IconComponent={ExclamationTriangleIcon}
        title="Xác nhận chuyển hướng"
        questionContent={["Dữ liệu thay đổi chưa được lưu. ", "Bạn có chắc chắn muốn chuyển hướng?"]}
        isVisible={blocker.state === "blocked"}
        yesButtonText="Chắc chắn"
        yesButtonClassName="danger"
        noButtonText="Quay lại"
        onAnswer={handleNavigationConfirmationAnswered}
      />

      <FormSubmissionSucceededModal
        isVisible={isSucceededModalVisible}
        onHidden={onSucceededModalHidden}
      />

      <FormSubmissionFailedModal
        isVisible={isFailedModalVisible}
        onHidden={handleSubmissionFailedModalHidden}
      />

    </MainContainer>
  );
}