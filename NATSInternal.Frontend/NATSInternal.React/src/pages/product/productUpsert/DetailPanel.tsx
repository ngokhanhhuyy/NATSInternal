import React, { useMemo } from "react";
import { useLoaderData } from "react-router";
import { getDisplayName } from "@/metadata";
import type { ProductUpsertInitialLoadedModels } from "./ProductUpsertPage";

// Child components.
import { FormField, TextInput, TextAreaInput, NumberInput } from "@/components/form";
import { SelectInput, type SelectInputOption } from "@/components/form";
import { BooleanSelectInput, type BooleanSelectInputOption } from "@/components/form";

// Props.
type DetailPanelProps = {
  model: ProductUpsertModel;
  onModelUpdated(updatedData: Partial<ProductUpsertModel>): any;
};

export default function DetailPanel(props: DetailPanelProps): React.ReactNode {
  // Dependencies.
  const { brandOptionModels, categoryOptionModels } = useLoaderData<ProductUpsertInitialLoadedModels>();

  // Computed.
  const brandDisplayName = useMemo<string>(() => getDisplayName("brand") ?? "", []);
  const categoryDislayName = useMemo<string>(() => getDisplayName("productCategory") ?? "", []);

  const brandOptions = useMemo<(SelectInputOption[])>(() => {
    return [
      { value: "", displayName: "Chưa chọn thương hiệu" },
      ...brandOptionModels.map(dto => ({ value: dto.id, displayName: dto.name }))
    ];
  }, []);

  const categoryOptions = useMemo<SelectInputOption[]>(() => {
    return [
      { value: "", displayName: "Chưa chọn phân loại" },
      ...categoryOptionModels.map(dto => ({ value: dto.name, displayName: dto.name }))
    ];
  }, []);

  // Callbacks.
  function handleBrandChanged(id: string): void {
    if (id) {
      props.onModelUpdated({ brand: brandOptionModels.find(b => b.id === id) });
    }

    props.onModelUpdated({ brand: null });
  }

  function handleProductCategoryChanged(name: string): void {
    if (name) {
      props.onModelUpdated({ category: categoryOptionModels.find(b => b.name === name) });
    }

    props.onModelUpdated({ category: null });
  }

  // Template.
  return (
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">
          Thông tin sản phẩm
        </span>
      </div>

      <div className="panel-body flex flex-col gap-3 px-4 pt-2.5 pb-4">
        <div className="grid grid-cols-1 sm:grid-cols-6 gap-3">
          {/* Name */}
          <FormField path="name" className="sm:col-span-4">
            <TextInput
              placeholder="Sản phẩm A"
              value={props.model.name}
              onValueChanged={(name) => props.onModelUpdated({ name })}
            />
          </FormField>
          
          {/* Unit */}
          <FormField path="unit" className="sm:col-span-2">
            <TextInput
              placeholder="Chai, lọ, vĩ, hộp, ..."
              value={props.model.unit}
              onValueChanged={(unit) => props.onModelUpdated({ unit })}
            />
          </FormField>

          {/* Description */}
          <FormField path="description" className="sm:col-span-6">
            <TextAreaInput
              placeholder="Mô tả sản phẩm"
              value={props.model.description}
              onValueChanged={(description) => props.onModelUpdated({ description })}
            />
          </FormField>
        </div>

        <div className="grid grid-cols-1 sm:grid-cols-6 gap-3">
          {/* DefaultAmountBeforeVatPerUnit */}
          <FormField path="defaultAmountBeforeVatPerUnit" className="sm:col-span-3">
            <div className="form-input-group">
              <NumberInput
                value={props.model.defaultAmountBeforeVatPerUnit}
                onValueChanged={(defaultAmountBeforeVatPerUnit) => {
                  props.onModelUpdated({ defaultAmountBeforeVatPerUnit });
                }}
              />
              <div className="form-input-group-text border-s-0">vnđ</div>
            </div>
          </FormField>

          {/* DefaultVatPercentagePerUnit */}
          <FormField path="defaultVatPercentagePerUnit" className="sm:col-span-3">
            <div className="form-input-group">
              <NumberInput
                value={props.model.defaultVatPercentagePerUnit}
                onValueChanged={(defaultVatPercentagePerUnit) => {
                  props.onModelUpdated({ defaultVatPercentagePerUnit });
                }}
              />
              <div className="form-input-group-text border-s-0">%</div>
            </div>
          </FormField>

          {/* Brand */}
          <FormField path="brand.id" displayName={brandDisplayName} className="sm:col-span-3">
            <SelectInput
              options={brandOptions}
              value={props.model.brand?.id ?? ""}
              onValueChanged={handleBrandChanged}
            />
          </FormField>

          {/* Category */}
          <FormField path="category.name" displayName={categoryDislayName} className="sm:col-span-3">
            <SelectInput
              options={categoryOptions}
              value={props.model.category?.name ?? ""}
              onValueChanged={handleProductCategoryChanged}
            />
          </FormField>


          {/* IsForRetail */}
          <FormField path="isForRetail" displayName="Dành cho loại giao dịch" className="sm:col-span-3">
            <BooleanSelectInput
              options={isForRetailInputOptions}
              value={props.model.isForRetail}
              onValueChanged={(isForRetail) => props.onModelUpdated({ isForRetail })}
            />
          </FormField>

          {/* IsDiscontinued */}
          <FormField path="isDiscontinued" displayName="Tình trạng" className="sm:col-span-3">
            <BooleanSelectInput
              options={isDiscontinuedInputOptions}
              value={props.model.isDiscontinued}
              onValueChanged={(isDiscontinued) => props.onModelUpdated({ isDiscontinued })}
            />
          </FormField>
        </div>
      </div>
    </div>
  );
}

const isForRetailInputOptions: BooleanSelectInputOption[] = [
  { value: true, displayName: "Cả liệu trình và bán lẻ" },
  { value: false, displayName: "Chỉ liệu trình" }
];

const isDiscontinuedInputOptions: BooleanSelectInputOption[] = [
  { value: true, displayName: "Đã ngưng sử dụng trong giao dịch" },
  { value: false, displayName: "Có thể sử dụng trong giao dịch" }
];

