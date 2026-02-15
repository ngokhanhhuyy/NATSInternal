import React, { useState, useRef, useMemo, useTransition } from "react";
import { useLoaderData, Link } from "react-router";
import { useApi } from "@/api";
import { createProductListModel, createBrandBasicModel, createProductCategoryBasicModel } from "@/models";
import { useInitialRendering, useWatcher } from "@/hooks";
import { useTsxHelper } from "@/helpers";

// Child components.
import ResultsPanel from "./ResultsPanel";
import FilterOptionsPanel from "@/pages/shared/searchablePageableList/FilterOptionsPanel";
import { MainContainer } from "@/components/layouts";
import { Paginator } from "@/components/ui";
import { FormField, SelectInput, type SelectInputOption } from "@/components/form";
import { PlusIcon } from "@heroicons/react/24/outline";

// Data loader.
type DataLoaderResults = {
  productList: ProductListModel;
  brandOptions: BrandBasicModel[];
  categoryOptions: ProductCategoryBasicModel[];
};

export async function loadDataAsync(): Promise<DataLoaderResults> {
  const api = useApi();
  
  const [productList, brandOptions, categoryOptions] = await Promise.all([
    loadProductListAsync(),
    api.brand.getAllAsync().then(dtos => dtos.map(dto => createBrandBasicModel(dto))),
    api.productCategory.getAllAsync().then(dtos => dtos.map(dto => createProductCategoryBasicModel(dto)))
  ]);

  return { productList, brandOptions, categoryOptions };
}

export async function loadProductListAsync(model?: ProductListModel): Promise<ProductListModel> {
  const api = useApi();
  if (model) {
    const responseDto = await api.product.getListAsync(model.toRequestDto());
    return model.mapFromResponseDto(responseDto);
  }

  model = createProductListModel();
  const responseDto = await api.product.getListAsync(model.toRequestDto());
  return model.mapFromResponseDto(responseDto);
}

// Components.
export default function ProductListPage(): React.ReactNode {
  // Dependencies.
  const initialModel = useLoaderData<DataLoaderResults>();
  const { compute } = useTsxHelper();

  // States.
  const [model, setModel] = useState(() => initialModel.productList);
  const [hasPendingReloading, setHasPendingReloading] = useState(() => false);
  const [reloadTriggeringKey, setReloadTriggerKey] = useState(() => 0);
  const [isReloading, startTransition] = useTransition();
  const latestRequestId = useRef<number>(0);

  // Computed.
  const brandOptionsModel = compute(() => initialModel.brandOptions);
  const categoryOptionsModel = compute(() => initialModel.categoryOptions);

  const brandOptions = useMemo<SelectInputOption[]>(() => {
    return [
      { value: "", displayName: "Chưa chọn thương hiệu" },
      ...(initialModel.brandOptions.map((brand) => ({ value: brand.id, displayName: brand.name })) ?? [])
    ];
  }, []);

  const categoryOptions = useMemo<SelectInputOption[]>(() => {
    return [
      { value: "", displayName: "Chưa chọn phân loại" },
      ...(initialModel.categoryOptions?.map((category) => ({ value: category.name, displayName: category.name })) ?? [])
    ];
  }, []);

  // Callbacks.
  function reload(): void {
    latestRequestId.current += 1;
    startTransition(async () => {
      const currentRequestId = latestRequestId.current;
      const reloadedModel = await loadProductListAsync(model);
      setHasPendingReloading(false);

      if (latestRequestId.current === currentRequestId) {
        setModel(reloadedModel);
        window.scrollTo({ top: 0, behavior: "smooth" });
      }
    });
  }

  // Life cycle hooks.
  useWatcher(() => {
    reload();
  }, [reloadTriggeringKey]);

  useWatcher(() => {
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
        >
          <div className={"grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-1 xl:grid-cols-2 gap-3"}>
            <FormField path="brandId" displayName="Thương hiệu">
              <SelectInput
                disabled={isInitialRendering}
                options={brandOptions}
                value={model.brand?.id ?? ""}
                onValueChanged={(brandId) => {
                  setModel(m => ({ ...m, brand: brandOptionsModel.find(b => b.id == brandId) ?? null }));
                }}
              />
            </FormField>

            <FormField path="categoryName" displayName="Phân loại">
              <SelectInput
                disabled={isInitialRendering}
                options={categoryOptions}
                value={model.category?.name ?? ""}
                onValueChanged={(categoryName) => {
                  setModel(m => ({ ...m, category: categoryOptionsModel.find(c => c.name === categoryName) ?? null }));
                }}
              />
            </FormField>
          </div>
        </FilterOptionsPanel>
      </div>
    </MainContainer>
  );
}