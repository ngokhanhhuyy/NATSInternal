import React from "react";
import { joinClassName, compute } from "@/helpers";
import style from "./ItemListPanel.module.css";

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

      <div className="panel-body w-full overflow-x-auto">
        <table className={joinClassName("data-table table-auto w-full min-w-250 lg:min-w-0", style.dataTable)}>
          <colgroup>
            <col />
            <col className="w-1/2" />
            <col />
            <col />
            <col />
            <col />
          </colgroup>
          <thead>
            <tr>
              <th className="max-w-[50%]" colSpan={2}>Tên</th>
              <th>Loại</th>
              <th>Đơn vị</th>
              <th>Đơn giá</th>
              <th>VAT</th>
              <th>Số lượng</th>
              <th>Thành tiền</th>
            </tr>
          </thead>
          <tbody>
            {props.model.productItems.map((productItem, index) => (
              <tr key={productItem.id}>
                <td>{index + 1}</td>
                <td>
                  <div className="max-w-full">
                    {productItem.product.name}
                  </div>
                </td>
                <td>Sản phẩm</td>
                <td>{productItem.product.unit}</td>
                <td>{productItem.displayAmountBeforeVatPerUnit}</td>
                <td>{productItem.vatPercentagePerUnit}%</td>
                <td>{productItem.quantity}</td>
                <td>{productItem.displayAmountAfterVat}</td>
              </tr>
            ))}

            {props.model.serviceItems.map((serviceItem, index) => (
              <tr key={serviceItem.id}>
                <td>{index + props.model.productItems.length + 1}</td>
                <td>
                  <div className="max-w-full">
                    {serviceItem.name}
                  </div>
                </td>
                <td>Dịch vụ</td>
                <td>-</td>
                <td>{serviceItem.displayAmountBeforeVatPerUnit}</td>
                <td>{serviceItem.vatPercentagePerUnit}%</td>
                <td>{serviceItem.quantity}</td>
                <td>{serviceItem.displayAmountAfterVat}</td>
              </tr>
            ))}

            <tr className="text-end">
              <td className="font-bold" colSpan={7}>Tổng giá đơn hàng</td>
              <td className="text-blue-600 dark:text-blue-400">{props.model.displayAmountAfterVat}</td>
            </tr>

            <tr className="text-end">
              <td className="font-bold" colSpan={7}>Đã thanh toán</td>
              <td className="text-blue-600 dark:text-blue-400">{props.model.displayPaidAmount}</td>
            </tr>

            <tr className="text-end">
              <td className="font-bold" colSpan={7}>Nợ</td>
              <td className="text-blue-600 dark:text-blue-400">{props.model.displayDebtAmount}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  );
}
