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
        <div className="alert alert-emerald-outline dark:alert-emerald dark:font-bold alert-sm">
          Cả liệu trình và bán lẻ
        </div>
      );
    }

    return (
      <div className="alert alert-blue-outline dark:alert-blue dark:font-bold alert-sm">
        Chỉ liệu trình
      </div>
    );
  };

  const StatusAlert = () => {
    const stock = props.model.stock;
    if (props.model.isDiscontinued) {
      return (
        <div className="alert alert-neutral-outline dark:alert-neutral dark:font-bold alert-sm">
          Đã ngưng kinh doanh
        </div>
      );
    }
    
    if (stock != null) {
      if (stock.stockingQuantity === 0) {
        return (
          <div className="alert alert-red-outline dark:alert-red dark:font-bold alert-sm">
            Đã hết hàng
          </div>
        );
      }

      if (stock.stockingQuantity <= stock.resupplyThresholdQuantity) {
        return (
          <div className="alert alert-yellow-outline dark:alert-yellow dark:font-bold alert-sm">
            Sắp hết hàng
          </div>
        );
      }
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