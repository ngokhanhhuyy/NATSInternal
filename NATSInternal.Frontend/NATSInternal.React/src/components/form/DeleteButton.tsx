import React, { useContext } from "react";
import { useTsxHelper } from "@/helpers";

// Child component.
import { FormContainerContext } from "@/components/layouts";
import { FormContext } from "@/components/form/Form";
import { Button } from "@/components/ui";
import { TrashIcon } from "@heroicons/react/24/outline";

// Props.
type DeleteButtonProps = Omit<React.ComponentPropsWithoutRef<"button">, "type">;

// Component.
export default function DeleteButton(props: DeleteButtonProps): React.ReactNode {
  // Dependencies.
  const formContainerContext = useContext(FormContainerContext);
  const formContext = useContext(FormContext);
  const { compute } = useTsxHelper();
  
  // Computed.
  const isSubmittingOrDeleting = compute<boolean>(() => {
    return formContainerContext?.isDeleting || formContext?.submissionState === "submitting" ;
  });
  
  // Template.
  return (
    <Button
      {...props}
      type="button"
      className="btn btn-danger min-w-20"
      onClick={formContainerContext.handleDeletionAsync}
    >
      {!isSubmittingOrDeleting && <TrashIcon className="size-4.5 me-1" />}
      <span>{formContainerContext.isDeleting ? "Đang xoá dữ liệu" : "Xoá dữ liệu"}</span>
    </Button>
  );
}