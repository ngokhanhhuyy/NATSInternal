import { useTsxHelper } from "@/helpers";
import Input from "./Input";

// Props.
export type TextInputProps = {
  type?: "text" | "password" | "tel" | "email"
  value: string;
  onValueChanged(newValue: string): any;
} & Omit<React.ComponentPropsWithoutRef<"input">, "type">;

// Component.
export default function TextInput(props: TextInputProps) {
  // Props.
  const { value, onValueChanged, ...domProps } = props;

  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // Template.
  function renderInput(className?: string, path?: string, displayName?: string) {
    return (
      <input
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