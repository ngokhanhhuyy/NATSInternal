import React from "react";
import { useTsxHelper } from "@/helpers";

// Child components.
import Spinner from "./Spinner";

// Props.
type ButtonProps = {
  variant?: ColorVariant;
  showSpinner?: boolean;
} & React.ComponentPropsWithoutRef<"button">;

export function Button(props: ButtonProps) {
  // Props.
  const { variant = "indigo", showSpinner, ...domProps } = props;

  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // Template.
  return (
    <button {...domProps} className={joinClassName(variant, props.className )}>
      {showSpinner && <Spinner size="sm" />}
      {props.children}
    </button>
  );
}