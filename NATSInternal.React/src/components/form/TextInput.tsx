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
  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // Template.
  function renderInput(className: string | undefined) {
    return (
      <>
        <input
          {...props}
          className={joinClassName(props.className, className)}
          type={props.password ? "password" : "text"}
          value={props.value}
          onInput={(event) => props.onValueChanged((event.target as HTMLInputElement).value)}
        />
      </>
    );
  }

  return <Input render={renderInput} />;
}