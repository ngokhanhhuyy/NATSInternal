import React from "react";
import { joinClassName } from "@/helpers";

// Child component.
import Input from "./Input";

// Props.
type DateTimeInputProps = Omit<React.ComponentPropsWithoutRef<"input">, "value" | "type"> & {
  type: "datetime" | "date";
  value: string;
  onValueChanged: (changedValue: string) => any;
};

// Component.
export default function DateTimeInput(props: DateTimeInputProps): React.ReactNode {
  // Props.
  const { onValueChanged, ...domProps } = props;
  
  // Template.
  function renderInputElement(className?: string, path?: string, _?: string): React.ReactNode {
    return (
      <input
        {...domProps}
        id={path}
        name={path}
        className={joinClassName(className, props.className)}
        type={props.type}
        value={props.value}
        onInput={(event) => onValueChanged((event.target as HTMLInputElement).value)}
      />
    );
  }
  
  return <Input render={renderInputElement} />;
}
