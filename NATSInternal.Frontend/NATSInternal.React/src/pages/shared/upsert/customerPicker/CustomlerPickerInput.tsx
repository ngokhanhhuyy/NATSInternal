import React, { useState, useMemo, useCallback } from "react";
import { getDisplayName } from "@/metadata";
import { joinClassName } from "@/helpers";

// Child component.
import Modal from "./Modal";
import { TextInput, type TextInputProps } from "@/components/form";

// Props.
export type CustomerPickerProps = {
  resourceName: string;
  value: CustomerBasicModel | null;
  onValueChanged(changedModel: CustomerBasicModel | null): any;
  excludedId: number | null;
} & Omit<TextInputProps, "value" | "onValueChanged">;

// Component.
export default function CustomerPickerInput(props: CustomerPickerProps): React.ReactNode {
  // Props.
  const { value, onValueChanged, excludedId, resourceName, ...domProps } = props;

  // States.
  const [isModalVisible, setIsModalVisible] = useState<boolean>(false);

  // Computed.
  const displayName = useMemo<string>(() => getDisplayName(props.resourceName) ?? props.resourceName, []);

  // Callbacks.
  const handlePicked = useCallback((customer: CustomerBasicModel) => {
    setTimeout(() => onValueChanged(customer), 110);
    setIsModalVisible(false);
  }, [onValueChanged]);

  const handleUnpicked = useCallback(() => {
    setTimeout(() => onValueChanged(null), 110);
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
        value={value?.fullName ?? `Chưa chọn ${displayName}`}
        onValueChanged={() => undefined}
        onClick={() => setIsModalVisible(v => !v)} readOnly
      />

      <Modal
        displayName={displayName}
        model={value}
        excludedId={excludedId}
        isVisible={isModalVisible}
        onPicked={handlePicked}
        onUnpicked={handleUnpicked}
        onCancel={handleCancel}
      />
    </>
  );
}
