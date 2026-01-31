import React, { useState, useMemo, useEffect, startTransition } from "react";
import { useApi } from "@/api";
import { getDisplayName } from "@/metadata";
import { createBrandBasicModel, createProductCategoryBasicModel } from "@/models";

// Child component.
import OptionsPanel from "./OptionsPanel";
import { Button } from "@/components/ui";
import { FormField, TextInput, SelectInput } from "@/components/form";
import { MagnifyingGlassIcon, BarsArrowUpIcon, BarsArrowDownIcon } from "@heroicons/react/24/outline";

// Props.
type Props<
    TListModel extends
      ISearchableListModel<TItemModel> &
      ISortableListModel<TItemModel> &
      IPageableListModel<TItemModel> &
      IUpsertableListModel<TItemModel>,
    TItemModel extends object> = {
  model: TListModel;
  onModelChanged(changedData: Partial<TListModel>): any;
  onSearchButtonClicked(): any;
  isReloading: boolean;
};

// Component.
function DisplayOptionsPanel<
      TListModel extends
        ISearchableListModel<TItemModel> &
        ISortableListModel<TItemModel> &
        IPageableListModel<TItemModel> &
        IUpsertableListModel<TItemModel>,
      TItemModel extends object>
    (props: Props<TListModel, TItemModel>): React.ReactNode {
  // Dependencies.
  const api = useApi();

  // States.
  const [brandOptionsModel, setBrandOptionsModel] = useState<BrandBasicModel[] | null>(null);
  const [categoryOptionsModel, setCategoryOptionsModel] = useState<ProductCategoryBasicModel[] | null>(null);

  // Computed.
  const brandOptions = useMemo(() => {
    return brandOptionsModel?.map((brand) => ({
        value: brand.id,
        displayName: brand.name
      }));
  }, [brandOptionsModel]);

  const categoryOptions = useMemo(() => {
    return categoryOptionsModel?.map((category) => ({
        value: category.name,
        displayName: category.name
      }));
  }, [categoryOptionsModel]);

  // Effect.
  useEffect(() => {
    startTransition(async () => {
      const [brandOptionResponseDtos, categoryOptionResponseDtos] = await Promise.all([
        api.brand.getAllAsync(),
        api.productCategory.getAllAsync()
      ]);

      setBrandOptionsModel(brandOptionResponseDtos.map((dto) => createBrandBasicModel(dto)));
      setCategoryOptionsModel(categoryOptionResponseDtos.map((dto) => createProductCategoryBasicModel(dto)));
    });
  }, []);

  // Template.
  return (
    <OptionsPanel title="Tuỳ chọn lọc" isReloading={props.isReloading}>
      <div className="flex flex-col gap-3">
        {/* Search content and advanced filter toggle button */}
        <FormField path="searchContent" displayName="Tìm kiếm">
          <div className="flex gap-3">
            <TextInput
              placeholder="Tìm kiếm"
              value={props.model.searchContent}
              onValueChanged={(searchContent) => props.onModelChanged({ searchContent } as Partial<TListModel>)}
            />

            <Button className="shrink-0 gap-1" onClick={props.onSearchButtonClicked}>
              <MagnifyingGlassIcon />
              <span className="hidden md:inline">Tìm kiếm</span>
            </Button>
          </div>
        </FormField>
        
        <div className={"grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-1 xl:grid-cols-2 gap-3"}>
          <FormField path="brandId">
            <SelectInput
              options={brandOptions}
              value={props.model.brand.name}
              onValueChanged={(sortByFieldName) => props.onModelChanged({ sortByFieldName } as Partial<TListModel>)}
            />
          </FormField>

          <FormField path="sortByAscending">
            <Button
              className="form-control justify-start gap-2"
              onClick={() => {
                props.onModelChanged({ sortByAscending: !props.model.sortByAscending } as Partial<TListModel>);
              }}
            >
              {props.model.sortByAscending ? (
                <>
                  <BarsArrowDownIcon />
                  <span>Từ nhỏ đến lớn</span>
                </>
              ) : (
                <>
                  <BarsArrowUpIcon />
                  <span>Từ lớn đến nhỏ</span>
                </>
              )}
            </Button>
          </FormField>

          <FormField path="resultsPerPage">
            <SelectInput
              options={resultsPerPageOptions}
              value={props.model.resultsPerPage.toString()}
              onValueChanged={resultsPerPageAsString => {
                props.onModelChanged({ resultsPerPage: parseInt(resultsPerPageAsString) } as Partial<TListModel>);
              }}
            />
          </FormField>
        </div>
      </div>
    </OptionsPanel>
  );
}

export default DisplayOptionsPanel;