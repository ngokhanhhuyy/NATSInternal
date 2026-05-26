import React from "react";

// Child components.
import { FormField, DateTimeInput, TextAreaInput } from "@/components/form";

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

      <div className="panel-body p-3 pt-2">
        <div className="flex flex-col gap-3">
          <FormField path="statsDate">
            <DateTimeInput
              type="date"
              value={props.model.statsDate}
              onValueChanged={(statsDate) => props.onModelUpdated({ statsDate })}
            />
          </FormField>

          <FormField path="note">
            <TextAreaInput
              value={props.model.note}
              onValueChanged={(note) => props.onModelUpdated({ note })}
            />
          </FormField>
        </div>
      </div>
    </div>
  );
}
