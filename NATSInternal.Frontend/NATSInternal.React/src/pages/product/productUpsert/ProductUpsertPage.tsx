import React from "react";
import { api } from "@/api";
import { useJSONDirtyModelChecker } from "@/hooks/jsonDirtyModelCheckerHook";
import { createProductCategoryBasicModel } from "@/models";

// Child components.
import DetailPanel from "./DetailPanel";
import StockPanel from "./StockPanel";
import { FormContainer } from "@/components/layouts";
import { SubmitButton, DeleteButton } from "@/components/form";

// Data loader.
export async function loadCategoryOptionsAsync(): Promise<ProductCategoryBasicModel[]> {
  const responseDtos = await api.productCategory.getAllAsync();
  return responseDtos.map(createProductCategoryBasicModel);
}

// Props.
type Props = {
  isForCreating: boolean;
  model: ProductUpsertModel;
  onModelUpdated(updatedData: Partial<ProductUpsertModel>): any;
  categoryModels: ProductCategoryBasicModel[];
  upsertAction(): Promise<void>;
  onUpsertingSucceeded(): any;
  deleteAction?(): Promise<void>;
  onDeletionSucceeded?(): any;
  renderButtons?(): React.ReactNode;
};

// Components.
export default function ProductUpsertPage(props: Props): React.ReactNode {
  // States.
  const [isModelDirty] = useJSONDirtyModelChecker(props.model);
  
  // Template.
  return (
    <FormContainer
      upsertAction={props.upsertAction}
      onUpsertingSucceeded={props.onUpsertingSucceeded}
      isModelDirty={isModelDirty}
    >
      <DetailPanel
        model={props.model}
        onModelUpdated={props.onModelUpdated}
        categoryModels={props.categoryModels}
      />

      <StockPanel
        model={props.model}
        onModelChanged={(changedData) => props.onModelUpdated(changedData)}
        isForCreating={props.isForCreating}
      />
      
      <div className="flex justify-end gap-3">
        {props.deleteAction && <DeleteButton/>}
        <SubmitButton/>
      </div>
    </FormContainer>
  );
}
