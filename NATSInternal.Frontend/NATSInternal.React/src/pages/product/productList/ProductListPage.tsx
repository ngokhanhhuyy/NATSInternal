import React, { useState, useCallback, useEffect, useTransition } from "react";
import { useLoaderData, Link } from "react-router";
import { useInitialRendering, useRequestHandlerQueue } from "@/hooks";
import { metadata } from "@/metadata";
import { getProductCategoryListRoutePath, joinClassName } from "@/helpers";

// Child components.
import { loadProductListAsync, type ProductListDataLoaderResults } from "./dataLoader";
import ProductListResults from "@/pages/shared/list/productListResults";
import FilterPanelChildren from "./FilterPanelChildren";
import ListPage from "@/pages/shared/list/listPage/searchableList";
import TopByCriterionPanel from "./TopByCriterionPanel";
import { TagIcon } from "@heroicons/react/24/outline";

// Components.
export default function ProductListPage(): React.ReactNode {
  // Dependencies.
  const initialModel = useLoaderData<ProductListDataLoaderResults>();

  // States.
  const [model, setModel] = useState(() => initialModel.model);
  const [isReloading, startTransition] = useTransition();
  const isInitialRendering = useInitialRendering();
  const reloadAsync = useRequestHandlerQueue(async () => await loadProductListAsync(model), (reloadedModel) => {
    setModel(reloadedModel);
    window.scrollTo({ top: 0, behavior: "smooth" });
  });

  // Callbacks.
  const handleModelUpdated = useCallback((updatedData: Partial<ProductListModel>) => {
    setModel(m => ({ ...m, ...updatedData }));
  }, []);

  // Effect.
  useEffect(() => {
    if (isInitialRendering) {
      return;
    }
    
    startTransition(reloadAsync);
  }, [
    model.sortByAscending,
    model.sortByFieldName,
    model.searchContent,
    model.page,
    model.resultsPerPage,
    model.category
  ]);

  // Template.
  return (
    <ListPage
      resourceName="product"
      model={model}
      onModelUpdated={handleModelUpdated}
      isReloading={isReloading}
      linkButtons={
        <div className="flex justify-end">
          <Link className="btn" to={getProductCategoryListRoutePath()}>
            <TagIcon className="size-4" />
            <span>Danh sách phân loại</span>
          </Link>
        </div>
      }
      filterPanelChildren={
        <FilterPanelChildren
          model={model}
          onModelUpdated={(updatedData) => setModel(m => ({ ...m, ...updatedData }))}
        />
      }
      sideBarPanels={
        <div className={joinClassName(
          "grid grid-cols-1 md:grid-cols-2 lg:grid-cols-1 gap-3 h-fit",
          "sticky top-[calc(var(--topbar-height)+(--spacing(3)))]"
        )}>
          <TopByCriterionPanel
            getTopAsync={async (api, requestDto) => api.product.getTopBySoldQuantity(requestDto)}
            criterion="SoldQuantity"
          />
          
          <TopByCriterionPanel
            getTopAsync={async (api, requestDto) => api.product.getTopByRevenue(requestDto)}
            criterion="Revenue"
          />
        </div>
      }
      canCreate={metadata.creatingAuthorization.canCreateProduct}
    >
      <ProductListResults className="list-group-flush" model={model} />
    </ListPage>
  );
}
