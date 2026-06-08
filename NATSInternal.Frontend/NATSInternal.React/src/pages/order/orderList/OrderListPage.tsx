import React, { useState, useCallback, useEffect, useTransition } from "react";
import { useLoaderData, Link } from "react-router";
import { useRerendingTrigger } from "@/hooks";
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
  const [renderingKey, triggerRendering] = useRerendingTrigger(reload);
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
    triggerRendering();
  }, []);

  // Effect.
  useEffect(() => {
    reload();
  }, [
    model.sortByAscending,
    model.sortByFieldName,
    model.page,
    model.resultsPerPage,
    model.statsMonthYear,
    renderingKey]);

  // Template.
  return (
    <HasStatsListPage
      resourceName="order"
      model={model}
      onModelUpdated={handleModelUpdated}
      isReloading={isReloading}
      onPaginatorPageChanged={handlePaginatorPageChanged}
      onReloadingRequested={triggerRendering}
      canCreate={metadata.creatingAuthorization.canCreateOrder}
    >
      <OrderListResults
        model={model}
        renderItem={(order) => (
          <div className="flex flex-col items-end">
            <Link className="text-blue-700 dark:text-blue-400" to={order.customer.detailRoutePath}>
              {order.customer.fullName}
            </Link>

            <span className="opacity-50 text-sm">
              {order.customer.nickName}
            </span>
          </div>
        )}
      />
    </HasStatsListPage>
  );
}
