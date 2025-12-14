import React from "react";
import { useTsxHelper } from "@/helpers";

// Props.
export type BlockProps = {
  title: string;
  headerChildren?: React.ReactNode;
  bodyClassName?: string;
  footerChildren?: React.ReactNode;
  footerClassName?: string;
} & React.ComponentPropsWithoutRef<"div">;

// Component.
export default function Block(props: BlockProps): React.ReactNode {
  // Props.
  const {
    title,
    headerChildren,
    bodyClassName,
    footerChildren,
    footerClassName,
    className,
    children,
    ...domProps
  } = props;

  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // Template.
  return (
    <div
      {...domProps}
      className={joinClassName("block-container", "flex flex-col overflow-hidden box-border", className)}
    >
      {/* Header */}
      <div className={joinClassName(
        "block-header bg-blue-500/15 border border-blue-500/50",
        "flex justify-between gap-3 items-center min-h-10",
        "ps-3 pe-1.5 py-1.5 col-span-2 rounded-t-lg",
      )}>
        <span className="text-blue-900/75 dark:text-blue-200/75 font-bold text-sm">
          {title.toUpperCase()}
        </span>
        {props.headerChildren}
      </div>

      {/* Body */}
      <div className={joinClassName(
        "block-body border-x border-blue-800/25 flex-1",
        !props.footerChildren && "border-b rounded-b-lg",
        props.bodyClassName,
      )}>
        {children}
      </div>

      {/* Footer */}
      {props.footerChildren && (
        <div className={joinClassName(
          "block-footer",
          "border border-black/10 dark:border-white/15",
          "rounded-b-lg flex justify-stretch p-3",
          props.footerClassName)}
        >
          {props.footerChildren}
        </div>
      )}
    </div>
  );
}