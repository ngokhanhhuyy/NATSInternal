import React from "react";
import { useTsxHelper } from "@/helpers";

// Props.
export type BlockProps = {
  title: string;
  headerChildren?: React.ReactNode;
  bodyClassName?: string;
} & React.ComponentPropsWithoutRef<"div">;

// Component.
export default function Block(props: BlockProps): React.ReactNode {
  // Props.
  const { title, headerChildren: headerContent, bodyClassName, className, children, ...domProps } = props;

  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // Template.
  return (
    <div
      {...domProps}
      className={joinClassName(
        "flex flex-col",
        className
      )}
    >
      {/* Header */}
      <div className={joinClassName(
        "bg-black/12 dark:bg-white/12 border border-black/10 dark:border-white/10",
        "flex justify-between gap-3 items-center min-h-[50px] ps-3 pe-2 py-2 col-span-2 rounded-t-lg",
      )}>
        <span className="text-black/60 dark:text-white/60 font-bold text-sm">
          {title.toUpperCase()}
        </span>
        {props.headerChildren}
      </div>

      {/* Body */}
      <div className={joinClassName(
        props.bodyClassName,
        "bg-black/3 dark:bg-white/3 border-x border-b border-black/10 dark:border-white/10 rounded-b-lg"
      )}>
        {children}
      </div>
    </div>
  );
}