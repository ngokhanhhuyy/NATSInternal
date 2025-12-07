import React from "react";

// Child components.
import FieldContainer from "./FieldContainer";
import Field from "./Field";
import { Block } from "@/components/ui";

// Props.
type DebtBlock = { model: CustomerDetailModel };

// Component.
export default function DebtBlock(props: DebtBlock): React.ReactNode {
  // Template.
  return (
    <Block title="Thông tin nợ" bodyClassName="p-3">
      <FieldContainer>
        <Field name="debtRemainingAmount" marginBottom={false}>
          {props.model.displayDebtRemainingAmountText}
        </Field>
      </FieldContainer>
    </Block>
  );
}