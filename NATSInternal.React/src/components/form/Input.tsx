import { useContext } from "react";
import { useTsxHelper } from "@/helpers";
import { FormFieldContext } from "./FormField";

// Props.
type InputProps = { render: (className?: string, displayName?: string) => React.ReactNode };

// Component.
export default function Input(props: InputProps) {
  // Dependencies.
  const formFieldContext = useContext(FormFieldContext);
  const { joinClassName, compute } = useTsxHelper();

  // Computed.
  const className = compute(() => {
    const sharedClassName = joinClassName(
      "border rounded-lg px-2 py-1 shadow-xs focus:shadow-sm",
      "transition duration-200 [transition-property:outline-color] [transition-duration:.1s]",
      "outline focus:outline-3 outline-transparent");
      
    if (!formFieldContext || !formFieldContext?.isValidated) {
      return `bg-white border-primary/10 focus:border-primary focus:outline-primary/15 ${sharedClassName}`;
    }
    
    let computedClassName: string = "";
    if (formFieldContext.hasError) {
      computedClassName = "bg-danger/5 text-danger border-danger/70 focus:border-danger focus:outline-danger/15";
    }

    return joinClassName(computedClassName, sharedClassName);
  });

  return props.render(className, formFieldContext?.displayName);
}