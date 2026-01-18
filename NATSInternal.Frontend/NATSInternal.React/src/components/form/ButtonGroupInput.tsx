import React from "react";
import { useTsxHelper } from "@/helpers";

// Child components.
import { Button } from "../ui";

// Props.
export type ButtonGroupInputOption<T> = {
  value: T;
  displayName?: string;
  icon?: React.ReactNode;
};

export type ButtonGroupInputProps<T> = {
  options: ButtonGroupInputOption<T>[];
  value: T;
  onValueChanged(newValue: T): any;
  buttonClassName?: string | ((isActive: boolean) => string | undefined)
} & Omit<React.ComponentPropsWithoutRef<"div">, "type">;

// Component.
export default function ButtonGroupInput<T>(props: ButtonGroupInputProps<T>): React.ReactNode {
  // Props.
  const { options, value, onValueChanged, buttonClassName, ...domProps } = props;

  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // Template.
  return (
    <div {...domProps} className={joinClassName("items-stretch", domProps.className)}>
      {props.options.map((option, index) => (
        <Button
          className={joinClassName(
            props.value === option.value && "btn-primary",
            index < props.options.length - 1 && "rounded-r-none",
            index > 0 && "rounded-l-none",
            typeof props.buttonClassName === "string"
              ? props.buttonClassName
              : props.buttonClassName?.(props.value === option.value)
          )}
          key={index}
          onClick={() => props.onValueChanged(option.value)}
        >
          {option.icon}
          {option.displayName ?? JSON.stringify(option.value)}
        </Button>
      ))}
    </div>
  );
}