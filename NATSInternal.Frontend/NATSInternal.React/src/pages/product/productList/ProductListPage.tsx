import React from "react";
import { useLoaderData, Link } from "react-router";
import { useApi } from "@/api";
import { createProductListModel } from "@/models";

// Child components.
import SearchablePageableListPage from "@/pages/shared/searchablePageableList/SearchablePageableListPage";
// import { BrandListPanel, ProductCategoryListPanel } from "./SecondaryPanels";
import { ExclamationTriangleIcon, CheckCircleIcon } from "@heroicons/react/24/outline";

// Api.
const api = useApi();

// Data loader.
export async function loadDataAsync(model?: ProductListModel): Promise<ProductListModel> {
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
    <SearchablePageableListPage<ProductListModel, ProductListProductModel>
      description="Danh sách các sản phẩm trong kho, kể cả các sản phẩm đã ngừng kinh doanh."
      initialModel={initialModel}
      loadDataAsync={loadDataAsync}
      renderTableHeaderRowChildren={() => (
        <>
          <th className="hidden md:table-cell">Tên sản phẩm</th>
          <th className="hidden md:table-cell">Giá niêm yết</th>
          <th className="hidden md:table-cell">Phân loại</th>
          <th className="hidden md:table-cell">Còn lại trong kho</th>
        </>
      )}
      renderTableBodyRowChildren={(itemModel) => (
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
    >
      {/* <div className="grid grid-cols-1 sm:grid-cols-2 gap-3 items-start">
        <BrandListPanel />
        <ProductCategoryListPanel />
      </div> */}
    </SearchablePageableListPage>
  );
}