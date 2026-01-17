import React, { useState } from "react";
import { useNavigate } from "react-router";
import { useApi } from "@/api";
import { createProductUpsertModel } from "@/models";

// Child components.
import ProductUpsertPage from "./ProductUpsertPage";

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
    setModel(m => ({ ...m, id }));
  }

  function handleUpsertingSucceededAsync(): void {
    setTimeout(() => navigate(model.detailRoutePath));
  }

  // Template.
  return (
    <ProductUpsertPage
      description="Tạo một sản phẩm mới, dùng cho các giao dịch về bán lẻ và liệu trình."
      isForCreating={true}
      model={model}
      onModelChanged={(changedData) => setModel(m => ({ ...m, ...changedData }))}
      upsertAction={handleUpsertAsync}
      onUpsertingSucceeded={handleUpsertingSucceededAsync}
    />
  );
}