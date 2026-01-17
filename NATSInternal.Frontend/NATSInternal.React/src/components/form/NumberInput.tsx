import React from "react";
import { useTsxHelper } from "@/helpers";
import Input from "./Input";

// Props.
export type NumberInputProps = {
  value: number;
  onValueChanged(newValue: number): any;
} & Omit<React.ComponentPropsWithoutRef<"input">, "type">;

// Component.
export default function NumberInput(props: NumberInputProps) {
  // Props.
  const { value, onValueChanged, ...domProps } = props;

  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // Callbacks.
  function handleInput(event: React.FormEvent<HTMLInputElement>): void {
    const inputElement = event.target as HTMLInputElement;
    if (!inputElement.value.length) {
      onValueChanged(0);
      return;
    }

    if (/\d+/.test(inputElement.value)) {
      onValueChanged(parseInt(inputElement.value));
    }
  }

  // Template.
  function renderInput(className?: string, path?: string, displayName?: string) {
    return (
      <input
        {...domProps}
        name={path}
        className={joinClassName(className, props.className)}
        placeholder={props.placeholder ?? displayName}
        value={value}
        onInput={handleInput}
      />
    );
  }

  return <Input render={renderInput} />;
}