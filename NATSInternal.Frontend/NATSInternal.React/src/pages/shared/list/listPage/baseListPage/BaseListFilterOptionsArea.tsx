import React, { useState, useMemo } from "react";
import { Link } from "react-router";
import { getDisplayName } from "@/metadata";
import { joinClassName } from "@/helpers";

// Child component.
import { Button } from "@/components/ui";
import { SelectInput, type SelectInputOption } from "@/components/form";
import { BarsArrowUpIcon, BarsArrowDownIcon, Bars3BottomRightIcon } from "@heroicons/react/24/outline";
import { ArrowUpIcon, ArrowDownIcon, PlusIcon } from "@heroicons/react/24/outline";

// Props.
type BaseListFilterOptionsAreaProps<
    TListModel extends IListModel<TItemModel> & IUpsertableListModel<TItemModel>,
    TItemModel extends object> = {
  model: TListModel;
  onModelUpdated(updatedData: Partial<TListModel>): any;
  children?: React.ReactNode | React.ReactNode[];
  displayName: string | null;
  canCreate: boolean;
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
    <div className="grid grid-cols-1 justify-stretch items-start w-full gap-x-3 transition-[gap]">
      <div className="grid grid-cols-[1fr_auto] gap-2">
        {props.children}

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-1 xl:grid-cols-2 gap-x-2 gap-y-3">
          {(props.canCreate && props.model.createRoutePath) && (
            <Link to={props.model.createRoutePath} className="btn aspect-square">
              <PlusIcon className="size-4" />
            </Link>
          )}

          <button
            type="button"
            className={joinClassName(
              "btn gap-1 aspect-square",
              isAdvancedFiltersVisible && "btn-primary"
            )}
            onClick={() => setIsAdvancedFiltersVisible(isVisible => !isVisible)}
          >
            <Bars3BottomRightIcon className="size-4" />
          </button>
        </div>
      </div>

      <div className={joinClassName(
        "grid grid-cols-1 sm:grid-cols-[1.5fr_1fr] md:grid-cols-2 gap-x-2 gap-y-3 min-w-fit",
        isAdvancedFiltersVisible ? "h-fit mt-3 translate-x-0" : "h-0 translate-x-[15%] opacity-0"
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
