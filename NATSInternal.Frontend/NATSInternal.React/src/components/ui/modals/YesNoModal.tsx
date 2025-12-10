import React from "react";
import { useTsxHelper } from "@/helpers";

// Child component.
import BaseModal from "./BaseModal";
import { Button } from "@/components/ui";

// Props.
type YesNoModalProps = {
  IconComponent(props: React.ComponentPropsWithoutRef<"svg">): React.ReactNode;
  questionContent: string | string[];
  yesButtonText?: string;
  yesButtonClassName?: string;
  noButtonText?: string;
  noButtonClassName?: string;
  onAnswer(answer: boolean): any;
} & Omit<ComponentProps<typeof BaseModal>, "onHidden">;

// Component.
export default function YesNoModalProps(props: YesNoModalProps): React.ReactNode {
  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // Template.
  const footerChildren = (
    <>
      <Button className={joinClassName("h-fit", props.noButtonClassName)} onClick={() => props.onAnswer(false)}>
        {props.noButtonText ?? "Không đồng ý"}
      </Button>

      <Button className={joinClassName("h-fit", props.yesButtonClassName)} onClick={() => props.onAnswer(true)}>
        {props.yesButtonText ?? "Đồng ý"}
      </Button>
    </>
  );

  return (
    <BaseModal {...props} onHidden={() => props.onAnswer(false)} footerChildren={footerChildren}>
      <div className="grid grid-cols-[auto_1fr] gap-5 p-5">
        <props.IconComponent className="size-8 shrink-0 grow-0 self-center" />
        <div className="flex flex-col">
          {props.questionContent && (Array.isArray(props.questionContent)
              ? props.questionContent.map((sentence, index) => <span key={index}>{sentence}</span>)
              : <span>{props.questionContent}</span>
          )}
        </div>
      </div>
    </BaseModal>
  );
}