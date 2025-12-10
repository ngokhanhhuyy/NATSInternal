import React from "react";
import { useTsxHelper } from "@/helpers";

// Child components.
import Spinner from "./Spinner";

// Props.
export type ButtonProps = { showSpinner?: boolean } & React.ComponentPropsWithoutRef<"button">;

export default function Button(props: ButtonProps) {
  // Props.
  const { type = "button", showSpinner, ...domProps } = props;

  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // Template.
  return (
    <button {...domProps} type={type} className={joinClassName("button", domProps.className)}>
      {showSpinner && <Spinner size="sm" />}
      {props.children}
    </button>
  );
}