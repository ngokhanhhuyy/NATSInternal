import React from "react";

// Child components.
import BaseModal from "./BaseModal";
import { Button } from "@/components/ui";
import { ExclamationTriangleIcon } from "@heroicons/react/24/outline";

// Props.
type FormSubmissionFailedModalProps = Omit<
  ComponentProps<typeof BaseModal>,
  "title" | "headerChildren" | "footerChildren"
>;

// Component.
export default function FormSubmissionFailedModal(props: FormSubmissionFailedModalProps): React.ReactNode {
  // Template.
  const footerChildren = <Button className="h-fit" onClick={props.onHidden}>Đồng ý</Button>;

  return (
    <BaseModal {...props} title="Dữ liệu không hợp lệ" footerChildren={footerChildren}>
      <div className="grid grid-cols-[auto_1fr] gap-5 p-5">
        <ExclamationTriangleIcon className="size-8 shrink-0 grow-0 self-center" />
        <div className="flex flex-col">
          <span>Dữ liệu đã nhập không hợp lệ.</span>
          <span>Vui lòng kiểm tra lại.</span>
        </div>
      </div>
    </BaseModal>
  );
}