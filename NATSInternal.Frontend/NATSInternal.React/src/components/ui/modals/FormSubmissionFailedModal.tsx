import React, { useRef, useImperativeHandle, forwardRef } from "react";

// Child components.
import ConfirmationModal, { type ConfirmationModalHandler } from "./ConfirmationModal";
import { ExclamationTriangleIcon } from "@heroicons/react/24/outline";

// Component.
const FormSubmissionFailedModal = forwardRef<ConfirmationModalHandler, { }>((_, ref): React.ReactNode => {
  // States.
  const componentRef = useRef<ConfirmationModalHandler>(null!);

  // Handle.
  useImperativeHandle(ref, () => ({
    async confirmAsync(): Promise<void> {
      await componentRef.current.confirmAsync();
    }
  }));

  // Template.
  return (
    <ConfirmationModal
      ref={componentRef}
      title="Dữ liệu không hợp lệ"
      IconComponent={ExclamationTriangleIcon}
      informationContent={["Dữ liệu đã nhập không hợp lệ.", "Vui lòng kiểm tra lại."]}
    />
  );
});

FormSubmissionFailedModal.displayName = "FormSubmissionFailedModal";
export default FormSubmissionFailedModal;