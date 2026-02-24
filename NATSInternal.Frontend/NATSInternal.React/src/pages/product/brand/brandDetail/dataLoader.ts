import { useApi } from "@/api";
import { createBrandDetailModel } from "@/models";

export async function loadDataAsync(id: string): Promise<BrandDetailModel> {
  const api = useApi();
  const responseDto = await api.brand.getDetailAsync(id);
  return createBrandDetailModel(responseDto);
}