import React, { useState, useRef, useImperativeHandle, forwardRef } from "react";
import { useTsxHelper } from "@/helpers";

// Child components.
import { BaseModal, Button } from "@/components/ui";

// Props.
type ConfirmationModalProps = {
  title: string;
  IconComponent(props: React.ComponentPropsWithoutRef<"svg">): React.ReactNode;
  iconClassName?: string;
  informationContent: string | string[];
  okButtonText?: string;
  okButtonClassName?: string;
};

// Handler.
export type ConfirmationModalHandler = {
  confirmAsync(): Promise<void>;
};

// Components.
const ConfirmationModal = forwardRef<ConfirmationModalHandler, ConfirmationModalProps>((props, ref) => {
  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // States.
  const [isOpen, setIsOpen] = useState<boolean>(false);
  const confirmationPromise = useRef<((value: void | PromiseLike<void>) => void) | null>(null);

  // Callbacks.
  const handleOkButtonClicked = () => {
    setIsOpen(false);
  };

  const handleOpenOrCloseTransitionEnded = () => {
    if (!isOpen) {
      confirmationPromise.current?.();
    }
  };
  
  // Handler.
  useImperativeHandle(ref, () => ({
    confirmAsync() {
      setIsOpen(true);
      return new Promise<void>(resolve => confirmationPromise.current = resolve);
    },
  }));
  
  // Template.
  const footerChildren = (
    <Button className={joinClassName("h-fit", props.okButtonClassName)} onClick={handleOkButtonClicked}>
      {props.okButtonText ?? "Đã hiểu"}
    </Button>
  );

  return (
    <BaseModal
      isOpen={isOpen}
      onClosed={() => setIsOpen(false)}
      onOpenOrCloseTransitionEnded={handleOpenOrCloseTransitionEnded}
      title={props.title}
      footerChildren={footerChildren}
    >
      <div className="grid grid-cols-[auto_1fr] items-center gap-4 p-5">
        <props.IconComponent className={joinClassName("size-8 shrink-0 grow-0", props.iconClassName)} />
        <div className="flex flex-col">
          {props.informationContent && (Array.isArray(props.informationContent)
              ? props.informationContent.map((sentence, index) => <span key={index}>{sentence}</span>)
              : <span>{props.informationContent}</span>
          )}
        </div>
      </div>
    </BaseModal>
  );
});

ConfirmationModal.displayName = "ConfirmationModal";
export default ConfirmationModal;