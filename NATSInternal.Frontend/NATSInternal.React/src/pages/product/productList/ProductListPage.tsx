import React from "react";
import { useLoaderData } from "react-router";
import { useApi } from "@/api";
import { createProductListModel } from "@/models";

// Child components.
import SearchablePageableListPage from "@/pages/shared/searchablePageableList/SearchablePageableListPage";

// Data loader.
export async function loadDataAsync(model?: ProductListModel): Promise<ProductListModel> {
  const api = useApi();
  const loadedModel = model ?? createProductListModel();
  const responseDto = await api.product.getListAsync(loadedModel.toRequestDto());
  loadedModel.mapFromResponseDto(responseDto);
  return loadedModel;
}

// Components.
export default function ProductListPage(): React.ReactNode {
  // Dependencies.
  const initialModel = useLoaderData<ProductListModel>();

  // Template.
  return (
    <SearchablePageableListPage
      description="Danh sách các khách hàng đã và đang giao dịch với cửa hàng."
      initialModel={initialModel}
      loadDataAsync={loadDataAsync}
    />
  );
}