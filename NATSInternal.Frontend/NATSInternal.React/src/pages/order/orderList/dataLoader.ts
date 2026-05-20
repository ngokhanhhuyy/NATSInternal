import { api } from "@/api";
import { createOrderListModel } from "@/models";

export async function loadDataAsync(model?: OrderListModel): Promise<OrderListModel> {
  model = model ?? createOrderListModel();
  const requestDto = model.toRequestDto();
  const responseDto = await api.order.getListAsync(requestDto);
  return model.mapFromResponseDto(responseDto);
}
