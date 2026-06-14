import React, { useState, useCallback, useEffect, useTransition } from "react";
import { useLoaderData } from "react-router";
import { useInitialRendering } from "@/hooks";
import { metadata } from "@/metadata";
import { loadDataAsync } from "./dataLoader";

// Child components.
import HasStatsListPage from "@/pages/shared/list/listPage/hasStatsList";
import OrderListResults from "@/pages/shared/list/orderListResults";

// Component.
export default function OrderListPage(): React.ReactNode {
  // Dependencies.
  const initialModel = useLoaderData<OrderListModel>();
  
  // States.
  const [model, setModel] = useState(() => initialModel);
  const [isReloading, startTransition] = useTransition();
  const isInitialRendering = useInitialRendering();
  
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

  // Effect.
  useEffect(() => {
    if (isInitialRendering) {
      return;
    }
    
    reload();
  }, [model.sortByAscending, model.sortByFieldName, model.page, model.resultsPerPage, model.statsMonthYear]);

  // Template.
  return (
    <HasStatsListPage
      resourceName="order"
      model={model}
      onModelUpdated={handleModelUpdated}
      isReloading={isReloading}
      canCreate={metadata.creatingAuthorization.canCreateOrder}
    >
      <OrderListResults model={model} />
    </HasStatsListPage>
  );
}
