import { useContext } from "solid-js";
import { FormFieldContext } from "./FormField";

// Props.
type InputProps = { render: (getClassName: () => string | undefined) => JSX.Element };

// Component.
export default function Input(props: InputProps) {
  // Context.
  const formFieldPayload = useContext(FormFieldContext);

  // Computed.
  const computeClassName = () => {
    if (!formFieldPayload || !formFieldPayload?.isValidated) {
      return;
    }

    return formFieldPayload.hasError ? "invalid" : "valid";
  };

  return props.render(computeClassName);
}