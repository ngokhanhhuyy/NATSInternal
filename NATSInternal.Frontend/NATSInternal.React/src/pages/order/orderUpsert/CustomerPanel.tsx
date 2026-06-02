import React from "react";
import { joinClassName } from "@/helpers";

// Child components.
import CustomerPicker from "@/pages/shared/upsert/customerPicker";
import CustomerUpsertInputs from "@/pages/shared/upsert/customerUpsertInputs";
import { FormField } from "@/components/form";
import { XMarkIcon } from "@heroicons/react/24/outline";

// Props.
type CustomerPanelProps = {
  model: OrderUpsertCustomerModel;
  onModelUpdated(updatedData: Partial<OrderUpsertCustomerModel>): any;
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

      <div className="panel-body flex flex-col gap-x-3 gap-y-2">
        <div className={joinClassName(
          "grid grid-cols-2 xl:grid-cols-3 gap-3 border-b",
          "border-neutral-900/15 dark:border-neutral-50/15 p-3 pt-2"
        )}>
          <FormField path="customer.createNewCustomer" displayName="Phương thức chọn khách hàng">
            <div className="grid grid-cols-2 justify-stretch">
              <button
                type="button"
                className={joinClassName(
                  "btn rounded-r-none",
                  !props.model.createNewCustomer ? "btn-primary" : "border-r-transparent"
                )}
                onClick={() => props.onModelUpdated({ createNewCustomer: false })}
              >
                Chọn khách hàng cũ
              </button>

              <button
                type="button"
                className={joinClassName(
                  "btn rounded-l-none",
                  props.model.createNewCustomer ? "btn-primary" : "border-l-transparent"
                )}
                onClick={() => props.onModelUpdated({ createNewCustomer: true })}
              >
                Tạo khách hàng mới
              </button>
            </div>
          </FormField>
        </div>
        
        {!props.model.createNewCustomer && (
          <div className="grid grid-cols-3 gap-3 px-3 pb-3">
            <FormField path="customer" displayName="Chọn khách hàng có sẵn">
              <div className="flex gap-2">
                <CustomerPicker
                  resourceName="customer"
                  value={props.model.basic}
                  onValueChanged={(basic) => props.onModelUpdated({ basic })}
                  excludedId={null}
                />

                {props.model.basic && (
                  <button type="button" className="btn btn-danger" onClick={() => props.onModelUpdated({ basic: null })}>
                    <XMarkIcon/>
                  </button>
                )}
              </div>
            </FormField>
          </div>
        )}

        {props.model.createNewCustomer && (
          <div className="flex flex-col gap-3 px-3 pb-3">
            <CustomerUpsertInputs
              model={props.model.create}
              onModelChanged={(updatedData) => {
                props.onModelUpdated({ create: ({ ...props.model.create, ...updatedData }) });
              }}
              isForCreating
              pathPrefix="customer.create"
            />
          </div>
        )}
      </div>
    </div>
  );
}
