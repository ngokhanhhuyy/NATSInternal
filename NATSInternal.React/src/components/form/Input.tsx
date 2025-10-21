import { useContext } from "react";
import { useTsxHelper } from "@/helpers";
import { FormFieldContext } from "./FormField";

// Props.
type InputProps = { render: (className: string | undefined) => React.ReactNode };

// Component.
export default function Input(props: InputProps) {
  // Dependencies.
  const formFieldPayload = useContext(FormFieldContext);
  const { compute } = useTsxHelper();

  // Computed.
  const className = compute(() => {
    if (!formFieldPayload || !formFieldPayload?.isValidated) {
      return;
    }

    return formFieldPayload.hasError ? "invalid" : "valid";
  });

  return props.render(className);
}