import React from "react";
import { useTsxHelper } from "@/helpers";

// Props.
type MainBlockProps = {
  title?: string;
  headerContent?: React.ReactNode;
  bodyClassName?: string;
} & React.ComponentPropsWithoutRef<"div">;

// Component.
export default function MainBlock(props: MainBlockProps): React.ReactNode {
  // Props.
  const { title, headerContent, bodyClassName, className, children, ...domProps } = props;

  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // Template.
  return (
    <div
      {...domProps}
      className={joinClassName(
        "border border-black/10 dark:border-white/10 flex flex-col gap-3 p-3 rounded-lg",
        className
      )}
    >
      {title && (
        <div className={joinClassName(
          "bg-black/5 dark:bg-white/5 px-3 py-2 text-sm font-bold col-span-2 rounded-md",
          "text-black/60 dark:text-white/60"
        )}>
          {title.toUpperCase()}
        </div>
      )}

      {children}
    </div>
  );
}