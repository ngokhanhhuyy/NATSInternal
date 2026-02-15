import { mergeProps, splitProps } from "solid-js";
import { useTsxHelper } from "@/helpers";

// Child components.
import Spinner from "./Spinner";

// Props.
type ButtonProps = { showSpinner?: boolean } & JSX.HTMLButtonAttributes;

export default function Button(props: ButtonProps) {
  // Props.
  const mergedProps = mergeProps({ type: "button" }, props);
  const [localProps, otherProps] = splitProps(mergedProps, ["type", "showSpinner"]);

  // Dependencies.
  const { joinClassName: joinClass } = useTsxHelper();

  // Template.
  return (
    <button
      {...otherProps}
      type={localProps.type as 'button' | 'submit' | 'reset' | 'menu'}
      class={joinClass(otherProps.class, "button")}
    >
      {localProps.showSpinner && <Spinner size="sm" />}
      {props.children}
    </button>
  );
}