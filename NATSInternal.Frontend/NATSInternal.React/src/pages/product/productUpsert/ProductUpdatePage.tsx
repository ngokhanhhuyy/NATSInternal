import React, { useState } from "react";
import { useNavigate, useLoaderData } from "react-router";
import { api } from "@/api";
import { createProductUpsertModel } from "@/models";

// Child components.
import ProductUpsertPage from "./ProductUpsertPage";
import { loadCategoryOptionsAsync } from "./ProductUpsertPage";

// Data loader.
type ProductUpdateInitialLoadedModels = { model: ProductUpsertModel; categoryModels: ProductCategoryBasicModel[]; }; 
export async function loadDataAsync(id: number): Promise<ProductUpdateInitialLoadedModels> {
  const [responseDto, categoryModels] = await Promise.all([
    api.product.getDetailAsync(id),
    loadCategoryOptionsAsync()
  ]);

  return {
    model: createProductUpsertModel(responseDto),
    categoryModels
  };
}

// Components.
export default function ProductUpdatePage(): React.ReactNode {
  // Dependencies.
  const navigate = useNavigate();

  // States.
  const initialModels = useLoaderData<ProductUpdateInitialLoadedModels>();
  const [model, setModel] = useState<ProductUpsertModel>(() => initialModels.model);

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
      categoryModels={initialModels.categoryModels}
      upsertAction={handleUpsertAsync}
      onUpsertingSucceeded={handleUpsertingSucceeded}
    />
  );
}
