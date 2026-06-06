import React from "react";
import { Link } from "react-router";
import { joinClassName } from "@/helpers";

// Child components.
import OrderListResults from "@/pages/shared/list/orderListResults";

// Props.
type ResultsPanelProps = {
  model: OrderListModel;
  isReloading: boolean;
};

// Components.
export default function ResultsPanel(props: ResultsPanelProps): React.ReactNode {
  // Templates.
  return (
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">
          Danh sách kết quả
        </span>
      </div>

      <div className={joinClassName("panel-body transition-opacity", props.isReloading && "opacity-50")}>
        <OrderListResults
          model={props.model}
          renderItem={(order) => (
            <div className="flex flex-col items-end">
              <Link className="text-blue-700 dark:text-blue-400" to={order.customer.detailRoutePath}>
                {order.customer.fullName}
              </Link>

              <span className="opacity-50 text-sm">
                {order.customer.nickName}
              </span>
            </div>
          )}
        />
      </div>
    </div>
  );
}
