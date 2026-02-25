import React, { useState } from "react";
import { useNavigate } from "react-router";
import { flushSync } from "react-dom";
import { useApi } from "@/api";
import { createBrandUpsertModel } from "@/models";

// Child components.
import BrandUpsertPage from "./BrandUpsertPage";

// Components.
export default function BrandCreatePage(): React.ReactNode {
  // Dependencies.
  const api = useApi();
  const navigate = useNavigate();
  
  // States.
  const [model, setModel] = useState<BrandUpsertModel>(createBrandUpsertModel);

  // Callbacks.
  async function handleUpsertAsync(): Promise<void> {
    const id = await api.brand.createAsync(model.toCreateRequestDto());
    flushSync(() => {
      setModel(m => ({ ...m, id }));
    });
  }

  function handleUpsertingSucceededAsync(): void {
    navigate(model.detailRoutePath);
  }

  // Template.
  return (
    <BrandUpsertPage
      isForCreating={true}
      model={model}
      onModelUpdated={(updatedData) => setModel(m => ({ ...m, ...updatedData }))}
      upsertAction={handleUpsertAsync}
      onUpsertingSucceeded={handleUpsertingSucceededAsync}
    />
  );
}