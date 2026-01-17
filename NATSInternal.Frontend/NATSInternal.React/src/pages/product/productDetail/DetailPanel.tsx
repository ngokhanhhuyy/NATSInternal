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
  return (
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">
          Chi tiết sản phẩm
        </span>
      </div>

      <div className="panel-body px-3 pt-3 pb-2">
        <div className="text-xl mb-3">
          <span className="text-xl">{props.model.name}</span>
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

          {/* StockingQuantity */}
          <Field propertyName="stockingQuantity">
            {props.model.stockingQuantity} {props.model.unit}
          </Field>
        </div>
      </div>
    </div>
  );
}