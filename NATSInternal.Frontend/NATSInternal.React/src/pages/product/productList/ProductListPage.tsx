import React, { useState, useEffect, useTransition } from "react";
import { useLoaderData, Link } from "react-router";
import { useApi } from "@/api";
import { createProductListModel } from "@/models";

// Child components.
import DisplayOptionsPanel from "@/pages/shared/searchablePageableList/DisplayOptionsPanel";
import FilterOptionsPanel from "@/pages/shared/searchablePageableList/FilterOptionsPanel";
import ResultsTablePanel from "@/pages/shared/searchablePageableList/ResultsTablePanel";
import { MainContainer } from "@/components/layouts";
import { Paginator } from "@/components/ui";
// import { BrandListPanel, ProductCategoryListPanel } from "./SecondaryPanels";
import { ExclamationTriangleIcon, CheckCircleIcon, PlusIcon } from "@heroicons/react/24/outline";

// Data loader.
export async function loadDataAsync(model?: ProductListModel): Promise<ProductListModel> {
  const api = useApi();
  if (model) {
    const responseDto = await api.product.getListAsync(model.toRequestDto());
    return model.mapFromResponseDto(responseDto);
  }

  model = createProductListModel();
  const responseDto = await api.product.getListAsync(model.toRequestDto());
  return model.mapFromResponseDto(responseDto);
}

// Components.
export default function ProductListPage(): React.ReactNode {
  // Dependencies.
  const initialModel = useLoaderData<ProductListModel>();

  // States.
  const [model, setModel] = useState(() => initialModel);
  const [isInitialRendering, setIsInitialRendering] = useState(() => true);
  const [isReloading, startTransition] = useTransition();

  // Callbacks.
  async function reloadAsync(): Promise<void> {
    const reloadedModel = await loadDataAsync(model);
    setModel(reloadedModel);
    window.scrollTo({ top: 0, behavior: "smooth" });
  }
  
  // Effect.
  useEffect(() => {
    if (isInitialRendering) {
      setIsInitialRendering(false);
      return;
    }

    startTransition(reloadAsync);
  }, [model.searchContent, model.sortByAscending, model.sortByFieldName, model.page, model.resultsPerPage]);

  // Template.
  const renderAmountAndVatPercentage = (itemModel: ProductListProductModel): React.ReactNode => {
    return (
      <>
        <span className="opacity-50 inline md:hidden">Giá:</span>
        <span>{itemModel.formattedDefaultAmountBeforeVatPerUnit}</span>
        {itemModel.defaultVatPercentagePerUnit > 0 && (
          <span className="opacity-50">({itemModel.defaultVatPercentagePerUnit}% VAT)</span>
        )}
      </>
    );
  };

  const renderStockingQuantity = (itemModel: ProductListProductModel): React.ReactNode => {
    return (
      <>
        <span className="inline md:hidden opacity-50">Còn lại:</span>
        <span>{itemModel.stockingQuantity} {itemModel.unit}</span>
      </>
    );
  };

  return (
    <MainContainer
      description="Danh sách các sản phẩm trong kho, kể cả các sản phẩm đã ngừng kinh doanh."
      className="gap-3"
      isLoading={isReloading}
    >
      <div className="flex flex-col items-stretch gap-3">
        <ResultsTablePanel
          model={model}
          renderHeaderRowChildren={() => (
            <>
              <th className="hidden md:table-cell">Tên sản phẩm</th>
              <th className="hidden md:table-cell">Giá niêm yết</th>
              <th className="hidden md:table-cell">Phân loại</th>
              <th className="hidden md:table-cell">Còn lại trong kho</th>
            </>
          )}
          renderBodyRowChildren={(itemModel: ProductListProductModel) => (
            <>
              <td>
                <div className="flex gap-2 items-center">
                  {itemModel.isResupplyNeeded ? (
                    <ExclamationTriangleIcon className="text-yellow-600 dark:text-yellow-400 size-4.5" />
                  ) : (
                    <CheckCircleIcon className="text-emerald-600 dark:text-emerald-400 size-4.5" />
                  )}
                  
                  <Link to={itemModel.detailRoute} className="text-blue-700 dark:text-blue-400 font-bold">
                    {itemModel.name}
                  </Link>
                </div>

                <div className="block md:hidden text-sm ps-7">
                  <div className="flex justify-start gap-3">
                    {renderAmountAndVatPercentage(itemModel)}
                  </div>

                  <div className="flex gap-3">
                    {renderStockingQuantity(itemModel)}
                  </div>
                  
                  {itemModel.category && (
                    <div className="flex gap-3">
                      <span className="opacity-50">Phân loại: </span>
                      <span>{itemModel.category.name}</span>
                    </div>
                  )}
                </div>
              </td>
              <td className="hidden md:table-cell">
                <div className="flex justify-between gap-5">
                  {renderAmountAndVatPercentage(itemModel)}
                </div>
              </td>
              <td className="hidden md:table-cell">{itemModel.category?.name}</td>
              <td className="hidden md:table-cell">{renderStockingQuantity(itemModel)}</td>
            </>
          )}
        />

        <div className="flex justify-end gap-3 mb-3 md:mb-5">
          <Paginator
            page={model.page}
            pageCount={model.pageCount}
            onPageChanged={(page) => setModel(m => ({ ...m, page }))}
            isReloading={isReloading}
            getPageButtonClassName={(_, isActive) => isActive ? "btn-primary" : undefined}
          />

          <div className="border-r border-black/25 dark:border-white/25 w-px" />

          <Link className="btn gap-1 shrink-0" to={model.createRoutePath}>
            <PlusIcon className="size-4.5" />
            <span>Tạo sản phẩm mới</span>
          </Link>
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-2 gap-3">
          <DisplayOptionsPanel
            model={model}
            onModelChanged={changedData => setModel(m => ({ ...m, ...changedData }))}
            isReloading={false}
          />

          <FilterOptionsPanel
            model={model}
            onModelChanged={changedData => setModel(m => ({ ...m, ...changedData }))}
            onSearchButtonClicked={reloadAsync}
            isReloading={false}
          />
        </div>
      </div>
    </MainContainer>
  );
}