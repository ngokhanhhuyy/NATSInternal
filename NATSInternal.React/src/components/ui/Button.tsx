import React from "react";
import { useTsxHelper } from "@/helpers";

// Child components.
import Spinner from "./Spinner";

// Props.
type ButtonProps = {
  variant?: ColorVariant;
  outline?: boolean;
  showSpinner?: boolean;
} & React.ComponentPropsWithoutRef<"button">;

export function Button(props: ButtonProps) {
  // Props.
  const { type = "button", variant = "secondary", outline = false, showSpinner, ...domProps } = props;

  // Dependencies.
  const { joinClassName, compute } = useTsxHelper();

  // Computed.
  const className = compute(() => {
    const sharedClassName = joinClassName(
      "flex justify-center items-center px-2 py-1 border",
      "rounded-lg shadow-xs cursor-pointer transition-all duration-150");
      
    if (variant === "secondary") {
      return joinClassName(
        "bg-white dark:bg-neutral-800 border-black/15 dark:border-white/15 text-black/80 dark:text-white/80",
        "hover:bg-neutral-100 hover:dark:bg-neutral-800 hover:border-black/20 hover:dark:border-white/20",
        sharedClassName
      );
    }

    let variantClassNameMap: Record<Exclude<ColorVariant, "secondary">, string>;
    if (outline) {
      variantClassNameMap = {
        primary: "border-primary text-primary hover:bg-primary",
        danger: "border-danger text-danger hover:bg-danger",
        success: "border-success text-success hover:bg-success",
        hinting: "border-hinting text-hinting hover:bg-hinting"
      };

      return `bg-white border hover:text-white ${variantClassNameMap[variant]} ${sharedClassName}`;
    }
    
    variantClassNameMap = {
      primary: "bg-primary border-primary hover:opacity-80",
      danger: "bg-red-500 dark:bg-red-700 border-red-600 dark:border-red-500 " +
        "hover:bg-red-400 dark:hover:bg-red-600 hover:border-red-600 dark:hover:border-red-400",
      success: "bg-emerald-500 dark:bg-emerald-600 border-emerald-500 hover:brightness-120",
      hinting: "bg-blue-500 dark:bg-blue-600 border-blue-500 hover:brightness-140"
    };

    return joinClassName(
      "text-primary-foreground border",
      variantClassNameMap[variant],
      sharedClassName
    );
  });

  // Template.
  return (
    <button {...domProps} type={type} className={joinClassName(className, props.className)}>
      {showSpinner && <Spinner size="sm" />}
      {props.children}
    </button>
  );
}