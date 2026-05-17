import React, { useState } from "react";
import { useNavigate } from "react-router";
import { createProductCategoryUpsertModel } from "@/models";
import { getProductCategoryDetailRoutePath } from "@/helpers";

// Child components.
import ProductCategoryUpsertPage from "./ProductCategoryUpsertPage";
import { api } from "@/api";

// Components.
export default function ProductCategoryCreatePage(): React.ReactNode {
  // Dependencies.
  const navigate = useNavigate();

  // States.
  const [model, setModel] = useState<ProductCategoryUpsertModel>(createProductCategoryUpsertModel);

  // Callbacks.
  async function handleUpsertAsync(): Promise<number> {
    return await api.productCategory.createAsync(model.toUpdateRequestDto());
  }

  function handleUpsertingSucceeded(id: number): void {
    navigate(getProductCategoryDetailRoutePath(id));
  }

  // Template.
  return (
    <ProductCategoryUpsertPage
      isForCreating={true}
      model={model}
      onModelUpdated={(updatedData) => setModel(m => ({ ...m, ...updatedData }))}
      upsertAction={handleUpsertAsync}
      onUpsertingSucceeded={handleUpsertingSucceeded}
    />
  );
}
