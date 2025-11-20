import React, { useState, useMemo, useEffect } from "react";
import { useLoaderData, Link } from "react-router";
import { useApi } from "@/api";
import { createCustomerListModel } from "@/models";
import { useTsxHelper } from "@/helpers";

// Child components.
import MainContainer from "@/components/layouts/MainContainer";
import { Button } from "@/components/ui";

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
  const { joinClassName } = useTsxHelper();

  // States
  const [model, setModel] = useState(() => initialModel);
  const [isInitialRendering, setIsInitialRendering] = useState(() => true);
  const [isReloading, setIsReloading] = useState(() => false);

  // Effect.
  useEffect(() => {
    if (isInitialRendering) {
      setIsInitialRendering(false);
      return;
    }

    setIsReloading(true);
    const reloadAsync = async () => {
      const reloadedModel = await loadDataAsync(model);
      setModel(reloadedModel);
      setIsReloading(false);
    };

    reloadAsync();
  }, [model.sortByAscending, model.sortByFieldName, model.page, model.resultsPerPage, model.searchContent]);

  // Template.
  return (
    <MainContainer className={joinClassName(isReloading && "cursor-wait")}>
      <div className={joinClassName("flex flex-wrap gap-2 mb-3", isReloading && "pointer-events-none")}>
        <Paginator
          page={model.page}
          pageCount={model.pageCount}
          onPageClicked={(page) => setModel(m => ({ ...m, page }))}
        />
      </div>

      <div className="grid grid-cols-2 gap-3">
        {model.items.map((item, index) => (
          <div
            className={joinClassName(
              "border-black/10 dark:border-white/10 rounded-md p-3",
              "col-span-2 xl:col-span-1 overflow-hidden",
              index !== model.items.length - 1 && "border-b"
            )}
            key={index}
          >
            <pre>
              {JSON.stringify(item, null, 2)}
            </pre>
          </div>
        ))}
      </div>
    </MainContainer>
  );
}

type PaginatorProps = {
  page: number;
  pageCount: number;
  onPageClicked: (page: number) => any;
};

function Paginator(props: PaginatorProps): React.ReactNode {
  // Computed.
  const pageArray = useMemo(() => Array.from({ length: props.pageCount }, (_, index) => index + 1), [props.pageCount]);

  // Template.
  return pageArray.map((pageNumber) => (
    <Button
      className="px-3"
      variant={pageNumber === props.page ? "primary" : undefined}
      onClick={() => props.onPageClicked(pageNumber)}
      key={pageNumber}>
      {pageNumber}
    </Button>
  ));
}