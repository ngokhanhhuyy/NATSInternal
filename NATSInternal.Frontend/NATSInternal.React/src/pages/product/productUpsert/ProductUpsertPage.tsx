import React from "react";
import { useDirtyModelChecker } from "@/hooks/dirtyModelCheckerHook";
import { useTsxHelper } from "@/helpers";

// Child components.
import DetailPanel from "./DetailPanel";
import StockPanel from "./StockPanel";
import { FormContainer } from "@/components/layouts";
import { SubmitButton, DeleteButton } from "@/components/form";

// Props.
type Props = {
  description: string;
  isForCreating: boolean;
  model: ProductUpsertModel;
  onModelChanged(changedData: Partial<ProductUpsertModel>): any;
  upsertAction(): Promise<void>;
  onUpsertingSucceeded(): any;
  deleteAction?(): Promise<void>;
  onDeletionSucceeded?(): any;
  renderButtons?(): React.ReactNode;
};

// Components.
export default function ProductUpsertPage(props: Props): React.ReactNode {
  // Dependencies.
  const { compute } = useTsxHelper();

  // States.
  const isModelDirty = useDirtyModelChecker(compute(() => {
    if (props.isForCreating) {
      return props.model.toCreateRequestDto();
    }

    return props.model.toUpdateRequestDto();
  }));
  
  // Template.
  return (
    <FormContainer
      description={props.description}
      upsertAction={props.upsertAction}
      onUpsertingSucceeded={props.onUpsertingSucceeded}
      isModelDirty={isModelDirty}
    >
      <DetailPanel model={props.model} onModelChanged={props.onModelChanged} />
      <StockPanel
        model={props.model.stock}
        onModelChanged={(changedData) => props.onModelChanged({ stock: { ...props.model.stock, ...changedData } })}
        isForCreating={props.isForCreating}
      />
      
      <div className="flex justify-end gap-3">
        {props.deleteAction && <DeleteButton/>}
        <SubmitButton/>
      </div>

      <pre className="border border-blue-500 p-3 rounded-lg">{JSON.stringify(props.model, null, 2)}</pre>
    </FormContainer>
  );
}