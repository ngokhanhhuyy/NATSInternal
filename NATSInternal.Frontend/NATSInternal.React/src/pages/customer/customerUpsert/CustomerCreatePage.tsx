import React, { useState, useCallback } from "react";
import { useNavigate } from "react-router";
import { useApi } from "@/api";
import { createCustomerUpsertModel } from "@/models/customers/customerUpsertModel";

// Child components.
import CustomerUpsertPage from "./CustomerUpsertPage";

// Component.
export default function CustomerCreatePage(): React.ReactNode {
  // Dependencies.
  const navigate = useNavigate();
  const api = useApi();

  // States.
  const [model, setModel] = useState(() => createCustomerUpsertModel());

  // Callbacks.
  const handleCreate = async (): Promise<string> => {
    return await api.customer.createAsync(model.toRequestDto());
  };

  const handleCreatingSucceeded = useCallback((createdId: string): void => {
    navigate(createdId);
  }, []);

  // Template.
  return (
    <CustomerUpsertPage
      description="Tạo bản ghi dữ liệu cho một khách hàng mới, bao gồm thông tin cá nhân và người giới thiệu (nếu có)."
      isForCreating={true}
      model={model}
      onModelChanged={(changedData) => setModel(m => ({ ...m, ...changedData }))}
      upsertAction={handleCreate}
      onUpsertingSucceeded={handleCreatingSucceeded}
    />
  );
}