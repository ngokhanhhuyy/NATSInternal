import React, { useState, useMemo, useCallback, useEffect } from "react";
import { useThrottleState } from "@/hooks";
import { useTsxHelper } from "@/helpers";
import { getDisplayName } from "@/metadata";

// Child component.
import { Button } from "@/components/ui";
import { FormField, TextInput, SelectInput, type SelectInputOption } from "@/components/form";
import { ChevronUpIcon } from "@heroicons/react/24/outline";
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
  isInitialRendering: boolean;
  children: React.ReactNode | React.ReactNode[];
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
  const { joinClassName } = useTsxHelper();

  // States.
  const [searchContent, throttledSearchContent, setSearchContent] = useThrottleState(() => props.model.searchContent);
  const [isBodyExpanded, setIsBodyExpanded] = useState(true);

  // Callbacks.
  const handleExpandButtonClicked = useCallback(() => {
    setIsBodyExpanded(isExpanded => !isExpanded);
  }, []);

  // Computed.
  const sortByFieldNameOptions = useMemo<SelectInputOption[]>(() => {
    return props.model.sortByFieldNameOptions.map((fieldName) => ({
        value: fieldName,
        displayName: getDisplayName(fieldName) ?? undefined
      }));
  }, []);

  const resultsPerPageOptions = useMemo<SelectInputOption[]>(() => {
    return [15, 20, 30, 40, 50].map(resultsPagePage => ({
      value: resultsPagePage.toString(),
      displayName: `${resultsPagePage.toString()} kết quả`
    }));
  }, []); 

  // Effect.
  useEffect(() => {
    props.onModelChanged({ searchContent: throttledSearchContent } as Partial<TListModel>);
  }, [throttledSearchContent]);

  // Template.
  return (
    <div className="panel">
      <div className={joinClassName("panel-header", !isBodyExpanded && "rounded-b-xl")}>
        <div className="panel-header-title">
          Tuỳ chọn lọc và sắp xếp
        </div>

        <Button className="btn btn-panel-header btn-sm aspect-square" onClick={handleExpandButtonClicked}>
          <ChevronUpIcon className={joinClassName(
            "size-4",
            !isBodyExpanded && "rotate-180" )}
          />
        </Button>
      </div>

      <div className={joinClassName(
        "panel-body transition-all duration-200 px-3",
        !isBodyExpanded ? "opacity-0 grid-rows-[0fr] py-0" : "opacity-100 grid-rows-[1fr] py-3"
      )}>
        <div className="overflow-hidden">
          <div className="flex flex-col gap-3">
            {/* Search content and advanced filter toggle button */}
            <FormField path="searchContent" displayName="Tìm kiếm">
              <TextInput
                placeholder="Tìm kiếm"
                value={searchContent}
                onValueChanged={(content) => setSearchContent(content)}
              />
            </FormField>

            <div className={"grid grid-cols-1 sm:grid-cols-3 lg:grid-cols-1 xl:grid-cols-3 gap-3 overflow-hidden"}>
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

            {props.children}
          </div>
        </div>
      </div>
    </div>
  );
}

export default DisplayOptionsPanel;