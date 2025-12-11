import React from "react";

// Child components.
import List from "./List";
import PickedIntroducerInfo from "./PickedIntroducerInfo";
import { Button, BaseModal } from "@/components/ui";

// Props.
export type ModalProps = {
  model: CustomerBasicModel | null;
  isVisible: boolean;
  onPicked(customer: CustomerListCustomerModel): any;
  onUnpicked(): any;
  onCancel(): any;
};

export default function Modal(props: ModalProps): React.ReactNode {
  // Template
  return (
    <BaseModal
      isOpen={props.isVisible}
      onClosed={props.onCancel}
      title="Chọn người giới thiệu"
      footerChildren={(
        <Button className="h-fit" onClick={props.onCancel}>Huỷ bỏ</Button>
      )}
    >
      {props.model ? (
        <PickedIntroducerInfo
          model={props.model}
          onUnpicked={props.onUnpicked}
        />
      ) : (
        <List onPicked={props.onPicked} />
      )}
    </BaseModal>
  );
}