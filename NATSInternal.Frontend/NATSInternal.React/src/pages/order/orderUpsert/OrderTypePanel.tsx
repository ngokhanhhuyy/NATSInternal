import React from "react";
import { joinClassName } from "@/helpers";

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
      <div className="panel-body grid grid-cols-5">
        <FormField path="type">
          <div className="grid grid-cols-3">
            <button
              type="button"
              className={joinClassName("btn", props.model.type === "Retail" ? "btn-primary" : "border-e-0")}
              onClick={props.onModelUpdated({ type: "Retail" })}
            />
            
            <button
              type="button"
              className={joinClassName("btn", props.model.type === "Treatment" ? "btn-primary" : "border-x-0")}
              onClick={props.onModelUpdated({ type: "Treatment" })}
            />
            
            <button
              type="button"
              className={joinClassName("btn", props.model.type === "Consultant" ? "btn-primary" : "border-s-0")}
              onClick={props.onModelUpdated({ type: "Consultant" })}
            />
          </div>
        </FormField>
      </div>
    </div>
  );
}
