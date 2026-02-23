import { useApi } from "@/api";
import { createProductListModel, createBrandBasicModel, createProductCategoryBasicModel } from "@/models";

// Data loader.
export type ProductListDataLoaderResults = {
  productList: ProductListModel;
  brandOptions: BrandBasicModel[];
  categoryOptions: ProductCategoryBasicModel[];
};

export async function loadDataAsync(): Promise<ProductListDataLoaderResults> {
  const api = useApi();
  
  const [productList, brandOptions, categoryOptions] = await Promise.all([
    loadProductListAsync(),
    api.brand.getAllAsync().then(dtos => dtos.map(dto => createBrandBasicModel(dto))),
    api.productCategory.getAllAsync().then(dtos => dtos.map(dto => createProductCategoryBasicModel(dto)))
  ]);

  return { productList, brandOptions, categoryOptions };
}

export async function loadProductListAsync(model?: ProductListModel): Promise<ProductListModel> {
  const api = useApi();
  if (model) {
    const responseDto = await api.product.getListAsync(model.toRequestDto());
    return model.mapFromResponseDto(responseDto);
  }

  model = createProductListModel();
  const responseDto = await api.product.getListAsync(model.toRequestDto());
  return model.mapFromResponseDto(responseDto);
}