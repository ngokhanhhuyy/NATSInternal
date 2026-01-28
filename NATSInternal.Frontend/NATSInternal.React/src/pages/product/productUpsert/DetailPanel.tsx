import React, { useMemo } from "react";

// Child components.
import { FormField, TextInput, TextAreaInput, NumberInput, BooleanSelectInput } from "@/components/form";

// Props.
type DetailPanelProps = {
  model: ProductUpsertModel;
  onModelChanged(changedData: Partial<ProductUpsertModel>): any;
};

export default function DetailPanel(props: DetailPanelProps): React.ReactNode {
  // Computed.
  const isForRetailInputOptions = useMemo(() => ([
    { value: true, displayName: "Cả liệu trình và bán lẻ" },
    { value: false, displayName: "Chỉ liệu trình" }
  ]), []);
  
  const isDiscontinuedInputOptions = useMemo(() => ([
    { value: true, displayName: "Đã ngưng sử dụng trong giao dịch" },
    { value: false, displayName: "Có thể sử dụng trong giao dịch" }
  ]), []);

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
              onValueChanged={(name) => props.onModelChanged({ name })}
            />
          </FormField>
          
          {/* Unit */}
          <FormField path="unit" className="sm:col-span-2">
            <TextInput
              placeholder="Chai, lọ, vĩ, hộp, ..."
              value={props.model.unit}
              onValueChanged={(unit) => props.onModelChanged({ unit })}
            />
          </FormField>

          {/* Description */}
          <FormField path="description" className="sm:col-span-6">
            <TextAreaInput
              placeholder="Mô tả sản phẩm"
              value={props.model.description}
              onValueChanged={(description) => props.onModelChanged({ description })}
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
                  props.onModelChanged({ defaultAmountBeforeVatPerUnit });
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
                  props.onModelChanged({ defaultVatPercentagePerUnit });
                }}
              />
              <div className="form-input-group-text border-s-0">%</div>
            </div>
          </FormField>

          {/* IsForRetail */}
          <FormField path="isForRetail" displayName="Dành cho loại giao dịch" className="sm:col-span-3">
            <BooleanSelectInput
              options={isForRetailInputOptions}
              value={props.model.isForRetail}
              onValueChanged={(isForRetail) => props.onModelChanged({ isForRetail })}
            />
          </FormField>

          {/* IsDiscontinued */}
          <FormField path="isDiscontinued" displayName="Tình trạng" className="sm:col-span-3">
            <BooleanSelectInput
              options={isDiscontinuedInputOptions}
              value={props.model.isDiscontinued}
              onValueChanged={(isDiscontinued) => props.onModelChanged({ isDiscontinued })}
            />
          </FormField>
        </div>
      </div>
    </div>
  );
}