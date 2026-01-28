import React from "react";

// Child components.
import { FormField, NumberInput } from "@/components/form";

// Props.
type StockPanelProps = {
  model: ProductUpsertStockModel;
  onModelChanged(changedData: Partial<ProductUpsertStockModel>): any;
  isForCreating: boolean;
};

export default function StockPanel(props: StockPanelProps): React.ReactNode {
  // Template.
  return (
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">
          Thông tin kho hàng
        </span>
      </div>

      <div className="panel-body flex gap-3 px-4 pt-2.5 pb-4">
        {/* StockingQuantity */}
        {props.isForCreating && (
          <FormField path="stock.stockingQuantity" className="flex-1">
            <NumberInput
              value={props.model.stockingQuantity}
              onValueChanged={(stockingQuantity) => {
                props.onModelChanged({ stockingQuantity });
              }}
            />
          </FormField>
        )}
        
        {/* ResupplyTHresholdQuantity */}
        <FormField path="stock.resupplyThresholdQuantity" className="flex-1">
          <NumberInput
            value={props.model.resupplyThresholdQuantity}
            onValueChanged={(resupplyThresholdQuantity) => {
              props.onModelChanged({ resupplyThresholdQuantity });
            }}
          />
        </FormField>
      </div>
    </div>
  );
}