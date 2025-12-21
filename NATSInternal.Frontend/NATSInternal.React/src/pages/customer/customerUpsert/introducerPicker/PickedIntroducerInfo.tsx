import React from "react";

// Child component.
import { Button, NewTabLink } from "@/components/ui";
import { XMarkIcon } from "@heroicons/react/24/solid";

// Props
export type PickedIntroducerInfoProps = {
  model: CustomerBasicModel;
  onUnpicked(): any;
};

// Component.
export default function PickedIntroducerInfo(props: PickedIntroducerInfoProps): React.ReactNode {
  // Template.
  return (
    <div className="flex justify-between items-center w-full gap-3">
      <div className="flex flex-col">
        <NewTabLink className="text-lg font-bold" href={props.model.detailRoute}>
          {props.model.fullName}
        </NewTabLink>
        <span className="opacity-50 text-sm">{props.model.nickName}</span>
      </div>

      <Button className="btn-danger outline-only btn-sm aspect-square shrink-0 grow-0" onClick={props.onUnpicked}>
        <XMarkIcon className="size-4.5" />
      </Button>
    </div>
  );
}