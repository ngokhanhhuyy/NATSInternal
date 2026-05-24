import { api } from "@/api";
import { createOrderDetailModel } from "@/models/order/orderDetailModel";

export async function loadDataAsync(id: number): Promise<OrderDetailModel> {
  const responseDto = await api.order.getDetailAsync(id);
  return createOrderDetailModel(responseDto);
}
