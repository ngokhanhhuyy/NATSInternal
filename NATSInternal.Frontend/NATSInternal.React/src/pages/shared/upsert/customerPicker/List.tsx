import React, { useState, useEffect, useTransition } from "react";
import { api } from "@/api";
import { createCustomerListModel } from "@/models";
import { joinClassName } from "@/helpers";

// Child components.
import { Button, Paginator } from "@/components/ui";
import { TextInput } from "@/components/form";
import CustomerList from "@/pages/shared/list/customerListResults";
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
      <CustomerList
        model={model}
        isReloading={isLoading}
        renderItemChildren={(customer) => (
          <Button className="btn-sm aspect-square me-1 self-center" onClick={() => onPicked(customer)}>
            <CheckIcon className="size-4.5" />
          </Button>
        )}
        openLinkInNewTab
      />

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
