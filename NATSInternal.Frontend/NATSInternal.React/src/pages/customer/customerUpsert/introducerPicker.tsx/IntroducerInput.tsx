import React, { useState, useCallback } from "react";
import { createCustomerBasicModelFromCustomerListCustomerModel } from "@/models";
import { useTsxHelper } from "@/helpers";

// Child component.
import Modal from "./Modal";
import { TextInput, type TextInputProps } from "@/components/form";

// Props.
export type IntroducerInputProps = {
  value: CustomerBasicModel | null;
  onValueChanged(changedModel: CustomerBasicModel | null): any;
} & Omit<TextInputProps, "value" | "onValueChanged">;

// Component.
export default function IntroducerInput(props: IntroducerInputProps): React.ReactNode {
  // Props.
  const { value, onValueChanged: onModelChanged, ...domProps } = props;

  // Dependencies.
  const { joinClassName } = useTsxHelper();

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
      <TextInput
        {...domProps}
        className={joinClassName(value == null && "text-black/50 dark:text-white/50")}
        value={value?.fullName ?? "Chưa chọn người giới thiệu"}
        onValueChanged={() => undefined}
        onClick={() => setIsModalVisible(v => !v)} readOnly
      />

      <Modal
        model={value}
        isVisible={isModalVisible}
        onPicked={handlePicked}
        onUnpicked={handleUnpicked}
        onCancel={handleCancel}
      />
    </>
  );
}