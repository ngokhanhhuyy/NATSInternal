import React from "react";

// Child components.
import Field from "@/pages/product/productDetail/Field";
import { ExclamationTriangleIcon } from "@heroicons/react/24/outline";

// Props.
type Props = {
  model: ProductDetailModel;
};

// Components.
export default function StockPanel({ model }: Props): React.ReactNode {
  // Template.
  return (
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">
          Thông tin kho hàng
        </span>
      </div>

      <div className="panel-body px-3 pt-3 pb-2">
        <div className="flex flex-col gap-y-3">
          {/* StockingQuantity */}
          <Field propertyName="stockingQuantity" className="flex gap-3">
            {model.stockingQuantity}
            {!model.isDiscontinued && model.stockingQuantity <= (model.resupplyThresholdQuantity ?? 0) && (
              <div className="flex items-center gap-1 text-yellow-600 dark:text-yellow-400 text-sm">
                <ExclamationTriangleIcon className="size-4.5" />
                <span>Cần nhập hàng</span>
              </div>
            )}
          </Field>

          {/* ResupplyStockingQuantity */}
          {model.resupplyThresholdQuantity && (
            <Field propertyName="resupplyThresholdQuantity">
              {model.resupplyThresholdQuantity ?? 0}
            </Field>
          )}
        </div>
      </div>
    </div>
  );
}
