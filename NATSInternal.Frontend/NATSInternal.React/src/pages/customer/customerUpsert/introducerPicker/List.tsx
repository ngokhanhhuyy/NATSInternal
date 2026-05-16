import React, { useState, useEffect, useTransition } from "react";
import { api } from "@/api";
import { createCustomerListModel } from "@/models";
import { joinClassName } from "@/helpers";

// Child components.
import { Button, NewTabWebsiteLink, Paginator } from "@/components/ui";
import { TextInput } from "@/components/form";
import { CheckIcon } from "@heroicons/react/24/solid";

// Props.
type ListProps = {
  onPicked(customer: CustomerBasicModel): any;
  excludedId: number | null;
} & React.ComponentPropsWithoutRef<"div">;

// Component.
export default function List(props: ListProps): React.ReactNode {
  // Props.
  const { onPicked, excludedId, ...domProps } = props;

  // States.
  const [isInitialLoading, setIsInitialLoading] = useState(true);
  const [isLoading, startTransition] = useTransition();
  const [searchContent, setSearchContent] = useState<string>("");
  const [model, setModel] = useState<CustomerListModel>(() => {
    const model = createCustomerListModel();
    model.resultsPerPage = 8;
    model.excludedId = props.excludedId;
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
      {excludedId ?? "null"}
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
              <NewTabWebsiteLink className="text-blue-700 dark:text-blue-400 font-bold" href={customer.detailRoute}>
                {customer.fullName}
              </NewTabWebsiteLink>
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
      <Paginator
        page={model.page}
        pageCount={model.pageCount}
        onPageChanged={page => setModel(m => ({ ...m, page }))}
        getPageButtonClassName={(_, isActive) => isActive ? "btn-primary" : undefined}
      />
    </div>
  );
}
