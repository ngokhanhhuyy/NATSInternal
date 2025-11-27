import { useTsxHelper } from "@/helpers";

// Child component.
import Input from "./Input";

// Props.
type SelectInputOption = {
  value: string | undefined;
  displayName?: string;
};

export type SelectInputProps = {
  showDisplayNameAsPrefix?: boolean;
  options: SelectInputOption[];
  value: string;
  onValueChanged(newValue: string): any;
} & Omit<React.ComponentPropsWithoutRef<"select">, "children">;

// Component.
export default function SelectInput(props: SelectInputProps) {
  // Props.
  const { showDisplayNameAsPrefix, options, value, onValueChanged, ...domProps } = props;

  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // Template.
  function renderInput(className?: string, path?: string) {
    return (
      <select
        {...domProps}
        name={path}
        className={joinClassName(className, props.className, "cursor-pointer")}
        value={value}
        onInput={(event) => onValueChanged((event.target as HTMLSelectElement).value)}
      >
        {options.map((op, index) => (
          <option value={op.value} key={index}>
            {op.displayName ?? op.value}
          </option>
        ))}
      </select>
    );
  }

  return <Input render={renderInput} />;
}