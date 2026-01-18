import React from "react";

// Child component.
import SelectInput, { type SelectInputProps } from "./SelectInput";

// Props.
type BooleanSelectInputOption = {
  value: boolean;
  displayName?: string;
};

type BooleanSelectInputProps = Omit<SelectInputProps, "options" | "value" | "onValueChanged"> & {
  options: BooleanSelectInputOption[];
  value: boolean;
  onValueChanged: (changedValue: boolean) => any;
};

// Component.
export default function BooleanSelectInput(props: BooleanSelectInputProps): React.ReactNode {
  // Props.
  const { options, value, onValueChanged, ...domProps } = props;
  // Template.
  return (
    <SelectInput
      {...domProps}
      options={options.map(option => ({
        value: option.value.toString(),
        displayName: option.displayName
      }))}
      value={value.toString()}
      onValueChanged={(changedValue) => onValueChanged(JSON.parse(changedValue) as boolean)}
    />
  );
}