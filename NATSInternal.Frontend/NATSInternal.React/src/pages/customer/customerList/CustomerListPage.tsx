import React, { useState, useMemo, useEffect, useTransition } from "react";
import { useLoaderData, useNavigate } from "react-router";
import { useApi } from "@/api";
import { createCustomerListModel } from "@/models";
import { getDisplayName } from "@/metadata";
import { useTsxHelper } from "@/helpers";

// Child components.
import MainContainer from "@/components/layouts/MainContainer";
import TableBlockProps from "./TableBlock";
import { Form, FormField, TextInput, SelectInput } from "@/components/form";
import { Button, MainPaginator } from "@/components/ui";
import { FunnelIcon, MagnifyingGlassIcon, BarsArrowUpIcon, BarsArrowDownIcon } from "@heroicons/react/24/outline";
import { PlusIcon } from "@heroicons/react/24/solid";

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
  const navigate = useNavigate();
  const initialModel = useLoaderData<CustomerListModel>();
  const { joinClassName } = useTsxHelper();

  // States
  const [model, setModel] = useState(() => initialModel);
  const [isInitialRendering, setIsInitialRendering] = useState(() => true);
  const [isFilterVisible, setIsFilterVisible] = useState(false);
  const [isReloading, startTransition] = useTransition();

  // Computed.
  const sortByFieldNameOptions = useMemo(() => {
    return model.sortByFieldNameOptions.map((fieldName) => ({
      value: fieldName,
      displayName: getDisplayName(fieldName) ?? undefined
    }));
  }, []);

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
      <div className="w-full flex gap-3 mb-3">
        <Button onClick={() => navigate(model.createRoute)}>
          <PlusIcon className="size-4 me-0.5" />
          <span>Tạo mới</span>
        </Button>

        <Button
          className={isFilterVisible ? "primary" : undefined}
          onClick={() => setIsFilterVisible(isVisible => !isVisible)}
        >
          <FunnelIcon className="size-4 me-1" />
          <span>Bộ lọc</span>
        </Button>
      </div>
      
      <Form
        className={joinClassName(
          "w-full grid grid-cols-2 md:grid-cols-12",
          "justify-stretch items-stretch gap-3 mb-5",
          isReloading && "pointer-events-none",
          !isFilterVisible && "hidden"
        )}
        upsertAction={reloadAsync}
        showSucceededAnnouncement={false}
      >
        <div className="grid grid-cols-[1fr_auto] col-span-2 md:col-span-6 items-end">
          <FormField path="searchContent" className="-me-px">
            <TextInput
              className="rounded-r-none z-2"
              placeholder="Tìm kiếm"
              value={model.searchContent}
              onValueChanged={(searchContent) => setModel(m => ({ ...m, searchContent }))}
            />
          </FormField>

          <Button type="submit" className="rounded-l-none">
            <MagnifyingGlassIcon className="size-4 me-1" />
          </Button>
        </div>

        <FormField className="md:col-span-3" path="sortByFieldName">
          <SelectInput
            options={sortByFieldNameOptions}
            value={model.sortByFieldName}
            onValueChanged={(sortByFieldName) => setModel(m => ({ ...m, sortByFieldName }))}
          />
        </FormField>

        <FormField className="md:col-span-3" path="sortByAscending">
          <Button
            className="justify-start"
            onClick={() => setModel(m => ({ ...m, sortByAscending: !m.sortByAscending }))}
          >
            {model.sortByAscending ? (
              <>
                <BarsArrowDownIcon className="size-4.5 me-2" />
                <span>Từ nhỏ đến lớn</span>
              </>
            ) : (
              <>
                <BarsArrowUpIcon className="size-4.5 me-2" />
                <span>Từ lớn đến nhỏ</span>
              </>
            )}
          </Button>
        </FormField>
      </Form>

      <TableBlockProps model={model} />

      <MainPaginator
        page={model.page}
        pageCount={model.pageCount}
        isReloading={isReloading}
        onPageChanged={(page) => setModel(m => ({ ...m, page }))}
      />
    </MainContainer>
  );
}