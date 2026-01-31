import React, { useState, useRef, useMemo, useCallback } from "react";
import { getDisplayName } from "@/metadata";
import { useTsxHelper } from "@/helpers";

// Child component.
import { Button } from "@/components/ui";
import { FormField, TextInput, SelectInput } from "@/components/form";
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

  // States.
  const [isBodyExpanded, setIsBodyExpanded] = useState(true);
  const elementRef = useRef<HTMLDivElement | null>(null);

  // Computed.
  const sortByFieldNameOptions = useMemo(() => {
    return props.model.sortByFieldNameOptions.map((fieldName) => {
      let displayName = getDisplayName(fieldName) ?? undefined;
      if (displayName != null) {
         displayName = `Sắp xếp theo ${displayName.toLowerCase()}`;
      }

      return {
        value: fieldName,
        displayName
      };
    });
  }, []);

  const resultsPerPageOptions = useMemo(() => {
    return [15, 20, 30, 40, 50].map(resultsPagePage => ({
      value: resultsPagePage.toString(),
      displayName: `Hiển thị ${resultsPagePage.toString()} kết quả mỗi trang`
    }));
  }, []);

  // Callbacks.
  const handleExpandButtonClicked = useCallback(() => {
    setIsBodyExpanded(isExpanded => !isExpanded);
  }, []);

  // Template.
  return (
    <div className="panel" ref={elementRef}>
      <div className={joinClassName("panel-header")}>
        <div className="panel-header-title">
          Chế độ hiển thị
        </div>

        <Button className="btn btn-panel-header btn-sm aspect-square" onClick={handleExpandButtonClicked}>
          <ChevronUpIcon className={joinClassName(
            "size-4.5",
            !isBodyExpanded && "rotate-180" )}
          />
        </Button>
      </div>

      <div className={joinClassName(
        "panel-body flex flex-col items-stretch gap-3 p-3",
        props.isReloading && "pointer-events-none",
        !isBodyExpanded && "hidden"
      )}>
        {/* Search content and advanced filter toggle button */}
        <FormField>
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
        
        <div className={"grid grid-cols-1 sm:grid-cols-3 gap-3"}>
          <FormField>
            <SelectInput
              options={sortByFieldNameOptions}
              value={props.model.sortByFieldName}
              onValueChanged={(sortByFieldName) => props.onModelChanged({ sortByFieldName } as Partial<TListModel>)}
            />
          </FormField>

          <FormField>
            <Button
              className="form-control justify-start gap-2"
              onClick={() => {
                props.onModelChanged({ sortByAscending: !props.model.sortByAscending } as Partial<TListModel>);
              }}
            >
              {props.model.sortByAscending ? (
                <>
                  <BarsArrowDownIcon />
                  <span>Sắp xếp từ nhỏ đến lớn</span>
                </>
              ) : (
                <>
                  <BarsArrowUpIcon />
                  <span>Sắp xếp từ lớn đến nhỏ</span>
                </>
              )}
            </Button>
          </FormField>

          <FormField>
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
    </div>
  );
}

export default SearchablePageableListPageFilterBlock;