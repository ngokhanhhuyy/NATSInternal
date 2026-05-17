import React, { useMemo } from "react";
import { useLoaderData } from "react-router";
import { getDisplayName } from "@/metadata";

// Child components.
import type { ProductListDataLoaderResults } from "./dataLoader";
import { FormField, SelectInput, type SelectInputOption } from "@/components/form";

type Props = {
  model: ProductListModel;
  onModelUpdated(updatedData: Partial<ProductListModel>): void;
};

export default function FilterPanelChildren(props: Props): React.ReactNode {
  // Dependencies.
  const initialModels = useLoaderData<ProductListDataLoaderResults>();

  // Computed.
  const categoryOptions = useMemo<SelectInputOption[]>(() => {
    const options = initialModels.categoryModels.map(category => ({
      value: category.id.toString(),
      displayName: category.name
    }));

    return [
      { value: "", displayName: "Tất cả phân loại" },
      ...options
    ];
  }, []);

  // Callbacks.
  function handleCategorySelectionChanged(idAsString: string): void {
    if (idAsString) {
      const id = parseInt(idAsString);
      const category = initialModels.categoryModels.find(pc => pc.id == id)!;
      props.onModelUpdated({ category });
      return;
    }

    props.onModelUpdated({ category: null });
  }

  // Template.
  return (
    <FormField path="categoryId" displayName={getDisplayName("category") ?? undefined}>
      <SelectInput
        options={categoryOptions}
        value={props.model.category?.id.toString() ?? ""}
        onValueChanged={handleCategorySelectionChanged}
      />
    </FormField>
  );
}
