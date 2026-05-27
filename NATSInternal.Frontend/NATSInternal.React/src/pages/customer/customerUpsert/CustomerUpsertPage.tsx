import React from "react";
import { useJSONDirtyModelChecker } from "@/hooks";

// Child components.
import { FormContainer } from "@/components/layouts";
import CustomerUpsertInputs from "@/pages/shared/upsert/customerUpsertInputs";
import { SubmitButton, DeleteButton } from "@/components/form";

// Props.
type CustomerUpsertPageProps<T> = {
  description: string;
  isForCreating: boolean;
  id?: number;
  model: CustomerUpsertModel;
  onModelChanged(changedData: Partial<CustomerUpsertModel>): any;
  upsertAction(): Promise<T>;
  onUpsertingSucceeded(result: T): any;
  deleteAction?(): Promise<void>;
  onDeletionSucceeded?(): any;
  renderButtons?(): React.ReactNode;
};

// Component.
export default function CustomerUpsertPage<T>(props: CustomerUpsertPageProps<T>): React.ReactNode {
  // States.
  const [isModelDirty] = useJSONDirtyModelChecker(props.model.toRequestDto());

  // Template;
  return (
    <FormContainer
      upsertAction={props.upsertAction}
      onUpsertingSucceeded={props.onUpsertingSucceeded}
      isModelDirty={isModelDirty}
    >
      <div className="panel">
        <div className="panel-header">
          <span className="panel-header-title">
            Thông tin cá nhân khách hàng
          </span>
        </div>

        <div className="panel-body flex flex-col gap-3 px-4 pt-2.5 pb-4">
          <CustomerUpsertInputs
            id={props.id}
            model={props.model}
            onModelChanged={props.onModelChanged}
            isForCreating={props.isForCreating}
          />
        </div>
      </div>

      <div className="flex justify-end gap-3">
        {props.deleteAction && <DeleteButton/>}
        <SubmitButton/>
      </div>
    </FormContainer>
  );
}
