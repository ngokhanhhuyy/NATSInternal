import React from "react";

// Child components.
import { FormField, DateTimeInput, TextAreaInput, NumberInput } from "@/components/form";

// Props.
type BasicInformationPanelProps = {
  model: OrderUpsertModel;
  onModelUpdated(updatedData: Partial<OrderUpsertModel>): any;
};

// Components.
export default function PaymentAndNotePanel(props: BasicInformationPanelProps): React.ReactNode {
  // Templates.
  return (
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">
          Thanh toán và ghi chú
        </span>
      </div>

      <div className="panel-body p-3 pt-2">
        <div className="flex flex-col gap-3">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
            <FormField path="statsDate">
              <DateTimeInput
                type="date"
                value={props.model.statsDate}
                onValueChanged={(statsDate) => props.onModelUpdated({ statsDate })}
              />
            </FormField>
            
            <FormField path="paidAmount">
              <div className="form-input-group">
                <NumberInput
                  value={props.model.paidAmount}
                  onValueChanged={(paidAmount) => props.onModelUpdated({ paidAmount })}
                />
                <span className="form-input-group-text border-s-0">vnđ</span>
              </div>
            </FormField>
          </div>

          <FormField path="note">
            <TextAreaInput
              placeholder="Ghi chú về đơn hàng ..."
              value={props.model.note}
              onValueChanged={(note) => props.onModelUpdated({ note })}
            />
          </FormField>
        </div>
      </div>
    </div>
  );
}
