import { api } from "@/api";
import { createProductCategoryUpsertModel } from "@/models";

export async function loadUpdateDataAsync(id: number): Promise<ProductCategoryUpsertModel> {
  const responseDto = await api.productCategory.getDetailAsync(id);
  return createProductCategoryUpsertModel(responseDto);
}
