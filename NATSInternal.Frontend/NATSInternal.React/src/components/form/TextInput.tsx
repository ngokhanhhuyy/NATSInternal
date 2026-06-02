import { joinClassName } from "@/helpers";
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
  const { value, onValueChanged, autoComplete = "off", ...domProps } = props;

  // Template.
  function renderInput(className?: string, path?: string, displayName?: string) {
    return (
      <input
        {...domProps}
        autoComplete={autoComplete}
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
