import React, { useState, useEffect, useTransition } from "react";
import { useApi } from "@/api";
import { createCustomerListModel } from "@/models";
import { useTsxHelper } from "@/helpers";

// Child components.
import { Button, NewTabLink, MainPaginator } from "@/components/ui";
import { TextInput } from "@/components/form";
import { CheckIcon } from "@heroicons/react/24/solid";

// Props.
type ListProps = {
  onPicked(customer: CustomerListCustomerModel): any;
} & React.ComponentPropsWithoutRef<"div">;

// Component.
export default function List(props: ListProps): React.ReactNode {
  // Props.
  const { onPicked, ...domProps } = props;

  // Dependencies.
  const api = useApi();
  const { joinClassName } = useTsxHelper();

  // States.
  const [isInitialLoading, setIsInitialLoading] = useState(true);
  const [isLoading, startTransition] = useTransition();
  const [searchContent, setSearchContent] = useState<string>("");
  const [model, setModel] = useState<CustomerListModel>(() => {
    const model = createCustomerListModel();
    model.resultsPerPage = 8;
    return model;
  });

  // Effect.
  useEffect(() => {
    const loadAsync = async () => {
      const responseDto = await api.customer.getListAsync(model.toRequestDto());
      setModel(m => m!.mapFromResponseDto(responseDto));

      if (isInitialLoading) {
        setIsInitialLoading(false);
      }
    };

    startTransition(loadAsync);
  }, [model.searchContent, model.page]);

  // Template.
  if (isInitialLoading) {
    return (
      <div className="flex justify-center items-center opacity-50 cursor-wait py-5">
        Đang tải dữ liệu...
      </div>
    );
  }

  return (
    <div
      {...domProps}
      className={joinClassName(
        "flex flex-col justify-start items-stretch transition-opacity gap-3 p-3",
        isLoading && "opacity-50 cursor-wait"
      )}
    >
      {/* Search */}
      <div className="flex justify-between gap-3">
        <TextInput
          className="justify-self-stretch w-full"
          placeholder="Tìm kiếm"
          value={searchContent}
          onValueChanged={(searchContent) => setSearchContent(searchContent)}
        />
        <Button className="shrink-0 grow-0" onClick={() => setModel(m => ({ ...m, searchContent }))}>
          Tìm kiếm
        </Button>
      </div>

      {/* List */}
      <ul className={joinClassName("list-group", isLoading && "pointer-events-none")}>
        {model.items.length ? model.items.map((customer, index) => (
          <li className="list-group-item flex justify-between items-center px-3 py-2" key={index}>
            <div className="flex flex-col">
              <NewTabLink className="font-bold" href={customer.detailRoute}>
                {customer.fullName}
              </NewTabLink>
              <span className="text-sm opacity-75">
                {customer.nickName}
              </span>
            </div>
            <Button className="btn-sm aspect-square" onClick={() => onPicked(customer)}>
              <CheckIcon className="size-4.5" />
            </Button>
          </li>
        )) : (
          <li className="list-group-item py-5">
            <span className="opacity-50">Không có kết quả</span>
          </li>
        )}
      </ul>

      {/* Paginator */}
      <MainPaginator
        page={model.page}
        pageCount={model.pageCount}
        onPageChanged={page => setModel(m => ({ ...m, page }))}
        isReloading={isLoading}
      />
    </div>
  );
}