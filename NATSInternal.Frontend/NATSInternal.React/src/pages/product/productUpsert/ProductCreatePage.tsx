import React, { useState } from "react";
import { flushSync } from "react-dom";
import { useNavigate, useLoaderData } from "react-router";
import { api } from "@/api";
import { createProductUpsertModel } from "@/models";

// Child components.
import ProductUpsertPage from "./ProductUpsertPage";
import { loadCategoryOptionsAsync } from "./ProductUpsertPage";

// Data loader.
export async function loadDataAsync(): Promise<ProductCategoryBasicModel[]> {
  return await loadCategoryOptionsAsync();
}

// Components.
export default function ProductCreatePage(): React.ReactNode {
  // Dependencies.
  const navigate = useNavigate();
  const categoryModels = useLoaderData<ProductCategoryBasicModel[]>();

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
      categoryModels={categoryModels}
      upsertAction={handleUpsertAsync}
      onUpsertingSucceeded={handleUpsertingSucceeded}
    />
  );
}
