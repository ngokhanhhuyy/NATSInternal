import React, { useContext } from "react";
import { useTsxHelper } from "@/helpers";

// Child component.
import { FormContext } from "@/components/form/Form.tsx";
import { Button } from "@/components/ui";
import { ArrowDownOnSquareStackIcon } from "@heroicons/react/24/outline";

// Props.
type SubmitButtonProps = Omit<React.ComponentPropsWithoutRef<"button">, "type">;

// Component.
export default function SubmitButton(props: SubmitButtonProps): React.ReactNode {
  // Dependencies.
  const formContext = useContext(FormContext);
  const { compute } = useTsxHelper();
  
  // Computed.
  const isSubmitting = compute<boolean>(() => {
    if (!formContext) {
      return false;
    }

    return formContext.submissionState === "submitting" && formContext.submissionType === "upsert";
  });
  
  // Template.
  return (
    <Button {...props} type="submit" className="primary min-w-20" showSpinner={isSubmitting}>
      {!isSubmitting && <ArrowDownOnSquareStackIcon className="size-4.5 me-1" />}
      <span>{isSubmitting ? "Đang lưu" : "Lưu"}</span>
    </Button>
  );
}