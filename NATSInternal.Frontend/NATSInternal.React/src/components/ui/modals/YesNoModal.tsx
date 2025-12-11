import React, { useState, useRef, useImperativeHandle, forwardRef } from "react";
import { useTsxHelper } from "@/helpers";

// Child component.
import BaseModal from "./BaseModal";
import { Button } from "@/components/ui";

// Props.
type YesNoModalProps = {
  title: string;
  IconComponent(props: React.ComponentPropsWithoutRef<"svg">): React.ReactNode;
  iconClassName?: string;
  questionContent: string | string[];
  yesButtonText?: string;
  yesButtonClassName?: string;
  noButtonText?: string;
  noButtonClassName?: string;
};

export type YesNoModalHandler = {
  getAnswerAsync(): Promise<boolean>;
};

// Component.
const YesNoModal = forwardRef<YesNoModalHandler, YesNoModalProps>((props, ref): React.ReactNode => {
  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // States.
  const [isOpen, setIsOpen] = useState(false);
  const answerPromise = useRef<((value: boolean | PromiseLike<boolean>) => void) | null>(null);

  // Callbacks.
  const handleYesButtonClicked = () => {
    setIsOpen(false);
    setTimeout(() => {
      answerPromise.current?.(true);
    }, 200);

  };

  const handleNoButtonClicked = () => {
    setIsOpen(false);
    setTimeout(() => {
      answerPromise.current?.(false);
    }, 200);
  };

  // Handler.
  useImperativeHandle(ref, () => ({
    getAnswerAsync(): Promise<boolean> {
      setIsOpen(true);
      return new Promise<boolean>(resolve => answerPromise.current = resolve);
    },
  }));

  // Template.
  const footerChildren = (
    <>
      <Button className={joinClassName("h-fit", props.noButtonClassName)} onClick={handleNoButtonClicked}>
        {props.noButtonText ?? "Không đồng ý"}
      </Button>

      <Button className={joinClassName("h-fit", props.yesButtonClassName)} onClick={handleYesButtonClicked}>
        {props.yesButtonText ?? "Đồng ý"}
      </Button>
    </>
  );

  return (
    <BaseModal
      isOpen={isOpen}
      onClosed={() => setIsOpen(false)}
      title={props.title}
      footerChildren={footerChildren}
    >
      <div className="grid grid-cols-[auto_1fr] items-center gap-4 p-5">
        <props.IconComponent className={joinClassName("size-8 shrink-0 grow-0", props.iconClassName)} />
        <div className="flex flex-col">
          {props.questionContent && (Array.isArray(props.questionContent)
              ? props.questionContent.map((sentence, index) => <span key={index}>{sentence}</span>)
              : <span>{props.questionContent}</span>
          )}
        </div>
      </div>
    </BaseModal>
  );
});
YesNoModal.displayName = "YesNoModal";

export default YesNoModal;