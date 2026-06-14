import React, { useState, useMemo } from "react";
import { getDisplayName } from "@/metadata";
import { joinClassName } from "@/helpers";

// Child component.
import { Button } from "@/components/ui";
import { SelectInput, type SelectInputOption } from "@/components/form";
import { BarsArrowUpIcon, BarsArrowDownIcon, Bars3BottomRightIcon } from "@heroicons/react/24/outline";
import { ArrowUpIcon, ArrowDownIcon } from "@heroicons/react/24/outline";

// Props.
type BaseListFilterOptionsAreaProps<
    TListModel extends IListModel<TItemModel> & IUpsertableListModel<TItemModel>,
    TItemModel extends object> = {
  model: TListModel;
  onModelUpdated(updatedData: Partial<TListModel>): any;
  children?: React.ReactNode | React.ReactNode[];
  displayName: string | null;
};

// Component.
function BaseListFilterOptionsArea<
      TListModel extends IListModel<TItemModel> & IUpsertableListModel<TItemModel>,
      TItemModel extends object>
    (props: BaseListFilterOptionsAreaProps<TListModel, TItemModel>): React.ReactNode {
  // States.
  const [isAdvancedFiltersVisible, setIsAdvancedFiltersVisible] = useState<boolean>(false);

  // Computed.
  const sortByFieldNameOptions = useMemo<SelectInputOption[]>(() => {
    return props.model.sortByFieldNameOptions.map((fieldName) => ({
      value: fieldName,
      displayName: getDisplayName(fieldName) ?? fieldName
    }));
  }, []);

  // Template.
  return (
    <div className="grid grid-cols-1 xl:grid-cols-[1fr_1.5fr] gap-3 justify-stretch items-start w-full">
      <div className="grid grid-cols-[1fr_auto] md:grid-cols-1 gap-3">
        {props.children}

        <button
          type="button"
          className={joinClassName(
            "btn gap-1 md:hidden aspect-square sm:aspect-auto",
            isAdvancedFiltersVisible && "btn-primary"
          )}
          onClick={() => setIsAdvancedFiltersVisible(isVisible => !isVisible)}
        >
          <Bars3BottomRightIcon className="size-4" />
          <span className="hidden sm:inline">Sắp xếp</span>
        </button>
      </div>

      <div className={joinClassName(
        "grid-cols-1 sm:grid-cols-[1.5fr_1fr] md:grid-cols-2 gap-3 flex-1 min-w-fit",
        !isAdvancedFiltersVisible ? "hidden md:grid" : "grid"
      )}>
        <div className="form-input-group">
          <div className="form-input-group-text gap-1.5 border-e-transparent">
            <Bars3BottomRightIcon className="size-4" />
          </div>

          <SelectInput
            className="shrink-0 min-w-40 flex-1"
            options={sortByFieldNameOptions}
            value={props.model.sortByFieldName}
            onValueChanged={(sortByFieldName) => props.onModelUpdated({ sortByFieldName } as Partial<TListModel>)}
          />
        </div>

        <div className="form-input-group">
          <div className="form-input-group-text gap-1.5 border-e-transparent shrink-0">
            {props.model.sortByAscending ? (
              <BarsArrowDownIcon className="size-4" />
            ) : (
              <BarsArrowUpIcon className="size-4" />
            )}
          </div>

          <Button
            className="form-control justify-between gap-2 min-w-fit pe-2.5"
            onClick={() => {
              props.onModelUpdated({ sortByAscending: !props.model.sortByAscending } as Partial<TListModel>);
            }}
          >
            {props.model.sortByAscending ? (
              <>
                <span>Từ nhỏ đến lớn</span>
                <ArrowUpIcon className="size-3.5" />
              </>
            ) : (
              <>
                <span>Từ lớn đến nhỏ</span>
                <ArrowDownIcon className="size-3.5" />
              </>
            )}
          </Button>
        </div>
      </div>
    </div>
  );
}

export default BaseListFilterOptionsArea;
