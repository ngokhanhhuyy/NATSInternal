import React, { useContext } from "react";
import { FormFieldContext } from "./FormField";

// Props.
type InputProps = { render: (className?: string, path?: string, displayName?: string) => React.ReactNode };

// Component.
export default function Input(props: InputProps) {
  // Dependencies.
  const context = useContext(FormFieldContext);

  // Template.
  return props.render(undefined, context?.path, context?.displayName);
}