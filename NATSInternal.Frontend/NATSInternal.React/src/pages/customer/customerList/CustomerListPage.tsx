import React, { useState, useEffect, useTransition } from "react";
import { useLoaderData } from "react-router";
import { useApi } from "@/api";
import { createCustomerListModel } from "@/models";

// Child components.
import MainContainer from "@/components/layouts/MainContainer";
import FilterBlock from "./FilterBlock";
import TableBlockProps from "./TableBlock";
import { MainPaginator } from "@/components/ui";

// Api.
const api = useApi();

// Loader
export async function loadDataAsync(model?: CustomerListModel): Promise<CustomerListModel> {
  if (model) {
    const responseDto = await api.customer.getListAsync(model.toRequestDto());
    return model.mapFromResponseDto(responseDto);
  }

  const responseDto = await api.customer.getListAsync();
  return createCustomerListModel(responseDto);
}

// Component.
export default function CustomerListPage(): React.ReactNode {
  // Dependencies.
  const initialModel = useLoaderData<CustomerListModel>();

  // States
  const [model, setModel] = useState(() => initialModel);
  const [isInitialRendering, setIsInitialRendering] = useState(() => true);
  const [isReloading, startTransition] = useTransition();

  // Callbacks.
  async function reloadAsync(): Promise<void> {
    startTransition(async () => {
      const reloadedModel = await loadDataAsync(model);
      setModel(reloadedModel);
    });

    await Promise.resolve();
  }

  // Effect.
  useEffect(() => {
    if (isInitialRendering) {
      setIsInitialRendering(false);
      return;
    }

    reloadAsync();
  }, [model.sortByAscending, model.sortByFieldName, model.page, model.resultsPerPage]);

  // Template.
  return (
    <MainContainer
      description="Danh sách các khách hàng đã và đang giao dịch với cửa hàng."
      isLoading={isReloading}
    >
      <div className="flex flex-col items-stretch gap-3">
        <FilterBlock
          model={model}
          onModelChanged={changedData => setModel(m => ({ ...m, ...changedData }))}
          onSearchButtonClicked={reloadAsync}
          isReloading={isReloading}
        />

        <TableBlockProps model={model} />

        <MainPaginator
          page={model.page}
          pageCount={model.pageCount}
          isReloading={isReloading}
          onPageChanged={(page) => setModel(m => ({ ...m, page }))}
        />
      </div>
    </MainContainer>
  );
}