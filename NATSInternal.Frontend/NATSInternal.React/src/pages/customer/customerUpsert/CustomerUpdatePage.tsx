import React, { useState, useCallback } from "react";
import { useNavigate, useLoaderData } from "react-router";
import { useApi } from "@/api";
import { createCustomerUpsertModel } from "@/models/customers/customerUpsertModel";
import { useRouteHelper } from "@/helpers";

// Child components.
import CustomerUpsertPage from "./CustomerUpsertPage";

// Loader.
export async function loadDataAsync(id: string): Promise<CustomerUpsertModel> {
  const api = useApi();
  const responseDto = await api.customer.getDetailAsync(id);
  return createCustomerUpsertModel(responseDto);
}

// Component.
export default function CustomerUpdatePage(): React.ReactNode {
  // Dependencies.
  const navigate = useNavigate();
  const api = useApi();
  const initialModel = useLoaderData<CustomerUpsertModel>();
  const { getCustomerListRoutePath, getCustomerDetailRoutePath } = useRouteHelper();

  // States.
  const [model, setModel] = useState(() => initialModel);

  // Callbacks.
  const handleUpdate = async (): Promise<void> => {
    await api.customer.updateAsync(model.id, model.toRequestDto());
  };

  const handleDelete = useCallback(async () => {
    await api.customer.deleteAsync(model.id);
  }, [model.id]);

  const handleUpdatingSucceeded = useCallback((): void => {
    navigate(getCustomerDetailRoutePath(model.id));
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
      isForCreating={true}
      model={model}
      onModelChanged={(changedData) => setModel(m => ({ ...m, ...changedData }))}
      upsertAction={handleUpdate}
      onUpsertingSucceeded={handleUpdatingSucceeded}
      deleteAction={handleDelete}
      onDeletionSucceeded={handleDeletionSucceeded}
    />
  );
}