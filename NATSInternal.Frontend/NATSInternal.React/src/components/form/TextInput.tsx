import { useTsxHelper } from "@/helpers";
import Input from "./Input";

// Props.
export type TextInputProps = {
  password?: boolean;
  value: string;
  onValueChanged(newValue: string): any;
} & React.ComponentPropsWithoutRef<"input">;

// Component.
export default function TextInput(props: TextInputProps) {
  // Props.
  const { password, value, onValueChanged, ...domProps } = props;

  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // Template.
  function renderInput(className?: string, path?: string, displayName?: string) {
    return (
      <input
        {...domProps}
        name={path}
        className={joinClassName(className, props.className)}
        type={password ? "password" : "text"}
        placeholder={props.placeholder ?? displayName}
        value={value}
        onInput={(event) => onValueChanged((event.target as HTMLInputElement).value)}
      />
    );
  }

  return <Input render={renderInput} />;
}