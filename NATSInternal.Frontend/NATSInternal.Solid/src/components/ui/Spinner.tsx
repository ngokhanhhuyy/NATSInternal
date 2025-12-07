import { createMemo, mergeProps, splitProps } from "solid-js";
import { useTsxHelper } from "@/helpers";
import styles from "./Spinner.module.css";

type SpinnerProps = {
  size?: "lg" | "md" | "sm" | "xs";
  variant?: string;
  shadow?: boolean;
} & JSX.HTMLDivAttributes;

export default function Spinner(props: SpinnerProps) {
  // Props.
  const mergedProps = mergeProps({ size: "md", variant: "indigo", shadow: false }, props);
  const [localProps, otherProps] = splitProps(mergedProps, ["size", "variant", "shadow"]);

  // Dependencies.
  const { joinClass } = useTsxHelper();

  // Computed.
  const getShadowClass = createMemo<string | undefined>(() => localProps.shadow ? "shadow-sm" : undefined);

  // Template.
  return (
    <div
      {...otherProps}
      class={joinClass(
        otherProps.class,
        styles[localProps.size],
        localProps.variant,
        styles.spinner,
        getShadowClass(),
        "spinner mx-2"
      )}
    />
  );
}