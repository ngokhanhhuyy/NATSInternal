import React, { useState, useMemo, useCallback, useEffect, useTransition } from "react";
import { useLoaderData } from "react-router";
import { useInitialRendering } from "@/hooks";
import { metadata, getDisplayName } from "@/metadata";
import { loadDataAsync } from "./dataLoader";

// Child components.
import HasStatsListPage from "@/pages/shared/list/listPage/hasStatsList";
import OrderListResults from "@/pages/shared/list/orderListResults";
import { FormField, SelectInput, type SelectInputOption } from "@/components/form";
import { TagIcon } from "@heroicons/react/24/outline";

// Component.
export default function OrderListPage(): React.ReactNode {
  // Dependencies.
  const initialModel = useLoaderData<OrderListModel>();
  
  // States.
  const [model, setModel] = useState(() => initialModel);
  const [isReloading, startTransition] = useTransition();
  const isInitialRendering = useInitialRendering();

  // Computed.
  const orderTypeOptions = useMemo<SelectInputOption[]>(() => {
    const options = [{ value: "", displayName: "Tất cả" }];
    for (const orderType of ["Retail", "Treatment", "Consultant"] satisfies OrderType[]) {
      options.push({ value: orderType, displayName: getDisplayName(orderType) ?? orderType });
    }

    return options;
  }, []);
  
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
  }, [
    model.sortByAscending,
    model.sortByFieldName,
    model.page,
    model.resultsPerPage,
    model.statsMonthYear,
    model.type]);

  // Template.
  return (
    <HasStatsListPage
      resourceName="order"
      model={model}
      onModelUpdated={handleModelUpdated}
      filterPanelChildren={
        <FormField path="type" displayName="Loại giao dịch" hideLabel>
          <div className="form-input-group">
            <span className="form-input-group-text border-e-0 shrink-0">
              <TagIcon className="size-4" />
            </span>
            
            <SelectInput
              className="min-w-fit"
              options={orderTypeOptions}
              value={model.type ?? ""}
              onValueChanged={(type) => setModel(m => ({ ...m, type: type as OrderType || null }))}
            />
          </div>
        </FormField>
      }
      sideBarPanels={<></>}
      isReloading={isReloading}
      canCreate={metadata.creatingAuthorization.canCreateOrder}
    >
      <OrderListResults model={model} />
    </HasStatsListPage>
  );
}
