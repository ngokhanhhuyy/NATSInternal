import React from "react";
import { useLoaderData, Link } from "react-router";
import { useApi } from "@/api";
import { createProductListModel, createBrandBasicModel, createProductCategoryBasicModel } from "@/models";

// Child components.
import SearchablePageableListPage from "@/pages/shared/searchablePageableList/SearchablePageableListPage";
import { BrandListBlock, ProductCategoryListBlock } from "./BrandListBlock";

// Api.
const api = useApi();

// Types.
type InitialModels = {
  productList: ProductListModel;
  brands: BrandBasicModel[];
  categories: ProductCategoryBasicModel[];
};

// Data loader.
export async function loadDataAsync(): Promise<InitialModels> {
  const [productList, brands, categories] = await Promise.all([
    loadProductListAsync(),
    api.brand.getAllAsync().then(responseDtos => responseDtos.map(createBrandBasicModel)),
    api.productCategory.getAllAsync().then(responseDtos => responseDtos.map(createProductCategoryBasicModel))
  ]);

  return { productList, brands, categories };
}

async function loadProductListAsync(model?: ProductListModel): Promise<ProductListModel> {
  if (model) {
    const responseDto = await api.product.getListAsync(model.toRequestDto());
    return model.mapFromResponseDto(responseDto);
  }

  const responseDto = await api.product.getListAsync();
  return createProductListModel(responseDto);
}

// Components.
export default function ProductListPage(): React.ReactNode {
  // Dependencies.
  const initialModels = useLoaderData<InitialModels>();

  // Template.
  return (
    <SearchablePageableListPage<ProductListModel, ProductListProductModel>
      description="Danh sách các sản phẩm trong kho, kể cả các sản phẩm đã ngừng kinh doanh."
      initialModel={initialModels.productList}
      loadDataAsync={loadProductListAsync}
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
            <Link to={itemModel.detailRoute} className="font-bold">
              {itemModel.name}
            </Link>
          </td>
          <td>{itemModel.formattedDefaultAmountBeforeVatPerUnit}</td>
          <td>{itemModel.category?.name}</td>
          <td>{itemModel.stockingQuantity} {itemModel.unit}</td>
          <td className="text-red-700 dark:text-red-400 align-middle">
            {itemModel.isResupplyNeeded && <span>Cần nhập hàng</span>}
          </td>
        </>
      )}
    >
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-3 items-start">
        <BrandListBlock model={initialModels.brands} />
        <ProductCategoryListBlock model={initialModels.categories} />
      </div>
    </SearchablePageableListPage>
  );
}