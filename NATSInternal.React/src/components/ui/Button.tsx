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
  const { variant = "secondary", outline = false, showSpinner, ...domProps } = props;

  // Dependencies.
  const { joinClassName, compute } = useTsxHelper();

  // Computed.
  const className = compute(() => {
    const sharedClassName = joinClassName(
      "flex justify-center items-center px-2 py-1 border",
      "rounded-lg shadow-xs cursor-pointer transition duration-150");
      
    if (variant === "secondary") {
      return `bg-secondary border-neutral-200 text-neutral-800 hover:bg-neutral-100 hover:border-neutral-300 ${sharedClassName}`;
    }

    let variantClassNameMap: Record<Exclude<ColorVariant, "secondary">, string>;
    if (outline) {
      variantClassNameMap = {
        primary: "border-primary text-primary hover:bg-primary",
        danger: "border-danger text-danger hover:bg-danger",
        success: "border-success text-success hover:bg-success",
      };

      return `bg-white border hover:text-white ${variantClassNameMap[variant]} ${sharedClassName}`;
    }
    
    variantClassNameMap = {
      primary: "bg-primary border-primary",
      danger: "bg-danger border-danger",
      success: "bg-success border-success",
    };

    return `text-primary-foreground hover:opacity-75 border ${variantClassNameMap[variant]} ${sharedClassName}`;
  });

  // Template.
  return (
    <button {...domProps} className={joinClassName(className, props.className)}>
      {showSpinner && <Spinner size="sm" />}
      {props.children}
    </button>
  );
}