import React, { useState, useCallback, useEffect, useTransition } from "react";
import { useLoaderData, Link } from "react-router";
import { useRerendingTrigger } from "@/hooks";
import { metadata } from "@/metadata";
import { getProductCategoryListRoutePath } from "@/helpers";
import { TagIcon } from "@heroicons/react/24/outline";

// Child components.
import { loadProductListAsync, type ProductListDataLoaderResults } from "./dataLoader";
import ProductListResults from "@/pages/shared/list/productListResults";
import FilterPanelChildren from "./FilterPanelChildren";
import ListPage from "@/pages/shared/list/listPage/searchableList";

// Components.
export default function ProductListPage(): React.ReactNode {
  // Dependencies.
  const initialModel = useLoaderData<ProductListDataLoaderResults>();

  // States.
  const [model, setModel] = useState(() => initialModel.model);
  const [renderingKey, triggerRerender] = useRerendingTrigger(reload);
  const [isReloading, startTransition] = useTransition();

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

  // Effect.
  useEffect(() => {
    reload();
  }, [model.sortByAscending, model.sortByFieldName, model.page, model.resultsPerPage, renderingKey]);

  // Template.
  return (
    <ListPage
      resourceName="product"
      model={model}
      onModelUpdated={handleModelUpdated}
      isReloading={isReloading}
      onPaginatorPageChanged={handlePaginatorPageChanged}
      onReloadingRequested={triggerRerender}
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
      canCreate={metadata.creatingAuthorization.canCreateProduct}
    >
      <ProductListResults className="list-group-flush" model={model} />
    </ListPage>
  );
}
