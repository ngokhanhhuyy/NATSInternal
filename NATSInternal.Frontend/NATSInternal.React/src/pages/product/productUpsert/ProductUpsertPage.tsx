import React, { useState, useMemo, useEffect } from "react";
import { useDirtyModelChecker } from "@/hooks/dirtyModelCheckerHook";

// Child components.
import { FormContainer } from "@/components/layouts";

// Props.
type Model = ProductCreateModel | ProductUpdateModel;
type Props<TModel extends Model> = {
  description: string;
  isForCreating: boolean;
  model: TModel;
  onModelChanged(changedData: Partial<TModel>): any;
    onModelChanged(changedData: Partial<TModel>): any;
    upsertAction(): Promise<TModel>;
    onUpsertingSucceeded(result: TModel): any;
    deleteAction?(): Promise<void>;
    onDeletionSucceeded?(): any;
    renderButtons?(): React.ReactNode;
};

// Components.
export default function ProductUpsertPage<TModel extends Model>(props: Props<TModel>): React.ReactNode {
  // States.
  const isModelDirty = useDirtyModelChecker(props.model.toRequestDto(), props.model);
  // Template.
  return (
    <FormContainer>

    </FormContainer>
  );
}