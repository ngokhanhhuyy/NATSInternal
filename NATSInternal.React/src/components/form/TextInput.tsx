import Input from "./Input";
import { useTsxHelper } from "@/helpers";

// Props.
export type TextInputProps = {
  password?: boolean;
  value: string;
  onValueChanged(newValue: string): any;
} & React.ComponentPropsWithoutRef<"div">;

// Component.
export default function TextInput(props: TextInputProps) {
  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // Template.
  function renderInput(className: string | undefined) {
    return (
      <input
        className={joinClassName(className, "form-control")}
        type={props.password ? "password" : "text"}
        value={props.value}
        onInput={(event) => props.onValueChanged((event.target as HTMLInputElement).value)}
      />
    );
  }

  return <Input render={renderInput} />;
}