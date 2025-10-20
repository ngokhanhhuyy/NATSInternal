import Input from "./Input";
import { useTsxHelper } from "@/helpers";

// Props.
export type TextInputProps = {
  password?: boolean;
  value: string;
  onValueChanged(newValue: string): any;
} & JSX.IntrinsicElements["input"]

// Component.
export default function TextInput(props: TextInputProps) {
  // Dependencies.
  const htmlHelper = useTsxHelper();

  // Template.
  function renderInput(getClassName: () => string | undefined) {
    return (
      <input
        class={htmlHelper.joinClassName(getClassName(), "form-control")}
        type={props.password ? "password" : "text"}
        value={props.value}
        onInput={(event) => props.onValueChanged(event.target.value)}
      />
    );
  }

  return <Input render={renderInput} />;
}