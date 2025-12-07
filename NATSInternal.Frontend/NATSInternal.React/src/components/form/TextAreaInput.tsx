import React from "react";
import { useTsxHelper } from "@/helpers";
import Input from "./Input";

// Props.
export type TextInputProps = {
  value: string;
  onValueChanged(newValue: string): any;
} & React.ComponentPropsWithoutRef<"textarea">;

// Component.
export default function TextInput(props: TextInputProps): React.ReactNode {
  // Props.
  const { value, onValueChanged, ...domProps } = props;

  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // Template.
  function renderInput(className?: string, path?: string, displayName?: string) {
    return (
      <textarea
        {...domProps}
        name={path}
        className={joinClassName(className, props.className)}
        placeholder={props.placeholder ?? displayName}
        value={value}
        onInput={(event) => onValueChanged((event.target as HTMLInputElement).value)}
      />
    );
  }

  return <Input render={renderInput} />;
}