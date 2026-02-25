import React from "react";
import { useApi } from "@/api";
import { useJSONDirtyModelChecker } from "@/hooks/jsonDirtyModelCheckerHook";
import { createBrandBasicModel, createProductCategoryBasicModel } from "@/models";

// Child components.
import DetailPanel from "./DetailPanel";
import StockPanel from "./StockPanel";
import { FormContainer } from "@/components/layouts";
import { SubmitButton, DeleteButton } from "@/components/form";

// Types.
export type ProductUpsertInitialLoadedModels = {
  brandOptionModels: BrandBasicModel[];
  categoryOptionModels: ProductCategoryBasicModel[];
};

// Data loader.
export async function loadBrandAndCategoryOptionsAsync(): Promise<ProductUpsertInitialLoadedModels> {
  const api = useApi();
  const [brandResponseDtos, categoryResponseDtos] = await Promise.all([
    api.brand.getAllAsync(),
    api.productCategory.getAllAsync()
  ]);

  return {
    brandOptionModels: brandResponseDtos.map(createBrandBasicModel),
    categoryOptionModels: categoryResponseDtos.map(createProductCategoryBasicModel)
  };
}

// Props.
type Props = {
  isForCreating: boolean;
  model: ProductUpsertModel;
  onModelUpdated(updatedData: Partial<ProductUpsertModel>): any;
  upsertAction(): Promise<void>;
  onUpsertingSucceeded(): any;
  deleteAction?(): Promise<void>;
  onDeletionSucceeded?(): any;
  renderButtons?(): React.ReactNode;
};

// Components.
export default function ProductUpsertPage(props: Props): React.ReactNode {
  // States.
  const isModelDirty = useJSONDirtyModelChecker(props.model);
  
  // Template.
  return (
    <FormContainer
      upsertAction={props.upsertAction}
      onUpsertingSucceeded={props.onUpsertingSucceeded}
      isModelDirty={isModelDirty}
    >
      <DetailPanel model={props.model} onModelUpdated={props.onModelUpdated} />
      <StockPanel
        model={props.model.stock}
        onModelChanged={(changedData) => props.onModelUpdated({ stock: { ...props.model.stock, ...changedData } })}
        isForCreating={props.isForCreating}
      />
      
      <div className="flex justify-end gap-3">
        {props.deleteAction && <DeleteButton/>}
        <SubmitButton/>
      </div>
    </FormContainer>
  );
}