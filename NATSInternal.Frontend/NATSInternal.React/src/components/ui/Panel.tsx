import React from "react";
import { useTsxHelper } from "@/helpers";

// Props.
export type PanelProps = {
  title: string;
  headerChildren?: React.ReactNode;
  bodyClassName?: string;
  footerChildren?: React.ReactNode;
  footerClassName?: string;
} & React.ComponentPropsWithoutRef<"div">;

// Component.
export default function Panel(props: PanelProps): React.ReactNode {
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
      className={joinClassName("panel", className)}
    >
      {/* Header */}
      <div className="panel-header">
        <span className="font-bold text-sm">
          {title.toUpperCase()}
        </span>
        {props.headerChildren}
      </div>

      {/* Body */}
      <div className={joinClassName(
        "panel-body",
        !props.footerChildren && "border-b rounded-b-lg",
        props.bodyClassName,
      )}>
        {children}
      </div>

      {/* Footer */}
      {props.footerChildren && (
        <div className={joinClassName(
          "panel-footer",
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