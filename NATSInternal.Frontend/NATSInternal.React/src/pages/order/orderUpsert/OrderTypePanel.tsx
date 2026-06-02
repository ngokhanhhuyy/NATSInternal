import React from "react";
import { getDisplayName } from "@/metadata";
import { joinClassName } from "@/helpers";
import style from "./OrderTypePanel.module.css";

// Child components.
import { FormField } from "@/components/form";

// Props.
type OrderTypePanelProps = {
  model: OrderUpsertModel;
  onModelUpdated(updatedData: Partial<OrderUpsertModel>): any;
};

// Components.
export default function OrderTypePanel(props: OrderTypePanelProps): React.ReactNode {
  // Templates.
  return (
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">
          Loại đơn hàng
        </span>
      </div>

      <div className="panel-body grid grid-cols-12 p-3 pt-1.5">
        <FormField
          path="type"
          className="col-span-4 xl:col-span-3"
          displayName={getDisplayName("orderType") ?? undefined}
        >
          <div className={joinClassName("grid grid-cols-3", style.orderTypeButtonContainer)}>
            <button
              type="button"
              className={joinClassName("btn", props.model.type === "Retail" && "btn-primary")}
              onClick={() => props.onModelUpdated({ type: "Retail" })}
            >
              Bán lẻ
            </button>
            
            <button
              type="button"
              className={joinClassName("btn", props.model.type === "Treatment" && "btn-primary")}
              onClick={() => props.onModelUpdated({ type: "Treatment" })}
            >
              Liệu trình
            </button>
            
            <button
              type="button"
              className={joinClassName("btn", props.model.type === "Consultant" && "btn-primary")}
              onClick={() => props.onModelUpdated({ type: "Consultant" })}
            >
              Tư vấn
            </button>
          </div>
        </FormField>
      </div>
    </div>
  );
}
