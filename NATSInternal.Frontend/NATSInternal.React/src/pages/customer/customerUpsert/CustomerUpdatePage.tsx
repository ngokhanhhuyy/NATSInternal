import React, { useState, useCallback } from "react";
import { useNavigate, useLoaderData } from "react-router";
import { useApi } from "@/api";
import { createCustomerUpsertModel } from "@/models/customers/customerUpsertModel";

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

  // States.
  const [model, setModel] = useState(() => initialModel);

  // Callbacks.
  const handleSubmit = async (): Promise<void> => {
    await api.customer.updateAsync(model.id, model.toRequestDto());
  };

  const handleSubmissionSucceeded = useCallback((): void => {
    navigate(model.id);
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
      submitAction={handleSubmit}
      onSubmissionSucceeded={handleSubmissionSucceeded}
    />
  );
}