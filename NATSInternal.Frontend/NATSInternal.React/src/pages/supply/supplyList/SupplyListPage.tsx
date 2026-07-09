import React, { useState, useMemo, useCallback, useEffect, useTransition } from "react";
import { useLoaderData } from "react-router";
import { useInitialRendering } from "@/hooks";
import { metadata } from "@/metadata";
import { loadModelAsync } from "./dataLoader";
import { joinClassName } from "@/helpers";

// Child components.
import HasStatsListPage from "@/pages/shared/list/listPage/hasStatsList";
import SupplyListResults from "@/pages/shared/list/supplyListResults";
import { FormField, SelectInput, type SelectInputOption } from "@/components/form";
import { TagIcon } from "@heroicons/react/24/outline";

// Component.
export default function SupplyListPage(): React.ReactNode {
  // Dependencies.
  const initialModel = useLoaderData<SupplyListModel>();
  
  // States.
  const [model, setModel] = useState(() => initialModel);
  const [productMinimalModels, setProductMinimalModels] = useState<ProductMinimalModel[]>([]);
  const [isReloading, startTransition] = useTransition();
  const isInitialRendering = useInitialRendering();

  // Computed.
  const productOptions = useMemo<SelectInputOption[]>(() => {
    return [
      { value: "", displayName: "Tất cả sản phẩm" },
      ...productMinimalModels.map(p => ({ value: p.id.toString(), displayName: p.name }))
    ];
  }, [productMinimalModels]);
  
  // Callbacks.
  function reload(): void {
    startTransition(async () => {
      const reloadedModel = await loadModelAsync(model);
      setModel(reloadedModel);
      window.scrollTo({ top: 0, behavior: "smooth" });
    });
  }
  
  const handleModelUpdated = useCallback((updatedData: Partial<SupplyListModel>) => {
    setModel(m => ({ ...m, ...updatedData }));
  }, []);

  const handleProductChanged = useCallback((productIdAsString: string): void => {
    if (!productIdAsString) {
      setModel(m => ({ ...m, product: null }));
    }

    setModel(m => ({ ...m, product: productMinimalModels.find(p => p.id === parseInt(productIdAsString))! }));
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
      filterPanelChildren={
        <FormField path="type" displayName="Loại giao dịch" hideLabel>
          <div className="form-input-group">
            <span className="form-input-group-text border-e-0 shrink-0">
              <TagIcon className="size-4" />
            </span>
            
            <SelectInput
              className="min-w-fit"
              options={productOptions}
              value={model.product?.id.toString() ?? ""}
              onValueChanged={handleProductChanged}
            />
          </div>
        </FormField>
      }
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
