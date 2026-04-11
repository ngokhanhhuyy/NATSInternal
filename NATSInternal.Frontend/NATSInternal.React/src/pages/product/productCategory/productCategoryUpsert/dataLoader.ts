import { useApi } from "@/api";
import { createProductCategoryUpsertModel } from "@/models";

export async function loadUpdateDataAsync(id: string): Promise<ProductCategoryUpsertModel> {
  const api = useApi();
  const responseDto = await api.productCategory.getDetailAsync(id);
  return createProductCategoryUpsertModel(responseDto);
}