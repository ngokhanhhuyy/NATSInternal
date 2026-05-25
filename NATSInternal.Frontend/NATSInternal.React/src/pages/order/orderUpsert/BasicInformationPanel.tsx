import React from "react";

// Child components.
import { FormField, DateInput } from "@/components/form";

// Props.
type BasicInformationPanelProps = {
  model: OrderUpsertModel;
  onModelUpdated(updatedData: Partial<OrderUpsertModel>): any;
};

// Components.
export default function BasicInformationPanel(props: BasicInformationPanelProps): React.ReactNode {
  // Templates.
  return (
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">
          Thông tin cơ bản
        </span>
      </div>

      <div className="panel-body">

      </div>
    </div>
  );
}
