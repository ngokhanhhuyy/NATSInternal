import React from "react";

// Child components.
import OrderItem from "./SupplyItem";

// Props.
type ResultsPanelProps = {
  model: OrderListModel;
  hideIcon?: boolean;
};

// Components.
export default function OrderList(props: ResultsPanelProps): React.ReactNode {
  // Templates.
  return (
    <ul className="list-group list-group-flush">
      {props.model.items.length > 0 ? props.model.items.map((order, index) => (
        <OrderItem
          model={order}
          hideIcon={props.hideIcon}
          key={index}
        />
      )) : (
        <li className="list-group-item opacity-50 px-3 py-10">
          Không có kết quả
        </li>
      )}
    </ul>
  );
}
