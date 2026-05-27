import React, { useState, useMemo } from "react";
import { useNavigate } from "react-router";
import { useJSONDirtyModelChecker } from "@/hooks";

// Child components.
import { FormContainer, } from "@/components/layouts";
import { SubmitButton } from "@/components/form";
import StepPanel from "./StepPanel";
import PaymentAndNotePanel from "./PaymentAndNotePanel";
import CustomerPanel from "./CustomerPanel";
import { ChevronLeftIcon, ChevronRightIcon, DevicePhoneMobileIcon } from "@heroicons/react/24/outline";

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
  // Dependencies.
  const navigate = useNavigate();

  // States.
  const [currentStep, setCurrentStep] = useState<number>(1);
  const [isModelDirty] = useJSONDirtyModelChecker(() => props.model.toRequestDto());

  // Computed.
  const steps = useMemo<Map<number, string>>(() => {
    return new Map([
      [1, "Khách hàng"],
      [2, "Sản phẩm và dịch vụ"],
      [3, "Thanh toán và ghi chú"]
    ]);
  }, []);
  
  // Template.
  return (
    <>
      <FormContainer
        className="hidden lg:flex"
        upsertAction={props.upsertAction}
        onUpsertingSucceeded={props.onUpsertingSucceeded}
        onUpsertingFailed={props.onUpsertingFailed}
        isModelDirty={isModelDirty}
      >
        <StepPanel currentStep={currentStep} steps={steps} onStepClicked={(step) => setCurrentStep(step)} />

        {currentStep === 1 && (
          <CustomerPanel
            customerModel={props.model.customer}
            customerUpsertModel={props.model.customerUpsert}
            onCustomerModelUpdated={(customer) => props.onModelUpdated({ customer })}
            onCustomerUpsertModelUpdated={(updatedData) => {
              props.onModelUpdated({ customerUpsert: ({ ...props.model.customerUpsert, ...updatedData }) });
            }}
          />
        )}

        {currentStep === 3 && (
          <PaymentAndNotePanel model={props.model} onModelUpdated={props.onModelUpdated} />
        )}

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

      <div className="flex flex-col gap-3 lg:hidden mt-3">
        <div className="panel">
          <div className="panel-body flex flex-col gap-y-1 justify-center items-center px-5 py-20">
            <div className="grid grid-cols-[auto_auto] justify-center items-center gap-4 opacity-75">
              <DevicePhoneMobileIcon className="size-10 p-2.5 border rounded-full" />
              <div className="flex flex-col justify-center items-start">
                <span>Trang này không hỗ trợ kích thước màn hình hiện tại của bạn.</span>
                <span>Vui lòng sử dụng thiết bị có màn hình lớn hơn.</span>
              </div>
            </div>
          </div>
        </div>

        <button type="button" className="btn gap-1 self-end" onClick={() => navigate(-1)}>
          <ChevronLeftIcon />
          <span>Quay lại</span>
        </button>
      </div>
    </>
  );
}
