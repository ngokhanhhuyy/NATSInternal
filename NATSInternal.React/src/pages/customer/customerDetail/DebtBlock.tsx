import React from "react";

// Child components.
import MainBlock from "./MainBlock";
import FieldContainer from "./FieldContainer";
import Field from "./Field";

// Props.
type DebtBlock = { model: CustomerDetailModel };

// Component.
export default function DebtBlock(props: DebtBlock): React.ReactNode {
  // Template.
  return (
    <MainBlock title="Thông tin nợ">
      <FieldContainer>
        <Field name="debtRemainingAmount" marginBottom={false}>
          {props.model.displayDebtRemainingAmountText}
        </Field>
      </FieldContainer>
    </MainBlock>
  );
}