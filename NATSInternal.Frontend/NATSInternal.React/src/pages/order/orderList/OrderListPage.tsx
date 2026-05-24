import React, { useState, useCallback, useTransition } from "react";
import { useLoaderData } from "react-router";
import { useRerendingTrigger } from "@/hooks";
import { loadDataAsync } from "./dataLoader";

// Child components.
import ListPage from "@/pages/shared/list/hasStatsList";
import ResultsPanel from "./ResultsPanel";

// Component.
export default function OrderListPage(): React.ReactNode {
  // Dependencies.
  const initialModel = useLoaderData<OrderListModel>();
  
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
  
  const handleModelUpdated = useCallback((updatedData: Partial<OrderListModel>) => {
    setModel(m => ({ ...m, ...updatedData }));
  }, []);

  const handlePaginatorPageChanged = useCallback((page: number) => {
    setModel(m => ({ ...m, page }));
    triggerRerender();
  }, []);

  // Template.
  return (
    <ListPage
      resourceName="order"
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
