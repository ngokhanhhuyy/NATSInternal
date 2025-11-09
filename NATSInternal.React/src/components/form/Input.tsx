import React, { useContext } from "react";
import { useTsxHelper } from "@/helpers";
import { FormFieldContext } from "./FormField";

// Props.
type InputProps = { render: (className?: string, displayName?: string) => React.ReactNode };

// Component.
export default function Input(props: InputProps) {
  // Dependencies.
  const context = useContext(FormFieldContext);
  const { joinClassName, compute } = useTsxHelper();

  // Computed.
  const className = compute(() => {
    const staticClassName = joinClassName(
      "border rounded-lg px-2 py-1 shadow-xs focus:shadow-sm",
      "transition duration-200 [transition-property:outline-color] [transition-duration:.1s]",
      "outline-3 outline-transparent"
    );

    if (!context || !context.isValidated || (!context.hasError && !context.showValidState)) {
      return `bg-white border-primary/10 focus:border-primary focus:outline-primary/15 ${staticClassName}`;
    }

    let computedClassName: string;
    if (context.hasError) {
      computedClassName = "bg-danger/5 text-danger border-danger/70 focus:border-danger focus:outline-danger/15";
    } else {
      computedClassName = "bg-success/5 text-success border-success/70 focus:border-success focus:outline-success/15";
    }

    return joinClassName(computedClassName, staticClassName);
  });

  return props.render(className, context?.displayName);
}