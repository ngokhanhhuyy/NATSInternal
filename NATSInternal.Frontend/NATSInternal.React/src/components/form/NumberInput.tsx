import React, { useRef, useEffect } from "react";
import { joinClassName } from "@/helpers";
import Input from "./Input";

// Props.
export type NumberInputProps = {
  value: number;
  onValueChanged(newValue: number): any;
  min?: number;
  max?: number;
  autoFocus?: boolean;
} & Omit<React.ComponentPropsWithoutRef<"input">, "type" | "autoFocus" | "min" | "max">;

// Component.
export default function NumberInput(props: NumberInputProps) {
  // Props.
  const { value, onValueChanged, autoFocus, ...domProps } = props;

  // States.
  const elementRef = useRef<HTMLInputElement | null>(null);

  // Callbacks.
  function handleInput(event: React.FormEvent<HTMLInputElement>): void {
    const inputElement = event.target as HTMLInputElement;
    if (!inputElement.value.length) {
      onValueChanged(0);
      return;
    }

    if (!/\d+/.test(inputElement.value)) {
      return;
    }
    
    const parsedValue = parseInt(inputElement.value);
    if (props.min != null && parsedValue < props.min) {
      onValueChanged(props.min);
      return;
    }

    if (props.max != null && parsedValue > props.max) {
      onValueChanged(props.max);
      return;
    }

    onValueChanged(parsedValue);
  }

  // Effect.
  useEffect(() => {
    if (props.autoFocus && elementRef.current) {
      elementRef.current.focus();
    }
  }, []);

  // Template.
  function renderInput(className?: string, path?: string, displayName?: string) {
    return (
      <input
        {...domProps}
        ref={elementRef}
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
