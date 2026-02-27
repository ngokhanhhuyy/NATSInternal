import React, { useState } from "react";
import { useNavigate } from "react-router";
import { useApi } from "@/api";
import { createBrandUpsertModel } from "@/models";
import { useRouteHelper } from "@/helpers";

// Child components.
import BrandUpsertPage from "./BrandUpsertPage";

// Components.
export default function BrandCreatePage(): React.ReactNode {
  // Dependencies.
  const api = useApi();
  const navigate = useNavigate();
  const { getBrandDetailRoutePath } = useRouteHelper();
  
  // States.
  const [model, setModel] = useState<BrandUpsertModel>(createBrandUpsertModel);

  // Callbacks.
  async function handleUpsertAsync(): Promise<string> {
    return await api.brand.createAsync(model.toCreateRequestDto());
  }

  function handleUpsertingSucceeded(createdId: string): void {
    navigate(getBrandDetailRoutePath(createdId));
  }

  // Template.
  return (
    <BrandUpsertPage
      isForCreating={true}
      model={model}
      onModelUpdated={(updatedData) => setModel(m => ({ ...m, ...updatedData }))}
      upsertAction={handleUpsertAsync}
      onUpsertingSucceeded={handleUpsertingSucceeded}
    />
  );
}