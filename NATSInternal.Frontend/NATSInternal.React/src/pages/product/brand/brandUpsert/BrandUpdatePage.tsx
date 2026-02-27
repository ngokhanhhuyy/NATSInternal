import React, { useState } from "react";
import { useNavigate, useLoaderData } from "react-router";
import { useApi } from "@/api";
import { useRouteHelper } from "@/helpers";

// Child components.
import BrandUpsertPage from "./BrandUpsertPage";

// Components.
export default function BrandUpdatePage(): React.ReactNode {
  // Dependencies.
  const api = useApi();
  const navigate = useNavigate();
    const { getBrandListRoutePath } = useRouteHelper();
  
  // States.
  const initialModel = useLoaderData<BrandUpsertModel>();
  const [model, setModel] = useState<BrandUpsertModel>(initialModel);

  // Callbacks.
  async function handleUpsertAsync(): Promise<void> {
    await api.brand.updateAsync(model.id, model.toUpdateRequestDto());
  }

  function handleUpsertingSucceeded(): void {
    navigate(model.detailRoutePath);
  }

  async function handleDeleteAysnc(): Promise<void> {
    await api.brand.deleteAsync(model.id);
  }

  function handleDeletionSucceeded(): void {
    navigate(getBrandListRoutePath());
  }

  // Template.
  return (
    <BrandUpsertPage
      isForCreating={false}
      model={model}
      onModelUpdated={(updatedData) => setModel(m => ({ ...m, ...updatedData }))}
      upsertAction={handleUpsertAsync}
      onUpsertingSucceeded={handleUpsertingSucceeded}
      deleteAction={handleDeleteAysnc}
      onDeletionSucceeded={handleDeletionSucceeded}
    />
  );
}