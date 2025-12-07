import React, { useState, useCallback } from "react";
import { createCustomerBasicModelFromCustomerListCustomerModel } from "@/models";

// Child component.
import Modal from "./Modal";
import { Button } from "@/components/ui";

// Props.
export type IntroducerInputProps = {
  model: CustomerBasicModel | null;
  onModelChanged(changedModel: CustomerBasicModel | null): any;
} & React.ComponentPropsWithoutRef<"button">;

// Component.
export default function IntroducerInput({ model, onModelChanged, ...props }: IntroducerInputProps): React.ReactNode {
  // States.
  const [isModalVisible, setIsModalVisible] = useState<boolean>(false);

  // Callbacks.
  const handlePicked = useCallback((customer: CustomerListCustomerModel) => {
    const basicModel = createCustomerBasicModelFromCustomerListCustomerModel(customer);
    setTimeout(() => onModelChanged(basicModel), 110);
    setIsModalVisible(false);
  }, [onModelChanged]);

  const handleUnpicked = useCallback(() => {
    setTimeout(() => onModelChanged(null), 110);
    setIsModalVisible(false);
  }, []);

  const handleCancel = useCallback(() => {
    setIsModalVisible(false);
  }, []);

  // Template.
  return (
    <>
      <Button {...props} onClick={() => setIsModalVisible(v => !v)}>
        {model?.fullName ?? "Chưa chọn"}
      </Button>

      <Modal
        model={model}
        isVisible={isModalVisible}
        onPicked={handlePicked}
        onUnpicked={handleUnpicked}
        onCancel={handleCancel}
      />
    </>
  );
}