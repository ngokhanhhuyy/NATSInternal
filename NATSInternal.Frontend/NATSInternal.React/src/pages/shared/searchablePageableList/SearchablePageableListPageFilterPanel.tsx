import React, { useMemo } from "react";
import { getDisplayName } from "@/metadata";
import { useTsxHelper } from "@/helpers";

// Child component.
import { Button } from "@/components/ui";
import { FormField, TextInput, SelectInput } from "@/components/form";
import { Disclosure, DisclosureButton, DisclosurePanel } from "@headlessui/react";
import { ChevronUpIcon, MagnifyingGlassIcon } from "@heroicons/react/24/outline";
import { BarsArrowUpIcon, BarsArrowDownIcon } from "@heroicons/react/24/outline";

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
function SearchablePageableListPageFilterBlock<
      TListModel extends
        ISearchableListModel<TItemModel> &
        ISortableListModel<TItemModel> &
        IPageableListModel<TItemModel> &
        IUpsertableListModel<TItemModel>,
      TItemModel extends object>
    (props: Props<TListModel, TItemModel>): React.ReactNode {
  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // Computed.
  const sortByFieldNameOptions = useMemo(() => {
    return props.model.sortByFieldNameOptions.map((fieldName) => ({
      value: fieldName,
      displayName: getDisplayName(fieldName) ?? undefined
    }));
  }, []);

  const resultsPerPageOptions = useMemo(() => {
    return [15, 20, 30, 40, 50].map(resultsPagePage => ({
      value: resultsPagePage.toString(),
      displayName: resultsPagePage.toString()
    }));
  }, []);

  // Template.
  return (
    <Disclosure as="div" className="panel" defaultOpen={true} data-open>
      {({ open }) => (
        <>
          <div className={joinClassName("panel-header", !open && "rounded-b-xl")}>
            <div className="panel-header-title">
              Chế độ hiển thị
            </div>

            <DisclosureButton className="btn btn-panel-header btn-sm aspect-square">
              <ChevronUpIcon className={joinClassName(
                "size-4.5 transition-transform",
                !open && "rotate-180" )}
              />
            </DisclosureButton>
          </div>

          <DisclosurePanel
            as="div"
            transition
            className={joinClassName(
              "panel-body flex flex-col items-stretch gap-3 p-3",
              "origin-top transition duration-200 ease-out data-closed:scale-y-0 data-closed:opacity-0",
              props.isReloading && "pointer-events-none",
              !open && "hidden"
            )}
          >
            {/* Search content and advanced filter toggle button */}
            <FormField path="searchByContent" displayName="Tìm kiếm">
              <div className="grid grid-cols-[1fr_auto_auto] gap-3">
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
            
            <div className={"grid grid-cols-1 sm:grid-cols-3 gap-3"}>
              <FormField path="sortByFieldName">
                <SelectInput
                  options={sortByFieldNameOptions}
                  value={props.model.sortByFieldName}
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
          </DisclosurePanel>
        </>
      )}
    </Disclosure>
  );
}

export default SearchablePageableListPageFilterBlock;