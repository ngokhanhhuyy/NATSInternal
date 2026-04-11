import React, { useState, useCallback, useTransition } from "react";
import { useLoaderData } from "react-router";
import { useApi } from "@/api";
import { createProductCategoryListModel } from "@/models";
import { useRerendingTrigger } from "@/hooks";

// Child components.
import ResultsPanel from "./ResultsPanel";
import ListPage from "@/pages/shared/searchablePageableList";

// Data loader.
export async function loadDataAsync(model?: ProductCategoryListModel): Promise<ProductCategoryListModel> {
  const api = useApi();
  let responseDto: ProductCategoryGetListResponseDto;
  if (!model) {
    responseDto = await api.productCategory.getListAsync(); 
    return createProductCategoryListModel(responseDto);
  }

  responseDto = await api.productCategory.getListAsync(model.toRequestDto());
  return model?.mapFromResponseDto(responseDto);
}

// Components.
export default function ProductListPage(): React.ReactNode {
  // Dependencies.
  const initialModel = useLoaderData<ProductCategoryListModel>();

  // States.
  const [model, setModel] = useState(() => initialModel);
  const [_, triggerRerender] = useRerendingTrigger(reload);
  const [isReloading, startTransition] = useTransition();

  // Callbacks.
  function reload(): void {
    startTransition(async () => {
      const reloadedModel = await loadDataAsync(model);
      setModel(reloadedModel);
      window.scrollTo({ top: 0, behavior: "smooth" });
    });
  }
  
  const handleModelUpdated = useCallback((updatedData: Partial<ProductCategoryListModel>) => {
    setModel(m => ({ ...m, ...updatedData }));
  }, []);

  const handlePaginatorPageChanged = useCallback((page: number) => {
    setModel(m => ({ ...m, page }));
    triggerRerender();
  }, []);

  // Template.
  return (
    <ListPage
      resourceName="productCategory"
      model={model}
      onModelUpdated={handleModelUpdated}
      isReloading={isReloading}
      onPaginatorPageChanged={handlePaginatorPageChanged}
      onFilterPanelReloadButtonClicked={triggerRerender}
    >
      <ResultsPanel model={model} isReloading={isReloading} />
    </ListPage>
  );
}