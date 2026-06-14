import React, { useState, useCallback, useEffect, useTransition } from "react";
import { useLoaderData } from "react-router";
import { api } from "@/api";
import { metadata } from "@/metadata";
import { createCustomerListModel } from "@/models";
import { useInitialRendering, useRequestHandlerQueue } from "@/hooks";

// Child components.
import CustomerListResults from "@/pages/shared/list/customerListResults";
import ListPage from "@/pages/shared/list/listPage/searchableList";

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
  }, [model.sortByAscending, model.sortByFieldName, model.page, model.resultsPerPage, model.searchContent]);

  // Template.
  return (
    <ListPage
      resourceName="customer"
      model={model}
      onModelUpdated={handleModelUpdated}
      isReloading={isReloading}
      canCreate={metadata.creatingAuthorization.canCreateCustomer}
    >
      <CustomerListResults className="list-group-flush" model={model} />
    </ListPage>
  );
}
