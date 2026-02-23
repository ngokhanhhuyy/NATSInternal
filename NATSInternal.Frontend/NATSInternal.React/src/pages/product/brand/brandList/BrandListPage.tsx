import React, { useState, useCallback, useTransition } from "react";
import { useLoaderData } from "react-router";
import { useApi } from "@/api";
import { createBrandListModel } from "@/models";
import { useRerendingTrigger } from "@/hooks";

// Child components.
import ResultsPanel from "./ResultsPanel";
import ListPage from "@/pages/shared/searchablePageableList";

// Data loader.
export async function loadDataAsync(model?: BrandListModel): Promise<BrandListModel> {
  const api = useApi();
  let responseDto: BrandGetListResponseDto;
  if (!model) {
    responseDto = await api.brand.getListAsync(); 
    return createBrandListModel(responseDto);
  }

  responseDto = await api.brand.getListAsync(model.toRequestDto());
  return model?.mapFromResponseDto(responseDto);
}

// Components.
export default function ProductListPage(): React.ReactNode {
  // Dependencies.
  const initialModel = useLoaderData<BrandListModel>();

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
  
  const handleModelUpdated = useCallback((updatedData: Partial<BrandListModel>) => {
    setModel(m => ({ ...m, ...updatedData }));
  }, []);

  const handlePaginatorPageChanged = useCallback((page: number) => {
    setModel(m => ({ ...m, page }));
    triggerRerender();
  }, []);

  // Template.
  return (
    <ListPage
      resourceName="brand"
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