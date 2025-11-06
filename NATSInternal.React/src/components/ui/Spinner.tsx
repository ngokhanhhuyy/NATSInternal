import { useTsxHelper } from "@/helpers";
import styles from "./Spinner.module.css";

type SpinnerProps = {
  size?: "lg" | "md" | "sm" | "xs";
  variant?: ColorVariant;
  shadow?: boolean;
} & React.ComponentPropsWithoutRef<"div">;

export default function Spinner(props: SpinnerProps) {
  // Props.
  const { size = "md", variant = "indigo", shadow = false, ...domProps } = props;

  // Dependencies.
  const { joinClassName, compute } = useTsxHelper();

  // Computed.
  const shadowClassName = compute(() => shadow ? "shadow-sm" : undefined);

  // Template.
  return (
    <div
      {...domProps}
      className={joinClassName(
        props.className,
        styles[size],
        variant,
        styles.spinner,
        shadowClassName,
        "spinner mx-2"
      )}
    />
  );
}