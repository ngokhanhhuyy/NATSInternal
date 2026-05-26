import React from "react";

// Child components.
import CustomerPicker from "@/pages/shared/upsert/customerPicker";
import { FormField } from "@/components/form";

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

      <div className="panel-body p-3 pt-2">
        <div className="flex flex-col gap-3">
          <FormField path="customer">
            <CustomerPicker
              resourceName="customer"
              value={props.customerModel}
              onValueChanged={(changedModel) => props.onCustomerModelUpdated(changedModel)}
              excludedId={null}
            />
          </FormField>
        </div>
      </div>
    </div>
  );
}
