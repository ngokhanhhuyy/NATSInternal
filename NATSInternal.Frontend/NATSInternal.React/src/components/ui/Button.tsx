import React from "react";
import { joinClassName } from "@/helpers";

// Child components.
import Spinner from "./Spinner";

// Props.
export type ButtonProps = { showSpinner?: boolean } & React.ComponentPropsWithoutRef<"button">;

export default function Button(props: ButtonProps) {
  // Props.
  const { type = "button", showSpinner, ...domProps } = props;

  // Template.
  return (
    <button {...domProps} type={type} className={joinClassName("btn", domProps.className)}>
      {showSpinner && <Spinner size="sm" />}
      {props.children}
    </button>
  );
}
