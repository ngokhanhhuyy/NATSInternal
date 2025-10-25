import { useContext } from "react";
import { useTsxHelper } from "@/helpers";
import { FormFieldContext } from "./FormField";

// Props.
type InputProps = { render: (className?: string, displayName?: string) => React.ReactNode };

// Component.
export default function Input(props: InputProps) {
  // Dependencies.
  const formFieldContext = useContext(FormFieldContext);
  const { compute } = useTsxHelper();

  // Computed.
  const className = compute(() => {
    if (!formFieldContext || !formFieldContext?.isValidated) {
      return;
    }

    return formFieldContext.hasError ? "invalid" : "valid";
  });

  return props.render(className, formFieldContext?.displayName);
}