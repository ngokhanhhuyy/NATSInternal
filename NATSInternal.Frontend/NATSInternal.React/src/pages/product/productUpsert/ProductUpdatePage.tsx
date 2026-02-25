import React, { useState } from "react";
import { useNavigate, useLoaderData } from "react-router";
import { useApi } from "@/api";
import { createProductUpsertModel } from "@/models";

// Child components.
import ProductUpsertPage from "./ProductUpsertPage";
import { loadBrandAndCategoryOptionsAsync, type ProductUpsertInitialLoadedModels } from "./ProductUpsertPage";

// Data loader.
type ProductUpdateInitialLoadedModels = ProductUpsertInitialLoadedModels & { updateModel: ProductUpsertModel; }; 
export async function loadDataAsync(id: string): Promise<ProductUpdateInitialLoadedModels> {
  const api = useApi();
  const [responseDto, optionModels] = await Promise.all([
    api.product.getDetailAsync(id),
    loadBrandAndCategoryOptionsAsync()
  ]);

  return {
    updateModel: createProductUpsertModel(responseDto),
    ...optionModels
  };
};

// Components.
export default function ProductUpdatePage(): React.ReactNode {
  // Dependencies.
  const navigate = useNavigate();
  const api = useApi();

  // States.
  const initialModels = useLoaderData<ProductUpdateInitialLoadedModels>();
  const [model, setModel] = useState<ProductUpsertModel>(() => initialModels.updateModel);

  // Callbacks.
  async function handleUpsertAsync(): Promise<void> {
    await api.product.updateAsync(model.id, model.toUpdateRequestDto());
  }

  function handleUpsertingSucceeded(): void {
    navigate(model.detailRoutePath);
  }

  // Template.
  return (
    <ProductUpsertPage
      isForCreating={false}
      model={model}
      onModelUpdated={(changedData) => setModel(m => ({ ...m, ...changedData }))}
      upsertAction={handleUpsertAsync}
      onUpsertingSucceeded={handleUpsertingSucceeded}
    />
  );
}