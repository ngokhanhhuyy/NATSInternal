import React, { useState, useMemo, useEffect, useTransition } from "react";
import { useLoaderData, useNavigate } from "react-router";
import { useApi } from "@/api";
import { createCustomerListModel } from "@/models";
import { getDisplayName } from "@/metadata";
import { useTsxHelper } from "@/helpers";

// Child components.
import MainContainer from "@/components/layouts/MainContainer";
import Row from "./Row";
import Cells from "./Cells";
import { Form, FormField, TextInput, SelectInput } from "@/components/form";
import { Button, MainPaginator } from "@/components/ui";
import { FunnelIcon as FunnelOutlineIcon, MagnifyingGlassIcon } from "@heroicons/react/24/outline";
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
      className={joinClassName(isReloading && "cursor-wait")}
    >
      <div className="w-full flex gap-3 mb-3">
        <Button onClick={() => navigate(model.createRoute)}>
          <PlusIcon className="size-4 me-0.5" />
          <span>Tạo mới</span>
        </Button>

        <Button
          variant={isFilterVisible ? "primary" : undefined}
          onClick={() => setIsFilterVisible(isVisible => !isVisible)}
        >
          <FunnelOutlineIcon className="size-4 me-1" />
          <span>Bộ lọc</span>
        </Button>
      </div>
      
      <Form
        className={joinClassName(
          "w-full grid grid-cols-2 md:grid-cols-12",
          "justify-stretch items-stretch gap-3 mb-5 transition-all duration-200",
          !isFilterVisible && "hidden"
        )}
        submitAction={reloadAsync}
        showSucceededAnnouncement={false}
      >
        <div className="grid grid-cols-[1fr_auto] col-span-2 md:col-span-6 items-end">
          <FormField path="searchContent">
            <TextInput
              className="rounded-r-none z-2 not-[focus]:border-r-transparent"
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
          <Button className="justify-start" onClick={() => setModel(m => ({ ...m, sortByAscending: !m.sortByAscending }))}>
            {model.sortByAscending ? "Từ nhỏ đến lớn" : "Từ lớn đến nhỏ"}
          </Button>
        </FormField>
      </Form>

      <div className={joinClassName(
        "border border-black/10 dark:border-white/10 overscroll-x-none h-fit",
        "overflow-x-auto w-full max-w-full rounded-lg transition-opacity mb-3",
        isReloading && "opacity-50 pointer-events-none"
      )}>
        <table className="border-collapse min-w-max w-full">
          <thead className="whitespace-nowrap">
            <tr className={joinClassName(
              "bg-black/5 dark:bg-white/5 border-b border-black/10 dark:border-white/10",
              "text-black/50 dark:text-white/50 font-bold"
            )}>
              <td className="px-3 py-2">Họ và tên</td>
              <td className="px-3 py-2">Biệt danh</td>
              <td className="px-3 py-2">Giới tính</td>
              <td className="px-3 py-2">Số điện thoại</td>
              <td className="px-3 py-2">Ngày sinh</td>
              <td className="px-3 py-2">Nợ còn lại</td>
              <td/>
            </tr>
          </thead>
          <tbody>
            {model.items.length ? model.items.map((customer, index) => (
              <Row key={index}>
                <Cells model={customer} />
              </Row>
            )) : (
              <Row>
                <td className="px-3 py-2 text-center" colSpan={6}>
                  Không có kết quả
                </td>
              </Row> 
            )}
          </tbody>
        </table>
      </div>

      <MainPaginator
        page={model.page}
        pageCount={model.pageCount}
        isReloading={isReloading}
        onPageChanged={(page) => setModel(m => ({ ...m, page }))}
      />
    </MainContainer>
  );
}