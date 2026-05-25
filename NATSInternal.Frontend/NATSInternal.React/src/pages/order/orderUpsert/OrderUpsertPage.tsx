import React from "react";

// Child components.
import { FormContainer } from "@/components/layouts";

// Props.
type OrderUpsertPageProps<TUpsertResult> = {
  model: OrderUpsertModel;
  onModelUpdated(changedData: Partial<OrderUpsertModel>): any;
  upsertAction(): Promise<TUpsertResult>;
  onUpsertingSucceeded(result: TUpsertResult): any;
  onUpsertingFailed?(error: Error, errorHandled: boolean): any;
  isModelDirty: boolean;
};

// Components.
export default function OrderUpsertPage<TUpsertResult>(props: OrderUpsertPageProps<TUpsertResult>): React.ReactNode {
  // Template.
  return (
    <FormContainer
      upsertAction={props.upsertAction}
      onUpsertingSucceeded={props.onUpsertingSucceeded}
      onUpsertingFailed={props.onUpsertingFailed}
      isModelDirty={props.isModelDirty}
    >
      
    </FormContainer>
  );
}
