import React, { useRef, useCallback } from "react";
import { createPortal } from "react-dom";
import { joinClassName } from "@/helpers";

// Props.
type BaseModalProps = {
  isOpen: boolean;
  onClosed?(): any;
  onOpenOrCloseTransitionEnded?(isOpen: boolean): any;
  title?: string;
  modalClassName?: string;
  headerChildren?: React.ReactNode | React.ReactNode[];
  children?: React.ReactNode | React.ReactNode[];
  footerChildren?: React.ReactNode | React.ReactNode[];
  closeOnEscapeKeyDown?: boolean;
};

export default function BaseModal(props: BaseModalProps) {
  // States.
  const elementRef = useRef<HTMLDivElement | null>(null);

  // Callbacks.
  const handleTransitionEnd = useCallback((event: React.TransitionEvent<HTMLDivElement>) => {
    if (event.target !== event.currentTarget) {
      return;
    }

    if (event.propertyName !== "opacity") {
      return;
    }

    props.onOpenOrCloseTransitionEnded?.(props.isOpen);
  }, [props.onOpenOrCloseTransitionEnded, props.isOpen]);

  const handleKeyDown = useCallback((event: React.KeyboardEvent) => {
    if (event.target !== event.currentTarget) {
      return;
    }

    if ((props.closeOnEscapeKeyDown ?? true) && event.key === "Escape") {
      props.onClosed?.();
    }
  }, [props.closeOnEscapeKeyDown]);

  // Template.
  return createPortal((
    <div
      ref={elementRef}
      id="customer-introducer-picker-modal"
      className={joinClassName(
        "bg-black/75 w-screen h-screen flex justify-center items-center z-1000 contain-[layout-paint] duration-100",
        "fixed top-0 left-0 transition-all will-change-[background] transform-gpu ease-in-out",
        props.isOpen ? "opacity-100 backdrop-blur-md" : "opacity-0 backdrop-blur-none pointer-events-none"
      )}
      onTransitionEnd={handleTransitionEnd}
      onKeyDown={handleKeyDown}
    >
      <div className={joinClassName(
        "bg-white dark:bg-neutral-900 will-change-[transform_opacity] transform-gpu",
        "rounded-xl shadow max-w-sm w-full mx-3 sm:mx-auto transition-[opacity_transform]",
        props.modalClassName
      )}>
        {/* Header */}
        <div className={joinClassName(
          "dark:bg-white/10 flex justify-between items-center p-3 rounded-t-xl",
          "border border-transparent border-b-black/15 dark:border-white/10"
        )}>
          <div className="text-sm font-bold opacity-75">
            {props.title && props.title.toUpperCase()}
          </div>

          {props.headerChildren}
        </div>

        {/* Body */}
        <div className="border-x border-black/10 dark:border-white/10">
          {props.children}
        </div>

        {/* Footer */}
        <div className="flex justify-end gap-2 p-2 border border-black/10 dark:border-white/10 rounded-b-xl">
          {props.footerChildren}
        </div>
      </div>
    </div>
  ), document.body);
}
