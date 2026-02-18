// Props.
export type TextInputProps = {
  password?: boolean;
  value: string;
  onValueChanged(newValue: string): any;
} & JSX.IntrinsicElements["input"]

// Component.
export default function TextInput(props: TextInputProps) {
  const template = (
    <Show when={{}}>
      {value => }
    </Show>
  );

  // Template.
  return (
    <input
      class="form-control"
      type={props.password ? "password" : "text"}
      value={props.value}
      onInput={(event) => props.onValueChanged(event.target.value)}
    />
  );
}