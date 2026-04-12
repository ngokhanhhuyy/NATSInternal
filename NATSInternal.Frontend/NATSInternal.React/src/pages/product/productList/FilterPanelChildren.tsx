import React, { useMemo } from "react";
import { useLoaderData } from "react-router";

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
  const brandOptions = useMemo<SelectInputOption[]>(() => {
    return [
      { value: "", displayName: "Chưa chọn thương hiệu" },
      ...(initialModels.brandOptions.map((brand) => ({ value: brand.id, displayName: brand.name })) ?? [])
    ];
  }, []);

  const categoryOptions = useMemo<SelectInputOption[]>(() => {
    return [
      { value: "", displayName: "Chưa chọn phân loại" },
      ...(initialModels.categoryOptions?.map((category) => ({ value: category.name, displayName: category.name })) ?? [])
    ];
  }, []);

  // Callback.
  function handleBrandChanged(id: string): void {
    if (id) {
      props.onModelUpdated({ brand: initialModels.brandOptions.find(b => b.id === id) ?? null });
      return;
    }

    props.onModelUpdated({ brand: null });
  };
  
  function handleCategoryChanged(name: string): void {
    if (name) {
      props.onModelUpdated({ category: initialModels.categoryOptions.find(b => b.name === name) ?? null });
      return;
    }

    props.onModelUpdated({ category: null });
  };

  // Template.
  return (
    <div className={"grid grid-cols-1 sm:grid-cols-2 gap-3"}>
      <FormField path="brandId" displayName="Thương hiệu">
        <SelectInput
          options={brandOptions}
          value={props.model.brand?.id ?? ""}
          onValueChanged={handleBrandChanged}
        />
      </FormField>

      <FormField path="categoryName" displayName="Phân loại">
        <SelectInput
          options={categoryOptions}
          value={props.model.category?.name ?? ""}
          onValueChanged={handleCategoryChanged}
        />
      </FormField>
    </div>
  );
}