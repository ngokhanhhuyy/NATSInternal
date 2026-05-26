import React, { useState } from "react";
import { useLoaderData, useNavigate } from "react-router";
import { api } from "@/api";
import { createOrderUpsertModel } from "@/models/order/orderUpsertModel";
import { getOrderDetailRoutePath } from "@/helpers";

// Child components.
import OrderUpsertPage from "./OrderUpsertPage";

// Data loader.
export async function loadDataAsync(id: number): Promise<[OrderUpsertModel, number]> {
  const responseDto = await api.order.getDetailAsync(id);
  const model = createOrderUpsertModel(responseDto);
  return [model, id];
}

// Components.
export default function OrderUpdatePage(): React.ReactNode {
  // Dependencies.
  const navigate = useNavigate();
  const [initialModel, id] = useLoaderData<[OrderUpsertModel, number]>();

  // States.
  const [model, setModel] = useState<OrderUpsertModel>(initialModel);

  // Callbacks.
  const handleUpsertAsync = async (): Promise<void> => {
    await api.order.updateAsync(id, model.toRequestDto());
  };

  const handleUpsertingSucceeded = () => {
    navigate(getOrderDetailRoutePath(id));
  };
  
  // Templates.
  return (
    <OrderUpsertPage
      model={model}
      onModelUpdated={(updatedData) => setModel(m => ({ ...m, ...updatedData }))}
      upsertAction={handleUpsertAsync}
      onUpsertingSucceeded={handleUpsertingSucceeded}
    />
  );
}
