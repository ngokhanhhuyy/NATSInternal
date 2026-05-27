import React from "react";
import { joinClassName } from "@/helpers";

// Child components.
import CustomerPicker from "@/pages/shared/upsert/customerPicker";
import CustomerUpsertInputs from "@/pages/shared/upsert/customerUpsertInputs";
import { FormField } from "@/components/form";
import { XMarkIcon } from "@heroicons/react/24/outline";

// Props.
type CustomerPanelProps = {
 customerModel: CustomerBasicModel | null;
 customerUpsertModel: CustomerUpsertModel;
 onCustomerModelUpdated(pickedModel: CustomerBasicModel | null): any;
 onCustomerUpsertModelUpdated(updatedData: Partial<CustomerUpsertModel>): any;
};

// Components.
export default function CustomerPanel(props: CustomerPanelProps): React.ReactNode {
  // Templates.
  return (
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">
          Khách hàng
        </span>
      </div>

      <div className="panel-body">
        <div className="flex flex-col p-3 pt-2">
          <FormField path="customer" displayName="Chọn khách hàng có sẵn">
            <div className="flex gap-2">
              <CustomerPicker
                resourceName="customer"
                value={props.customerModel}
                onValueChanged={(changedModel) => props.onCustomerModelUpdated(changedModel)}
                excludedId={null}
              />

              {props.customerModel && (
                <button className="btn btn-danger" type="button" onClick={() => props.onCustomerModelUpdated(null)}>
                  <XMarkIcon/>
                </button>
              )}
            </div>
          </FormField>
        </div>

        {!props.customerModel && (
          <div className={joinClassName(
            "flex flex-col gap-3 px-4 pt-2.5 pb-4",
            "border-t border-neutral-900/15 dark:border-neutral-50/15"
          )}>
            <CustomerUpsertInputs
              model={props.customerUpsertModel}
              onModelChanged={props.onCustomerUpsertModelUpdated}
              isForCreating
              pathPrefix="customer"
            />
          </div>
        )}
      </div>
    </div>
  );
}
