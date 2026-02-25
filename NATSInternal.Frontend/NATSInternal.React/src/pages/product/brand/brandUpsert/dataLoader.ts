import { useApi } from "@/api";
import { createBrandUpsertModel } from "@/models";


export async function loadUpdateDataAsync(id: string): Promise<BrandUpsertModel> {
  const api = useApi();
  const responseDto = await api.brand.getDetailAsync(id);
  return createBrandUpsertModel(responseDto);
}