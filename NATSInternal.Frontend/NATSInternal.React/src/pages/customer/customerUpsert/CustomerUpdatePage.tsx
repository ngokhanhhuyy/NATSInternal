import React, { useState, useCallback } from "react";
import { useNavigate, useLoaderData } from "react-router";
import { api } from "@/api";
import { createCustomerUpsertModel } from "@/models/customer/customerUpsertModel";
import { getCustomerListRoutePath, getCustomerDetailRoutePath } from "@/helpers";

// Child components.
import CustomerUpsertPage from "./CustomerUpsertPage";

// Loader.
export async function loadDataAsync(id: number): Promise<[CustomerUpsertModel, number]> {
  const responseDto = await api.customer.getDetailAsync(id);
  const model = createCustomerUpsertModel(responseDto);
  return [model, id];
}

// Component.
export default function CustomerUpdatePage(): React.ReactNode {
  // Dependencies.
  const navigate = useNavigate();
  const [initialModel, id] = useLoaderData<[CustomerUpsertModel, number]>();

  // States.
  const [model, setModel] = useState(() => initialModel);

  // Callbacks.
  const handleUpdate = async (): Promise<void> => {
    await api.customer.updateAsync(id, model.toRequestDto());
  };

  const handleDelete = useCallback(async () => {
    await api.customer.deleteAsync(id);
  }, [id]);

  const handleUpdatingSucceeded = useCallback((): void => {
    navigate(getCustomerDetailRoutePath(id));
  }, []);

  const handleDeletionSucceeded = useCallback((): void => {
    navigate(getCustomerListRoutePath());
  }, []);

  // Template.
  return (
    <CustomerUpsertPage
      description={
        "Chỉnh sửa một bản ghi dữ liệu của một khách hàng đang tồn tại, " +
        "bao gồm thông tin cá nhân và người giới thiệu (nếu có)."
      }
      isForCreating={false}
      model={model}
      onModelChanged={(changedData) => setModel(m => ({ ...m, ...changedData }))}
      upsertAction={handleUpdate}
      onUpsertingSucceeded={handleUpdatingSucceeded}
      deleteAction={handleDelete}
      onDeletionSucceeded={handleDeletionSucceeded}
    />
  );
}
