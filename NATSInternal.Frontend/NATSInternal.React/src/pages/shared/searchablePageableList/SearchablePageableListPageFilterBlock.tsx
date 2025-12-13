import React, { useState, useMemo } from "react";
import { getDisplayName } from "@/metadata";
import { useTsxHelper } from "@/helpers";

// Child component.
import { Button, Block } from "@/components/ui";
import { FormField, TextInput, SelectInput } from "@/components/form";
import { Bars3BottomLeftIcon, MagnifyingGlassIcon } from "@heroicons/react/24/outline";
import { BarsArrowUpIcon, BarsArrowDownIcon } from "@heroicons/react/24/outline";

// Props.
type Props<TListModel extends ISearchablePagableListModel<TItemModel>, TItemModel extends object> = {
  model: TListModel;
  onModelChanged(changedData: Partial<TListModel>): any;
  onSearchButtonClicked(): any;
  isReloading: boolean;
};

// Component.
function SearchablePageableListPageFilterBlock<
      TListModel extends ISearchablePagableListModel<TItemModel>,
      TItemModel extends object>
    (props: Props<TListModel, TItemModel>): React.ReactNode {
  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // States.
  const [isAdvancedFilterCollapsed, setIsAdvancedFilterCollapsed] = useState(true);

  // Computed.
  const sortByFieldNameOptions = useMemo(() => {
    return props.model.sortByFieldNameOptions.map((fieldName) => ({
      value: fieldName,
      displayName: getDisplayName(fieldName) ?? undefined
    }));
  }, []);

  // Template.
  return (
    <Block
      title="Tìm kiếm và sắp xếp"
      bodyClassName={joinClassName(
        "flex flex-col items-stretch gap-3 p-3",
        props.isReloading && "pointer-events-none",
      )}
    >
      {/* Search content and advanced filter toggle button */}
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

        <Button
          className={joinClassName("shrink-0 gap-1", !isAdvancedFilterCollapsed && "primary")}
          onClick={() => setIsAdvancedFilterCollapsed(isVisible => !isVisible)}
        >
          <Bars3BottomLeftIcon />
          <span className="hidden md:inline">Sắp xếp</span>
        </Button>
      </div>

      {/* Collapsible advanced filter */}
      {!isAdvancedFilterCollapsed && (
        <div className={"grid grid-cols-1 sm:grid-cols-2 gap-3"}>
          <FormField path="sortByFieldName">
            <SelectInput
              options={sortByFieldNameOptions}
              value={props.model.sortByFieldName}
              onValueChanged={(sortByFieldName) => props.onModelChanged({ sortByFieldName } as Partial<TListModel>)}
            />
          </FormField>

          <FormField path="sortByAscending">
            <Button
              className="justify-start as-input gap-2"
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
        </div>
      )}
    </Block>
  );
}

export default SearchablePageableListPageFilterBlock;