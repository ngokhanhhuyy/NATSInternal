import React, { useState, useCallback, useEffect } from "react";
import { useApi } from "@/api";
import { createCustomerListModel, createCustomerBasicModelFromCustomerListCustomerModel } from "@/models";

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
  // Dependencies.
  const api = useApi();

  // States.
  const [listModel, setListModel] = useState<CustomerListModel>(() => {
    const model = createCustomerListModel();
    model.resultsPerPage = 8;
    return model;
  });
  
  const [isInitialLoading, setIsInitialLoading] = useState(true);
  const [isReloading, setIsReloading] = useState(false);
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

  // Effect.
  useEffect(() => {
    const loadListModelAsync = async () => {
      const responseDto = await api.customer.getListAsync(listModel.toRequestDto());
      setListModel(m => m!.mapFromResponseDto(responseDto));
    };

    if (!isInitialLoading) {
      setIsReloading(true);
    }

    loadListModelAsync().finally(() => {
      setIsInitialLoading(false);
      setIsReloading(false);
    });
  }, [listModel?.searchContent, listModel?.page]);

  // Template.
  return (
    <>
      <Button
        {...props}
        disabled={isInitialLoading}
        showSpinner={isInitialLoading}
        onClick={() => setIsModalVisible(v => !v)}
      >
        {model?.fullName ?? "Chưa chọn"}
      </Button>

      {listModel && (
        <Modal
          pickedModel={model}
          listModel={listModel}
          onListModelChanged={(changedData) => setListModel(m => ({ ...m!, ...changedData }))}
          isVisible={isModalVisible}
          isReloading={isReloading}
          onPicked={handlePicked}
          onUnpicked={handleUnpicked}
          onCancel={handleCancel}
        />
      )}
    </>
  );
}