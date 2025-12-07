import React, { useContext } from "react";
import { useTsxHelper } from "@/helpers";

// Child component.
import { FormContext } from "@/components/form/Form.tsx";
import { Button } from "@/components/ui";
import { TrashIcon } from "@heroicons/react/24/outline";

// Props.
type DeleteButtonProps = Omit<React.ComponentPropsWithoutRef<"button">, "type">;

// Component.
export default function DeleteButton(props: DeleteButtonProps): React.ReactNode {
  // Dependencies.
  const formContext = useContext(FormContext);
  const { compute } = useTsxHelper();
  
  // Computed.
  const isSubmitting = compute<boolean>(() => {
    if (!formContext) {
      return false;
    }

    return formContext.submissionState === "submitting" && formContext.submissionType === "delete";
  });
  
  // Template.
  return (
    <Button {...props} type="button" className="danger min-w-20">
      {!isSubmitting && <TrashIcon className="size-4.5 me-1" />}
      <span>{isSubmitting ? "Đang xoá dữ liệu" : "Xoá dữ liệu"}</span>
    </Button>
  );
}