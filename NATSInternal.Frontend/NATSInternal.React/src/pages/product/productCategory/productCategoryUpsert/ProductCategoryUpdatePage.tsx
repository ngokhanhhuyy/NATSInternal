import React, { useState } from "react";
import { useNavigate, useLoaderData } from "react-router";
import { useApi } from "@/api";
// import { useRouteHelper } from "@/helpers";

// Child components.
import ProductCategoryUpsertPage from "./ProductCategoryUpsertPage";

// Components.
export default function ProductCategoryUpdatePage(): React.ReactNode {
  // Dependencies.
  const api = useApi();
  const navigate = useNavigate();
  // const { getProductCategoryListRoutePath } = useRouteHelper();
  
  // States.
  const initialModel = useLoaderData<ProductCategoryUpsertModel>();
  const [model, setModel] = useState<ProductCategoryUpsertModel>(initialModel);

  // Callbacks.
  async function handleUpsertAsync(): Promise<void> {
    await api.productCategory.updateAsync(model.id, model.toUpdateRequestDto());
  }

  function handleUpsertingSucceeded(): void {
    navigate(model.detailRoutePath);
  }

  // async function handleDeleteAysnc(): Promise<void> {
  //   await api.productCategory.deleteAsync(model.id);
  // }

  // function handleDeletionSucceeded(): void {
  //   navigate(getProductCategoryListRoutePath());
  // }

  // Template.
  return (
    <ProductCategoryUpsertPage
      isForCreating={false}
      model={model}
      onModelUpdated={(updatedData) => setModel(m => ({ ...m, ...updatedData }))}
      upsertAction={handleUpsertAsync}
      onUpsertingSucceeded={handleUpsertingSucceeded}
    />
  );
}