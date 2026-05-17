import { api } from "@/api";
import { createProductListModel, createProductCategoryBasicModel } from "@/models";

// Data loader.
export type ProductListDataLoaderResults = {
  productList: ProductListModel;
  categoryOptions: ProductCategoryBasicModel[];
};

export async function loadDataAsync(): Promise<ProductListDataLoaderResults> {
  const [productList, categoryOptions] = await Promise.all([
    loadProductListAsync(),
    api.productCategory.getAllAsync().then(dtos => dtos.map(dto => createProductCategoryBasicModel(dto)))
  ]);

  return { productList, categoryOptions };
}

export async function loadProductListAsync(model?: ProductListModel): Promise<ProductListModel> {
  if (model) {
    const responseDto = await api.product.getListAsync(model.toRequestDto());
    return model.mapFromResponseDto(responseDto);
  }

  model = createProductListModel();
  const responseDto = await api.product.getListAsync(model.toRequestDto());
  return model.mapFromResponseDto(responseDto);
}
