import React, { useState, useRef, useEffect, useTransition } from "react";
import { Link } from "react-router";
import { useLoaderData } from "react-router";
import { useApi } from "@/api";
import { createCustomerListModel } from "@/models";

// Child components.
import ResultsPanel from "./ResultsPanel";
import FilterOptionsPanel from "@/pages/shared/searchablePageableList/FilterOptionsPanel";
import { MainContainer } from "@/components/layouts";
import { Paginator } from "@/components/ui";
import { PlusIcon } from "@heroicons/react/24/outline";

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

// Components.
export default function CustomerListPage(): React.ReactNode {
  // Dependencies.
  const initialModel = useLoaderData<CustomerListModel>();

  // States.
  const [model, setModel] = useState(() => initialModel);
  const [hasPendingReloading, setHasPendingReloading] = useState(() => false);
  const [reloadTriggeringKey, setReloadTriggerKey] = useState(() => 0);
  const [isInitialRendering, setIsInitialRendering] = useState(() => true);
  const [isReloading, startTransition] = useTransition();
  const latestRequestId = useRef<number>(0);

  // Callbacks.
  function reload(): void {
    latestRequestId.current += 1;
    startTransition(async () => {
      const currentRequestId = latestRequestId.current;
      const reloadedModel = await loadDataAsync(model);

      if (latestRequestId.current === currentRequestId) {
        setModel(reloadedModel);
        window.scrollTo({ top: 0, behavior: "smooth" });
      }
    });
  }

  // Effects.
  useEffect(() => {
    if (isInitialRendering) {
      setIsInitialRendering(false);
      return;
    }

    reload();
  }, [reloadTriggeringKey]);
  
  useEffect(() => {
    if (isInitialRendering) {
      return;
    }

    setHasPendingReloading(true);
  }, [model.sortByAscending, model.sortByFieldName, model.searchContent, model.page, model.resultsPerPage]);

  // Template.
  return (
    <MainContainer
      description="Danh sách các sản phẩm trong kho, kể cả các sản phẩm đã ngừng kinh doanh."
      className="gap-3"
    >
      <div className="flex flex-col items-stretch gap-3">
        <ResultsPanel model={model} isReloading={isReloading} />

        <div className="flex justify-end gap-3 mb-3 md:mb-5">
          <Paginator
            page={model.page}
            pageCount={model.pageCount}
            onPageChanged={(page) => {
              setModel(m => ({ ...m, page }));
              setReloadTriggerKey(key => key += 1);
            }}
            getPageButtonClassName={(_, isActive) => isActive ? "btn-primary" : undefined}
          />

          {model.pageCount > 1 && (
            <div className="border-r border-black/25 dark:border-white/25 w-px" />
          )}

          <Link className="btn gap-1 shrink-0" to={model.createRoutePath}>
            <PlusIcon className="size-4.5" />
            <span>Tạo sản phẩm mới</span>
          </Link>
        </div>

        <FilterOptionsPanel
          model={model}
          onModelChanged={changedData => setModel(m => ({ ...m, ...changedData }))}
          onReloadButtonClicked={() => {
            setModel(m => ({ ...m, page: 1 }));
            setReloadTriggerKey(key => key += 1);
          }}
          hasPendingReloading={hasPendingReloading}
        />
      </div>
    </MainContainer>
  );
}