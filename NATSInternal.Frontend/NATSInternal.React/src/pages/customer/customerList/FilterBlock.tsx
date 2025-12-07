import React, { useState, useMemo } from "react";
import { getDisplayName } from "@/metadata";
import { useTsxHelper } from "@/helpers";

// Child component.
import { Block, Button, Collapsible } from "@/components/ui";
import { FormField, TextInput, SelectInput } from "@/components/form";
import { Bars3BottomLeftIcon, MagnifyingGlassIcon } from "@heroicons/react/24/outline";
import { BarsArrowUpIcon, BarsArrowDownIcon } from "@heroicons/react/24/outline";

// Props.
type FilterBlockProps = {
  model: CustomerListModel;
  onModelChanged(changedData: Partial<CustomerListModel>): any;
  onSearchButtonClicked(): any;
  isReloading: boolean;
};

// Component.
export default function FilterBlock(props: FilterBlockProps): React.ReactNode {
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
        "flex flex-col items-stretch gap-3 pt-3 px-3",
        props.isReloading && "pointer-events-none",
      )}
    >
      {/* Search content and advanced filter toggle button */}
      <div className="grid grid-cols-[1fr_auto_auto] gap-3">
        <TextInput
          placeholder="Tìm kiếm"
          value={props.model.searchContent}
          onValueChanged={(searchContent) => props.onModelChanged({ searchContent })}
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
      <Collapsible isCollapsed={isAdvancedFilterCollapsed}>
        <div className="grid grid-cols-1 sm:grid-cols-2 gap-3 mb-3">
          <FormField path="sortByFieldName">
            <SelectInput
              options={sortByFieldNameOptions}
              value={props.model.sortByFieldName}
              onValueChanged={(sortByFieldName) => props.onModelChanged({ sortByFieldName })}
            />
          </FormField>

          <FormField path="sortByAscending">
            <Button
              className="justify-start as-input gap-2"
              onClick={() => props.onModelChanged({ sortByAscending: !props.model.sortByAscending })}
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
      </Collapsible>
    </Block>
  );
}