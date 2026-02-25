import React, { useState } from "react";
import { useNavigate, useLoaderData } from "react-router";
import { useApi } from "@/api";

// Child components.
import BrandUpsertPage from "./BrandUpsertPage";

// Components.
export default function BrandUpdatePage(): React.ReactNode {
  // Dependencies.
  const api = useApi();
  const navigate = useNavigate();
  
  // States.
  const initialModel = useLoaderData<BrandUpsertModel>();
  const [model, setModel] = useState<BrandUpsertModel>(initialModel);

  // Callbacks.
  async function handleUpsertAsync(): Promise<void> {
    await api.brand.updateAsync(model.id, model.toUpdateRequestDto());
  }

  function handleUpsertingSucceededAsync(): void {
    navigate(model.detailRoutePath);
  }

  // Template.
  return (
    <BrandUpsertPage
      isForCreating={false}
      model={model}
      onModelUpdated={(updatedData) => setModel(m => ({ ...m, ...updatedData }))}
      upsertAction={handleUpsertAsync}
      onUpsertingSucceeded={handleUpsertingSucceededAsync}
    />
  );
}