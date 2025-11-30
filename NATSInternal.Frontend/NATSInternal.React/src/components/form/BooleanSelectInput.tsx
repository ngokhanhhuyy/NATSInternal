import React from "react";

// Child component.
import SelectInput, { type SelectInputProps } from "./SelectInput";

// Props.
type BooleanSelectInputProps = Omit<SelectInputProps, "value" | "onValueChanged"> & {
  value: boolean;
  onValueChanged: (changedValue: boolean) => any;
};

// Component.
export default function BooleanSelectInput(props: BooleanSelectInputProps): React.ReactNode {
  // Props.
  const { value, onValueChanged, ...domProps } = props;
  // Template.
  return (
    <SelectInput
      {...domProps}
      value={value.toString()}
      onValueChanged={(changedValue) => onValueChanged(JSON.parse(changedValue) as boolean)}
    />
  );
}