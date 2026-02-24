import React, { useState, useCallback, useTransition } from "react";
import { useLoaderData } from "react-router";
import { useRerendingTrigger } from "@/hooks";

// Child components.
import { loadProductListAsync, type ProductListDataLoaderResults } from "./dataLoader";
import ResultsPanel from "./ResultsPanel";
import FilterPanelChildren from "./FilterPanelChildren";
import ListPage from "@/pages/shared/searchablePageableList";
import { BrandListPanel, ProductCategoryListPanel } from "./SecondaryPanels";

// Components.
export default function ProductListPage(): React.ReactNode {
  // Dependencies.
  const initialModel = useLoaderData<ProductListDataLoaderResults>();

  // States.
  const [model, setModel] = useState(() => initialModel.productList);
  const [_, triggerRerender] = useRerendingTrigger(reload);
  const [isReloading, startTransition] = useTransition();

  // Computed.
  const compareModelAdditionally = useCallback((originalModel: ProductListModel, currentModel: ProductListModel) => {
    return (
      originalModel.brand?.id === currentModel.brand?.id &&
      originalModel.category?.name === currentModel.category?.name
    );
  }, []);

  // Callbacks.
  function reload(): void {
    startTransition(async () => {
      const reloadedModel = await loadProductListAsync(model);
      setModel(reloadedModel);
      window.scrollTo({ top: 0, behavior: "smooth" });
    });
  }
  
  const handleModelUpdated = useCallback((updatedData: Partial<ProductListModel>) => {
    setModel(m => ({ ...m, ...updatedData }));
  }, []);

  const handlePaginatorPageChanged = useCallback((page: number) => {
    setModel(m => ({ ...m, page }));
    triggerRerender();
  }, []);

  // Template.
  return (
    <ListPage
      resourceName="product"
      model={model}
      onModelUpdated={handleModelUpdated}
      isReloading={isReloading}
      onPaginatorPageChanged={handlePaginatorPageChanged}
      onFilterPanelReloadButtonClicked={triggerRerender}
      filterPanelChildren={
        <FilterPanelChildren
          model={model}
          onModelUpdated={(updatedData) => setModel(m => ({ ...m, ...updatedData }))}
        />
      }
      additionalPanels={
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-3 mt-5">
          <BrandListPanel />
          <ProductCategoryListPanel />
        </div>
      }
      additionalDirtyModelComparer={compareModelAdditionally}
    >
      <ResultsPanel model={model} isReloading={isReloading} />
    </ListPage>
  );
}