import React, { useState, useMemo } from "react";
import { useJSONDirtyModelChecker } from "@/hooks";

// Child components.
import { FormContainer, } from "@/components/layouts";
import { SubmitButton } from "@/components/form";
import StepPanel from "./StepPanel";
import BasicInformationPanel from "./BasicInformationPanel";
import CustomerPanel from "./CustomerPanel";
import { ChevronLeftIcon, ChevronRightIcon } from "@heroicons/react/24/outline";

// Props.
type OrderUpsertPageProps<TUpsertResult> = {
  model: OrderUpsertModel;
  onModelUpdated(changedData: Partial<OrderUpsertModel>): any;
  upsertAction(): Promise<TUpsertResult>;
  onUpsertingSucceeded(result: TUpsertResult): any;
  onUpsertingFailed?(error: Error, errorHandled: boolean): any;
};

// Components.
export default function OrderUpsertPage<TUpsertResult>(props: OrderUpsertPageProps<TUpsertResult>): React.ReactNode {
  // States.
  const [currentStep, setCurrentStep] = useState<number>(1);
  const [isModelDirty] = useJSONDirtyModelChecker(() => props.model.toRequestDto());

  // Computed.
  const steps = useMemo<Map<number, string>>(() => {
    return new Map([
      [1, "Khách hàng"],
      [2, "Sản phầm và dịch vụ"],
      [3, "Thanh toán và ghi chú"]
    ]);
  }, []);
  
  // Template.
  return (
    <FormContainer
      upsertAction={props.upsertAction}
      onUpsertingSucceeded={props.onUpsertingSucceeded}
      onUpsertingFailed={props.onUpsertingFailed}
      isModelDirty={isModelDirty}
    >
      <StepPanel currentStep={currentStep} steps={steps} />

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-3">
        <BasicInformationPanel model={props.model} onModelUpdated={props.onModelUpdated} />
        <CustomerPanel
          customerModel={props.model.customer}
          customerUpsertModel={props.model.customerUpsert}
          onCustomerModelUpdated={(customer) => props.onModelUpdated({ customer })}
          onCustomerUpsertModelUpdated={(updatedData) => {
            props.onModelUpdated({ customerUpsert: ({ ...props.model.customerUpsert, ...updatedData }) });
          }}
        />
      </div>

      <div className="flex justify-end gap-3">
        {currentStep > 1 && (
          <button type="button" className="btn gap-0.5" onClick={() => setCurrentStep(step => step - 1)}>
            <ChevronLeftIcon />
            <span>Quay lại</span>
          </button>
        )}

        {currentStep < Math.max(...steps.keys()) && (
          <button type="button" className="btn gap-0.5" onClick={() => setCurrentStep(step => step + 1)}>
            <span>Tiếp theo</span>
            <ChevronRightIcon />
          </button>
        )}

        {currentStep === Math.max(...steps.keys()) && (
          <SubmitButton/>
        )}
      </div>
    </FormContainer>
  );
}
