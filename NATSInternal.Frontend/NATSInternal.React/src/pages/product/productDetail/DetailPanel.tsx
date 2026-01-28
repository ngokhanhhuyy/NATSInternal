import React from "react";

// Child components.
import Field from "@/pages/product/productDetail/Field.tsx";

// Props.
type Props = {
  model: ProductDetailModel;
};

// Components.
export default function DetailPanel(props: Props): React.ReactNode {
  // Template.
  const TargetTransactionTypeAlert = () => {
    if (props.model.isForRetail) {
      return (
        <div className="bg-emerald-500/25 border border-emerald-500 rounded-md px-1 py-0.5 text-xs">
          <span className="text-emerald-500">
            Cả liệu trình và bán lẻ
          </span>
        </div>
      );
    }

    return (
      <div className="bg-blue-500/25 border border-blue-500 rounded-md px-1 py-0.5 text-xs">
        <span className="text-blue-500">
          Chỉ liệu trình
        </span>
      </div>
    );
  };

  const StatusAlert = () => {
    const stock = props.model.stock;
    if (props.model.isDiscontinued) {
      return (
        <div className="bg-red-500/25 border border-red-500 rounded-md px-1 py-0.5 text-xs">
          <span className="text-red-500">
            Đã ngưng kinh doanh
          </span>
        </div>
      );
    } else if (stock != null && stock.stockingQuantity <= stock.resupplyThresholdQuantity) {
      return (
        <div className="bg-yellow-600/25 border border-yellow-600 rounded-md px-1 py-0.5 text-xs">
          <span className="text-yellow-500">
            Cần nhập hàng
          </span>
        </div>
      );
    }
  };

  return (
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">
          Chi tiết sản phẩm
        </span>
      </div>

      <div className="panel-body px-3 pt-3 pb-2">
        <div className="text-2xl text-blue-700 dark:text-blue-400">
          {props.model.name}
        </div>

        <div className="flex flex-wrap gap-2 mb-3">
          <TargetTransactionTypeAlert />
          <StatusAlert />
        </div>

        <div className="flex flex-col gap-y-3">
          {/* Description */}
          {props.model.description && (
            <Field propertyName="description">
              {props.model.description}
            </Field>
          )}

          {/* DefaultAmountBeforeVatPerUnit */}
          <Field propertyName="defaultAmountBeforeVatPerUnit">
            {props.model.formattedDefaultAmountBeforeVatPerUnit}
          </Field>

          {/* DefaultVatPercentagePerUnit */}
          <Field propertyName="defaultVatPercentagePerUnit">
            {props.model.defaultVatPercentagePerUnit}%
          </Field>
        </div>
      </div>
    </div>
  );
}