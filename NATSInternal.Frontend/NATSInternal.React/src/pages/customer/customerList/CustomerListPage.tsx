import React, { useState, useMemo, useCallback, useEffect, useTransition } from "react";
import { useLoaderData } from "react-router";
import { api } from "@/api";
import { metadata } from "@/metadata";
import { createCustomerListModel } from "@/models";
import { useInitialRendering, useRequestHandlerQueue } from "@/hooks";

// Child components.
import CustomerListResults from "@/pages/shared/list/customerListResults";
import SearchableListPage from "@/pages/shared/list/listPage/searchableList";
import CountPanel from "./CountPanel";
import CountOverTimePanel from "./CountOverTimePanel";
import { FormField, SelectInput, type SelectInputOption } from "@/components/form";
import { ExclamationCircleIcon } from "@heroicons/react/24/outline";

// Loader
export async function loadDataAsync(model?: CustomerListModel): Promise<CustomerListModel> {
  if (model) {
    const responseDto = await api.customer.getListAsync(model.toRequestDto());
    return model.mapFromResponseDto(responseDto);
  }

  const responseDto = await api.customer.getListAsync();
  return createCustomerListModel(responseDto);
}

// Components.
export default function CustomerListPage(): React.ReactNode {
  // Dependencies.
  const initialModel = useLoaderData<CustomerListModel>();

  // States.
  const [model, setModel] = useState(() => initialModel);
  const [isReloading, startTransition] = useTransition();
  const isInitialRendering = useInitialRendering();
  const reloadAsync = useRequestHandlerQueue(async () => await loadDataAsync(model), (reloadedModel) => {
    setModel(reloadedModel);
    window.scrollTo({ top: 0, behavior: "smooth" });
  });

  // Memo.
  const paymentStatusOptions = useMemo<SelectInputOption[]>(() => {
    return [
      { value: "", displayName: "Tất cả" },
      { value: "DebtOnly", displayName: "Nợ" },
      { value: "RefundNeededOnly", displayName: "Cần hoàn tiền" },
      { value: "DebtAndRefundNeeded", displayName: "Nợ và cần hoàn tiền" }
    ] satisfies { value: CustomerListOrderPaymentStatus | ""; displayName: string }[];
  }, []);

  // Callbacks.
  const handleModelUpdated = useCallback((updatedData: Partial<CustomerListModel>) => {
    setModel(m => ({ ...m, ...updatedData }));
  }, []);

  // Effect.
  useEffect(() => {
    if (isInitialRendering) {
      return;
    }

    startTransition(reloadAsync);
  }, [
    model.sortByAscending,
    model.sortByFieldName,
    model.page,
    model.resultsPerPage,
    model.searchContent,
    model.paymentStatus
  ]);

  // Template.
  return (
    <SearchableListPage
      resourceName="customer"
      model={model}
      onModelUpdated={handleModelUpdated}
      isReloading={isReloading}
      filterPanelChildren={
        <FormField path="paymentStatus" hideLabel hideValidationMessage>
          <div className="form-input-group">
            <span className="form-input-group-text border-e-0">
              <ExclamationCircleIcon className="size-4" />
            </span>

            <SelectInput
              options={paymentStatusOptions}
              value={model.paymentStatus ?? ""}
              onValueChanged={(paymentStatus) => {
                setModel(m => ({ ...m, paymentStatus: paymentStatus as CustomerListOrderPaymentStatus ?? null }));
              }}
            />
          </div>
        </FormField>
      }
      sideBarPanels={
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-1 gap-3 h-fit">
          <CountOverTimePanel
            criteriaDisplayName="Khách hàng mới"
            getCountAsync={async (requestDto) => api.customer.getNewCountAsync(requestDto)}
          />

          <CountOverTimePanel
            criteriaDisplayName="Khách đã giao dịch"
            getCountAsync={async (requestDto) => api.customer.getPurchasedCountAsync(requestDto)}
          />
          
          <CountPanel
            title="Tổng số khách hàng"
            getCountAsync={async () => api.customer.getCountAsync()}
          />
          
          <CountPanel
            title="Số khách nợ"
            metricTextColor="yellow"
            getCountAsync={async () => api.customer.getHavingDebtAsync()}
          />
          
          <CountPanel
            title="Số khách cần hoàn tiền"
            metricTextColor="red"
            getCountAsync={async () => api.customer.getRefundNeededCountAsync()}
          />
        </div>
      }
      canCreate={metadata.creatingAuthorization.canCreateCustomer}
    >
      <CustomerListResults className="list-group-flush" model={model} />
    </SearchableListPage>
  );
}
