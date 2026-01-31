import React, { useState, useRef, useMemo, useEffect, useTransition } from "react";
import { useLoaderData, Link } from "react-router";
import { useApi } from "@/api";
import { createProductListModel, createBrandBasicModel, createProductCategoryBasicModel } from "@/models";
import { useTsxHelper } from "@/helpers";

// Child components.
import FilterOptionsPanel from "@/pages/shared/searchablePageableList/FilterOptionsPanel";
import ResultsTablePanel from "@/pages/shared/searchablePageableList/ResultsTablePanel";
import { MainContainer } from "@/components/layouts";
import { Paginator } from "@/components/ui";
import { FormField, SelectInput, type SelectInputOption } from "@/components/form";
import { ExclamationTriangleIcon, CheckCircleIcon, PlusIcon } from "@heroicons/react/24/outline";
import { BuildingStorefrontIcon, TagIcon } from "@heroicons/react/24/outline";

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
  const { joinClassName, compute } = useTsxHelper();

  // States.
  const [model, setModel] = useState(() => initialModel.productList);
  const [isInitialRendering, setIsInitialRendering] = useState(() => true);
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
  async function reloadAsync(): Promise<void> {
    latestRequestId.current += 1;
    const currentRequestId = latestRequestId.current;
    const reloadedModel = await loadProductListAsync(model);

    if (latestRequestId.current === currentRequestId) {
      setModel(reloadedModel);
      window.scrollTo({ top: 0, behavior: "smooth" });
    }
  }
  
  // Effect.
  useEffect(() => {
    if (isInitialRendering) {
      setIsInitialRendering(false);
      return;
    }

    startTransition(reloadAsync);
  }, [
    model.searchContent,
    model.sortByAscending,
    model.sortByFieldName,
    model.page,
    model.resultsPerPage,
    model.brand,
    model.category
  ]);

  // Template.
  return (
    <MainContainer
      description="Danh sách các sản phẩm trong kho, kể cả các sản phẩm đã ngừng kinh doanh."
      className="gap-3"
    >
      <div className="flex flex-col items-stretch gap-3">
        <ResultsTablePanel
          model={model}
          isReloading={isReloading}
          renderHeaderRowChildren={() => (
            <>
              <th>Sản phẩm</th>
              <th>Tình trạng</th>
            </>
          )}
          renderBodyRowChildren={(itemModel: ProductListProductModel) => (
            <>
              <td>
                <div className="grid grid-cols-[auto_auto_1fr] items-center gap-3">
                  {itemModel.isResupplyNeeded ? (
                    <ExclamationTriangleIcon className="text-yellow-600 dark:text-yellow-400 size-6" />
                  ) : (
                    <CheckCircleIcon className="text-emerald-600 dark:text-emerald-400 size-6" />
                  )}

                  <img src={itemModel.thumbnailUrl} className="img-thumbnail size-12" alt={itemModel.name} />

                  <div className="flex flex-col self-start">
                    <div className="flex gap-3 items-center">
                      <Link to={itemModel.detailRoute} className="text-blue-700 dark:text-blue-400 font-bold">
                        {itemModel.name}
                      </Link>

                      <div className={joinClassName(
                        "alert dark:font-bold dark:alert-sm",
                        itemModel.isResupplyNeeded
                          ? "alert-warning-outline dark:alert-warning"
                          : "alert-emerald-outline dark:alert-emerald",
                      )}>
                        {itemModel.stockingQuantity}
                      </div>
                    </div>

                    <div className="text-sm">
                      {itemModel.brand && (
                        <div className="flex justify-start items-center gap-1">
                          <BuildingStorefrontIcon className="size-4" />
                          <span>{itemModel.brand.name}</span>
                        </div>
                      )}

                      {itemModel.category && (
                        <div className="flex justify-start items-center gap-1">
                          <TagIcon className="size-4" />
                          <span>{itemModel.category.name}</span>
                        </div>
                      )}
                    </div>
                  </div>
                </div>
              </td>
            </>
          )}
        />

        <div className="flex justify-end gap-3 mb-3 md:mb-5">
          <Paginator
            page={model.page}
            pageCount={model.pageCount}
            onPageChanged={(page) => setModel(m => ({ ...m, page }))}
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
          onSearchButtonClicked={reloadAsync}
          isInitialRendering={isInitialRendering}
        >
          <div className={"grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-1 xl:grid-cols-2 gap-3"}>
            <FormField path="brandId" displayName="Thương hiệu">
              <SelectInput
                disabled={isInitialRendering}
                options={brandOptions}
                value={model.brand?.id ?? ""}
                onValueChanged={(brandId) => {
                  setModel(m => {
                    const brand = brandId ? (brandOptionsModel ?? []).filter(b => b.id === brandId)[0] : null;
                    return { ...m, brand };
                  });
                }}
              />
            </FormField>

            <FormField path="categoryName" displayName="Phân loại">
              <SelectInput
                disabled={isInitialRendering}
                options={categoryOptions}
                value={model.category?.name ?? ""}
                onValueChanged={(categoryName) => {
                  setModel(m => {
                    const category = categoryName
                      ? (categoryOptionsModel ?? []).filter(c => c.name === categoryName)[0]
                      : null;

                    return { ...m, category };
                  });
                }}
              />
            </FormField>
          </div>
        </FilterOptionsPanel>
      </div>
    </MainContainer>
  );
}