import React, { useState } from "react";
import { useNavigate, useLoaderData } from "react-router";
import { useApi } from "@/api";
import { createProductUpsertModel } from "@/models";

// Child components.
import ProductUpsertPage from "./ProductUpsertPage";

// Data loader.
export async function loadDataAsync(id: string): Promise<ProductUpsertModel> {
  const api = useApi();
  const responseDto = await api.product.getDetailAsync(id);
  return createProductUpsertModel(responseDto);
}

// Components.
export default function ProductUpdatePage(): React.ReactNode {
  // Dependencies.
  const navigate = useNavigate();
  const api = useApi();

  // States.
  const initialModel = useLoaderData<ProductUpsertModel>();
  const [model, setModel] = useState<ProductUpsertModel>(() => initialModel);

  // Callbacks.
  async function handleUpsertAsync(): Promise<void> {
    await api.product.updateAsync(model.id, model.toUpdateRequestDto());
  }

  function handleUpsertingSucceededAsync(): void {
    setTimeout(() => navigate(model.detailRoutePath));
  }

  // Template.
  return (
    <ProductUpsertPage
      description="Chỉnh sửa một sản phẩm đang tồn tại, dùng cho các giao dịch về bán lẻ và liệu trình."
      isForCreating={false}
      model={model}
      onModelChanged={(changedData) => setModel(m => ({ ...m, ...changedData }))}
      upsertAction={handleUpsertAsync}
      onUpsertingSucceeded={handleUpsertingSucceededAsync}
    />
  );
}