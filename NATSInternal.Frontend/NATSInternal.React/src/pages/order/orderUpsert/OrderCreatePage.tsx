import React, { useState } from "react";
import { useNavigate } from "react-router";
import { api } from "@/api";
import { createOrderUpsertModel } from "@/models/order/orderUpsertModel";
import { getOrderDetailRoutePath } from "@/helpers";

// Child components.
import OrderUpsertPage from "./OrderUpsertPage";
import OrderTypePanel from "./OrderTypePanel";
import CustomerPanel from "./CustomerPanel";

// Components.
export default function OrderCreatePage(): React.ReactNode {
  // Dependencies.
  const navigate = useNavigate();

  // States.
  const [model, setModel] = useState<OrderUpsertModel>(createOrderUpsertModel);

  // Callbacks.
  const handleUpsertAsync = async (): Promise<number> => {
    return await api.order.createAsync(model.toRequestDto());
  };

  const handleUpsertingSucceeded = (id: number) => {
    navigate(getOrderDetailRoutePath(id));
  };
  
  // Templates.
  return (
    <OrderUpsertPage
      model={model}
      onModelUpdated={(updatedData) => setModel(m => ({ ...m, ...updatedData }))}
      upsertAction={handleUpsertAsync}
      onUpsertingSucceeded={handleUpsertingSucceeded}
    >
      <OrderTypePanel
        model={model}
        onModelUpdated={(updatedData) => setModel(m => ({ ...m, ...updatedData }))}
      />
      
      <CustomerPanel
        model={model.customer}
        onModelUpdated={(updatedData) => setModel(m => ({ ...m, customer: { ...m.customer, ...updatedData } }))}
      />
    </OrderUpsertPage>
  );
}
