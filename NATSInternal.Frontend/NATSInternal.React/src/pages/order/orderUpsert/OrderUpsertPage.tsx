import React from "react";
import { useNavigate } from "react-router";
import { useJSONDirtyModelChecker } from "@/hooks";

// Child components.
import CustomerPanel from "./CustomerPanel";
import ItemListView from "./ItemListView";
import PaymentAndNotePanel from "./PaymentAndNotePanel";
import { FormContainer, } from "@/components/layouts";
import { SubmitButton } from "@/components/form";
import { ChevronLeftIcon, DevicePhoneMobileIcon } from "@heroicons/react/24/outline";

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
  const [isModelDirty] = useJSONDirtyModelChecker(() => props.model.toRequestDto());
  
  // Template.
  return (
    <>
      <FormContainer
        className="hidden lg:flex pb-50"
        upsertAction={props.upsertAction}
        onUpsertingSucceeded={props.onUpsertingSucceeded}
        onUpsertingFailed={props.onUpsertingFailed}
        isModelDirty={isModelDirty}
      >
        <CustomerPanel
          model={props.model.customer}
          onModelUpdated={(updatedData) => {
            props.onModelUpdated({ customer: ({ ...props.model.customer, ...updatedData }) });
          }}
        />
        
        <ItemListView
          model={props.model}
          onModelUpdated={(updatedData) => props.onModelUpdated(updatedData)}
        />
        
        <PaymentAndNotePanel model={props.model} onModelUpdated={props.onModelUpdated} />

        <div className="flex justify-end gap-3">
          <SubmitButton/>
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
