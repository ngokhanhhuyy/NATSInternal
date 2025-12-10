import React, { useEffect } from "react";
import { createPortal } from "react-dom";
import { useTsxHelper } from "@/helpers";

// Props.
type BaseModalProps = {
  isVisible: boolean;
  onVisibilityChanged(isVisible: boolean): any;
  title?: string;
  headerContent?: React.ReactNode | React.ReactNode[];
  bodyContent?: React.ReactNode | React.ReactNode[];
  footerContent?: React.ReactNode | React.ReactNode[];
  closeOnEscapeKeyDown?: boolean;
};

export default function Modal(props: BaseModalProps): React.ReactNode {
  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // Effect.
  useEffect(() => {
    const handleKeyDown = (event: KeyboardEvent) => {
      if ((props.closeOnEscapeKeyDown ?? true) && event.key === "Escape") {
        props.onVisibilityChanged(false);
      }
    };

    document.addEventListener("keydown", handleKeyDown);
    return () => {
      document.removeEventListener("keydown", handleKeyDown);
    };
  }, [props.closeOnEscapeKeyDown]);

  // Template.
  return createPortal((
    <div
      id="customer-introducer-picker-modal"
      className={joinClassName(
        "bg-black/50 w-screen h-screen flex justify-center items-center z-1000",
        "fixed top-0 left-0 backdrop-blur-md transition-opacity",
        props.isVisible ? "opacity-100" : "opacity-0 pointer-events-none"
      )}
    >
      <div className={joinClassName(
        "bg-white dark:bg-neutral-800 border border-transparent dark:border-white/10",
        "rounded-xl shadow w-full max-w-sm mx-3 sm:mx-auto transition-all",
        props.isVisible ? "scale-100" : "scale-0"
      )}>
        {/* Header */}
        <div className="flex justify-between items-center p-3">
          <div className="text-sm font-bold opacity-75 p-3">
            {props.title && props.title.toUpperCase()}
          </div>

          {props.headerContent}
        </div>

        {/* Body */}
        <div className="border-y border-black/10 dark:border-white/10 p-3">
          {props.bodyContent}
        </div>

        {/* Footer */}
        <div className="flex justify-end gap-3 p-3">
          {props.footerContent}
        </div>
      </div>
    </div>
  ), document.getElementById("root")!);
}