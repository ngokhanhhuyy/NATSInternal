import React, { useState, useCallback, useTransition } from "react";
import { useLoaderData } from "react-router";
import { useApi } from "@/api";
import { createCustomerListModel } from "@/models";
import { useRerendingTrigger } from "@/hooks";

// Child components.
import ResultsPanel from "./ResultsPanel";
import ListPage from "@/pages/shared/searchablePageableList";

// Loader
export async function loadDataAsync(model?: CustomerListModel): Promise<CustomerListModel> {
  const api = useApi();
  if (model) {
    const responseDto = await api.customer.getListAsync(model.toRequestDto());
    return model.mapFromResponseDto(responseDto);
  }

  const responseDto = await api.customer.getListAsync();
  return createCustomerListModel(responseDto);
}

// Components.
export default function CustomerListPage(): React.ReactNode {
  // Dependencies.
  const initialModel = useLoaderData<CustomerListModel>();

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

  const handleModelUpdated = useCallback((updatedData: Partial<CustomerListModel>) => {
    setModel(m => ({ ...m, ...updatedData }));
  }, []);

  const handlePaginatorPageChanged = useCallback((page: number) => {
    setModel(m => ({ ...m, page }));
    triggerRerender();
  }, []);

  // Template.
  return (
    <ListPage
      resourceName="customer"
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