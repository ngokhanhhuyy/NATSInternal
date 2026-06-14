import React from "react";
import { getDisplayName } from "@/metadata";
import { compute, joinClassName, getTextClassNameBasedOnDebtAmount } from "@/helpers";

// Child components.
import Item from "./Item";

// Props.
type ItemListPanelProps = {
  model: OrderDetailModel;
};

// Components.
export default function ItemListPanel(props: ItemListPanelProps): React.ReactNode {
  // Computed.
  const title = compute<string>(() => {
    switch (props.model.type) {
      case "Consultant":
        return "Danh sách dịch vụ";
      case "Retail":
        return "Danh sách sản phẩm";
      case "Treatment":
        return "Danh sách sản phẩm và dịch vụ";
    }
  });

  // Template.
  return (
    <div className="panel flex-1">
      <div className="panel-header">  
        <span className="panel-header-title">
          {title}
        </span>
      </div>

      <div className="panel-body flex flex-col justify-between gap-3 w-full overflow-x-auto p-3">
        <ul className="list-group panel-body-area">
          {props.model.productItems.map((productItem, index) => (
            <Item
              model={productItem}
              index={index}
              key={productItem.id}
            />
          ))}
          
          {props.model.serviceItems.map((serviceItem, index) => (
            <Item
              model={serviceItem}
              index={index + props.model.productItems.length}
              key={serviceItem.id}
            />
          ))}
        </ul>

        <ul className="list-group panel-body-area">
          <li className="list-group-item grid grid-cols-[auto_auto_1fr] gap-5 px-2 py-1">
            <span className="opacity-50 w-40">Tổng giá tiền</span>
            <span className="text-blue-700 dark:text-blue-400 font-bold">{props.model.displayAmountAfterVat}</span>
          </li>

          <li className="list-group-item grid grid-cols-[auto_auto_1fr] gap-5 px-2 py-1">
            <span className="opacity-50 w-40">{getDisplayName("paidAmount")}</span>
            <span className="text-blue-700 dark:text-blue-400 font-bold">{props.model.displayPaidAmount}</span>
          </li>

          <li className="list-group-item grid grid-cols-[auto_auto_1fr] gap-5 px-2 py-1">
            <span className="opacity-50 w-40">
              {props.model.debtAmount >= 0 ? "Nợ" : "Cần hoàn tiền"}
            </span>
            <span className={joinClassName(
              "font-bold",
              getTextClassNameBasedOnDebtAmount(props.model.debtAmount, { noDebtClassName: "opacity-50" })
            )}>
              {props.model.displayDebtAmount.replaceAll("-", "")}
            </span>
          </li>
        </ul>
      </div>
    </div>
  );
}
