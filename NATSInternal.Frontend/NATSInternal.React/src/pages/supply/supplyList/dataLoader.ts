import { api } from "@/api";
import { createSupplyListModel } from "@/models";

export async function loadDataAsync(model?: SupplyListModel): Promise<SupplyListModel> {
  model = model ?? createSupplyListModel();
  const requestDto = model.toRequestDto();
  const responseDto = await api.supply.getListAsync(requestDto);
  return model.mapFromResponseDto(responseDto);
}
