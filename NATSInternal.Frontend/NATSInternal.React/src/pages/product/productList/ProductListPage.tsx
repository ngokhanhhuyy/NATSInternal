import React from "react";
import { useLoaderData, Link } from "react-router";
import { useApi } from "@/api";
import { createProductListModel } from "@/models";

// Child components.
import SearchablePageableListPage from "@/pages/shared/searchablePageableList/SearchablePageableListPage";
import { BrandListPanel, ProductCategoryListPanel } from "./SecondaryPanels";
import { ExclamationTriangleIcon } from "@heroicons/react/24/outline";

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
  return (
    <SearchablePageableListPage<ProductListModel, ProductListProductModel>
      description="Danh sách các sản phẩm trong kho, kể cả các sản phẩm đã ngừng kinh doanh."
      initialModel={initialModel}
      loadDataAsync={loadDataAsync}
      renderTableHeaderRowChildren={() => (
        <>
          <th>Tên sản phẩm</th>
          <th>Giá niêm yết</th>
          <th>Phân loại</th>
          <th>Còn lại trong kho</th>
          <th>Lưu ý</th>
        </>
      )}
      renderTableBodyRowChildren={(itemModel) => (
        <>
          <td>
            <Link to={itemModel.detailRoute} className="text-blue-700 dark:text-blue-400 font-bold">
              {itemModel.name}
            </Link>
          </td>
          <td>
            <div className="flex justify-between">
              <span>{itemModel.formattedDefaultAmountBeforeVatPerUnit}</span>
              {itemModel.defaultVatPercentagePerUnit > 0 && (
                <span className="opacity-50">({itemModel.defaultVatPercentagePerUnit}% VAT)</span>
              )}
            </div>
          </td>
          <td>{itemModel.category?.name}</td>
          <td>{itemModel.stockingQuantity} {itemModel.unit}</td>
          <td className="text-yellow-600 dark:text-yellow-400 align-middle">
            {itemModel.isResupplyNeeded && (
              <div className="flex items-center gap-1.5">
                <ExclamationTriangleIcon className="size-4.5" />
                <span>Cần nhập hàng</span>
              </div>
            )}
          </td>
        </>
      )}
    >
      <div className="grid grid-cols-1 sm:grid-cols-2 gap-3 items-start">
        <BrandListPanel />
        <ProductCategoryListPanel />
      </div>
    </SearchablePageableListPage>
  );
}