import React, { useContext } from "react";
import { useTsxHelper } from "@/helpers";
import { FormFieldContext } from "./FormField";

// Props.
type InputProps = { render: (className?: string, path?: string, displayName?: string) => React.ReactNode };

// Component.
export default function Input(props: InputProps) {
  // Dependencies.
  const context = useContext(FormFieldContext);
  const { joinClassName, compute } = useTsxHelper();

  // Computed.
  const className = compute(() => {
    const staticClassName = joinClassName(
      "border rounded-lg px-2 py-1 shadow-xs focus:shadow-sm min-h-[30px]",
      "transition duration-200 [transition-property:outline-color] [transition-duration:.1s]",
      "outline-3 outline-transparent"
    );

    if (!context || !context.isValidated || (!context.hasError && !context.showValidState)) {
      return joinClassName(
        "bg-white dark:bg-neutral-800 border-black/10 dark:border-white/15",
        "focus:border-black dark:focus:border-white/50 focus:outline-primary/15 dark:focus:outline-white/30",
        staticClassName
      );
    }

    let computedClassName: string | undefined;
    if (context.hasError) {
      computedClassName = joinClassName(
        "bg-red-500/5 dark:bg-red-600/10 text-red-500 dark:text-red-600",
        "border-red-500/70 dark:border-red-600/70 focus:border-red-500 dark:focus:border-red-600",
        "focus:outline-red-500/15 dark:focus:outline-red-600/40"
      );
    } else {
      computedClassName = joinClassName(
        "bg-emerald-500/5 dark:bg-emerald-600/10 text-emerald-500 dark:text-emerald-600",
        "border-emerald-500/70 dark:border-emerald-600/70 focus:border-emerald-500 dark:focus:border-emerald-600",
        "focus:outline-danger/15 dark:focus:outline-emerald-600/40"
      );
    }

    return joinClassName(computedClassName, staticClassName);
  });

  return props.render(className, context?.path, context?.displayName);
}