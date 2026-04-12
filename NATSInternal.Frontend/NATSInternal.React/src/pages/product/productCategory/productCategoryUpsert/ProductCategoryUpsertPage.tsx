import React from "react";
import { useJSONDirtyModelChecker } from "@/hooks";

// Child components.
import { FormContainer } from "@/components/layouts";
import { FormField, TextInput } from "@/components/form";
import { SubmitButton, DeleteButton } from "@/components/form";

// Props.
type Props<TUpsertResult extends string | void> = {
  isForCreating: boolean;
  model: ProductCategoryUpsertModel;
  onModelUpdated(updatedData: Partial<ProductCategoryUpsertModel>): any;
  upsertAction(): Promise<TUpsertResult>;
  onUpsertingSucceeded(upsertResult: TUpsertResult): any;
  deleteAction?(): Promise<void>;
  onDeletionSucceeded?(): any;
  renderButtons?(): React.ReactNode;
};

// Component.
const ProductCategoryUpsertPage = <TUpsertResult extends string | void>(props: Props<TUpsertResult>): React.ReactNode => {
  // States.
  const isModelDirty = useJSONDirtyModelChecker(props.model);

  // Template.
  return (
    <FormContainer
      upsertAction={props.upsertAction}
      deleteAction={props.deleteAction}
      onUpsertingSucceeded={props.onUpsertingSucceeded}
      onDeletionSucceeded={props.onDeletionSucceeded}
      isModelDirty={isModelDirty}
    >
      <div className="panel">
        <div className="panel-header">
          <span className="panel-header-title">
            Thông tin chi tiết
          </span>
        </div>

        <div className="panel-body flex flex-col gap-3 p-3">
          <FormField path="name" displayName="Tên thương hiệu">
            <TextInput
              required
              value={props.model.name}
              onValueChanged={(name) => props.onModelUpdated({ name })}
              placeholder="Tên thương hiệu"
            />
          </FormField>
        </div>
      </div>

      <div className="flex justify-end gap-3">
        {props.deleteAction && <DeleteButton onClick={props.deleteAction} />}
        <SubmitButton/>
      </div>
    </FormContainer>
  );
};

export default ProductCategoryUpsertPage;