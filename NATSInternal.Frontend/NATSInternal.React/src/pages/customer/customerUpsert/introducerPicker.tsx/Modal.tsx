import React, { useEffect } from "react";
import { createPortal } from "react-dom";
import { useTsxHelper } from "@/helpers";

// Child components.
import List from "./List";
import PickedIntroducerInfo from "./PickedIntroducerInfo";
import { Button } from "@/components/ui";

// Props.
export type ModalProps = {
  model: CustomerBasicModel | null;
  isVisible: boolean;
  onPicked(customer: CustomerListCustomerModel): any;
  onUnpicked(): any;
  onCancel(): any;
};

export default function Modal(props: ModalProps): React.ReactNode {
  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // Effect.
  useEffect(() => {
    const handleKeyDown = (event: KeyboardEvent) => {
      if (event.key === "Escape") {
        props.onCancel();
      }
    };

    document.addEventListener("keydown", handleKeyDown);
    return () => {
      document.removeEventListener("keydown", handleKeyDown);
    };
  }, [props.onCancel]);

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
        "rounded-xl shadow w-full max-w-sm mx-3 sm:mx-auto",
        "transition-all",
        props.isVisible ? "scale-100" : "scale-0"
      )}>
        {/* Header */}
        <div className="text-sm font-bold opacity-75 p-3">
          {"Chọn người giới thiệu".toUpperCase()}
        </div>

        {/* Body */}
        <div className="border-y border-black/10 dark:border-white/10 p-3">
          {props.model ? (
            <PickedIntroducerInfo
              model={props.model}
              onUnpicked={props.onUnpicked}
            />
          ): (
            <List onPicked={props.onPicked} />
          )}
        </div>

        {/* Footer */}
        <div className="flex justify-end gap-3 p-3">
          <Button className="h-fit" onClick={props.onCancel}>Huỷ bỏ</Button>
        </div>
      </div>
    </div>
  ), document.getElementById("root")!);
}