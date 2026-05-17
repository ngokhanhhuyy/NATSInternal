import React, { useMemo } from "react";
import { useLoaderData } from "react-router";
import { getDisplayName } from "@/metadata";

// Child components.
import type { ProductListDataLoaderResults } from "./dataLoader";
import { FormField } from "@/components/form";
import CategoryButton from "./CategoryButton";

type Props = {
  model: ProductListModel;
  onModelUpdated(updatedData: Partial<ProductListModel>): void;
};

export default function FilterPanelChildren(props: Props): React.ReactNode {
  // Dependencies.
  const initialModels = useLoaderData<ProductListDataLoaderResults>();

  // Computed.
  const selectableCategories = useMemo<ProductCategoryBasicModel[]>(() => {
    return initialModels.categoryOptions ?? [];
  }, []);

  // Callbacks.
  function handleCategorySelectionChanged(category: ProductCategoryBasicModel | null): void {
    if (category != null) {
      props.onModelUpdated({ category: category });
      return;
    }

    props.onModelUpdated({ category: null });
  };

  // Template.
  return (
    <FormField path="categoryId" displayName={getDisplayName("category") ?? undefined}>
      <div className={"flex gap-3"}>
        <CategoryButton
          model={null}
          isSelected={props.model.category?.id == null}
          onClick={handleCategorySelectionChanged}
        />

        {selectableCategories.map(category => (
          <CategoryButton
            model={category}
            isSelected={props.model.category?.id === category.id}
            onClick={handleCategorySelectionChanged}
            key={category.id}
          />
        ))}
      </div>
    </FormField>
  );
}
