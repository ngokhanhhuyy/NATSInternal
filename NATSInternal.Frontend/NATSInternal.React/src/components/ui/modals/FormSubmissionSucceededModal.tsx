import React from "react";

// Child components.
import BaseModal from "./BaseModal";
import { Button } from "@/components/ui";
import { ExclamationTriangleIcon } from "@heroicons/react/24/outline";

// Props.
type FormSubmissionSucceededModalProps = Omit<
  ComponentProps<typeof BaseModal>,
  "title" | "headerChildren" | "footerChildren"
>;

// Component.
export default function FormSubmissionSucceededModal(props: FormSubmissionSucceededModalProps): React.ReactNode {
  // Template.
  const footerChildren = <Button className="h-fit" onClick={props.onHidden}>Đồng ý</Button>;

  return (
    <BaseModal {...props} title="Lưu thành công" footerChildren={footerChildren}>
      <div className="grid grid-cols-[auto_1fr] gap-5 p-5">
        <ExclamationTriangleIcon className="size-8 shrink-0 grow-0 self-center" />
        <div className="flex justify-start items-center">
          <span>Dữ liệu đã được lưu thành công.</span>
        </div>
      </div>
    </BaseModal>
  );
}