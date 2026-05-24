import React from "react";

// Child components.
import { Field, FieldContainer } from "@/pages/shared/detail";

// Props.
type DebtPanelProps = { model: CustomerDetailModel };

// Component.
export default function DebtPanel(props: DebtPanelProps): React.ReactNode {
  // Template.
  return (
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">Thông tin nợ</span>
      </div>

      <div className="panel-body p-3">
        <FieldContainer>
          <Field name="debtRemainingAmount">
            {props.model.displayDebtRemainingAmountText}
          </Field>
        </FieldContainer>
      </div>
    </div>
  );
}
