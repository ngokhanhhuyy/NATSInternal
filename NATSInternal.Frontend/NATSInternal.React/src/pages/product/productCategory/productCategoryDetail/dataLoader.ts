import { api } from "@/api";
import { createProductCategoryDetailModel } from "@/models";

export async function loadDataAsync(id: number): Promise<ProductCategoryDetailModel> {
  const responseDto = await api.productCategory.getDetailAsync(id);
  return createProductCategoryDetailModel(responseDto);
}
