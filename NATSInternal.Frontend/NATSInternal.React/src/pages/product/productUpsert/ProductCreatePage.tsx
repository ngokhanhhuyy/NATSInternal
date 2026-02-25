import React, { useState } from "react";
import { flushSync } from "react-dom";
import { useNavigate } from "react-router";
import { useApi } from "@/api";
import { createProductUpsertModel } from "@/models";

// Child components.
import ProductUpsertPage from "./ProductUpsertPage";
import { loadBrandAndCategoryOptionsAsync, type ProductUpsertInitialLoadedModels } from "./ProductUpsertPage";

// Data loader.
type ProductCreateInitiaLoadedModels = ProductUpsertInitialLoadedModels; 
export async function loadDataAsync(): Promise<ProductCreateInitiaLoadedModels> {
  return await loadBrandAndCategoryOptionsAsync();
}

// Components.
export default function ProductCreatePage(): React.ReactNode {
  // Dependencies.
  const navigate = useNavigate();
  const api = useApi();

  // States.
  const [model, setModel] = useState<ProductUpsertModel>(createProductUpsertModel);

  // Callbacks.
  async function handleUpsertAsync(): Promise<void> {
    const id = await api.product.createAsync(model.toCreateRequestDto());
    flushSync(() => {
      setModel(m => ({ ...m, id }));
    });
  }

  function handleUpsertingSucceeded(): void {
    navigate(model.detailRoutePath);
  }

  // Template.
  return (
    <ProductUpsertPage
      isForCreating={true}
      model={model}
      onModelUpdated={(changedData) => setModel(m => ({ ...m, ...changedData }))}
      upsertAction={handleUpsertAsync}
      onUpsertingSucceeded={handleUpsertingSucceeded}
    />
  );
}