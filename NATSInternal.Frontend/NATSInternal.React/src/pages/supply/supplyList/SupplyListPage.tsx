import React, { useState, useCallback, useEffect, useTransition } from "react";
import { useLoaderData } from "react-router";
import { useInitialRendering } from "@/hooks";
import { metadata } from "@/metadata";
import { loadDataAsync } from "./dataLoader";
import { joinClassName } from "@/helpers";

// Child components.
import HasStatsListPage from "@/pages/shared/list/listPage/hasStatsList";
import { ArchiveBoxArrowDownIcon } from "@heroicons/react/24/outline";

// Component.
export default function SupplyListPage(): React.ReactNode {
  // Dependencies.
  const initialModel = useLoaderData<SupplyListModel>();
  
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
  
  const handleModelUpdated = useCallback((updatedData: Partial<SupplyListModel>) => {
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
      sideBarPanels={
        <div className={joinClassName(
          "grid grid-cols-1 sm:grid-cols-2 md:grid-cols-2 lg:grid-cols-1 gap-3 items-start self-start",
          "sticky top-[calc(var(--topbar-height)+(--spacing(3)))]"
        )}>
        </div>
      }
      isReloading={isReloading}
      canCreate={metadata.creatingAuthorization.canCreateSupply}
    >
      <SupplyListResults model={model} />
    </HasStatsListPage>
  );
}