import React, { useMemo } from "react";
import { compute, joinClassName } from "@/helpers";

// Child components.
import BaseListPage, { type BaseListPageProps } from "../baseListPage";
import { FormField, SelectInput, type SelectInputOption } from "@/components/form";
import { CalendarDaysIcon } from "@heroicons/react/24/outline";

// Props.
type ListModel<TItemModel extends object> = IHasStatsListModel<TItemModel> & IUpsertableListModel<TItemModel>;
export type HasStatsListPageProps<TListModel extends ListModel<TItemModel>, TItemModel extends object> =
  BaseListPageProps<TListModel, TItemModel>;

// Components.
export default function HasStatsListPage<TListModel extends ListModel<TItemModel>, TItemModel extends object>
  (props: HasStatsListPageProps<TListModel, TItemModel>): React.ReactNode
{
  // Computed.
  const statsMonthYearOptions = useMemo<SelectInputOption[]>(() => {
    const options = props.model.statsMonthYearOptions
      .map(option => ({
        value: `${option.year}-${option.month}`,
        displayName: `Tháng ${option.month} năm ${option.year}`
      }));

    return [
      { value: "", displayName: "Tất cả" },
      ...options.reverse()
    ];
  }, []);

  const selectedStatsMonthYearValue = compute<string>(() => {
    if (props.model.statsMonthYear) {
      return `${props.model.statsMonthYear.year}-${props.model.statsMonthYear.month}`;
    }

    return "";
  });

  // Callbacks.
  const handleStatsMonthYearInput = (statsMonthYearAsString: string) => {
    if (!statsMonthYearAsString) {
      props.onModelUpdated({ statsMonthYear: null } as Partial<TListModel>);
      return;
    }

    const [yearAsString, monthAsString] = statsMonthYearAsString.split("-");
    props.onModelUpdated({
      statsMonthYear: {
        year: parseInt(yearAsString),
        month: parseInt(monthAsString)
      }
    } as Partial<TListModel>);
  };
  
  // Template.
  return (
    <BaseListPage {...props} filterPanelChildren={
      <div className={joinClassName(
        "grid grid-cols-1 gap-x-2 gap-y-3",
        props.filterPanelChildren != null && "md:grid-cols-[auto_1fr] lg:grid-cols-1 xl:grid-cols-2"
      )}>
        <FormField path="statsMonthYear" displayName="Thời gian thống kê" hideLabel>
          <div className="form-input-group">
            <div className="form-input-group-text border-e-0 flex items-center shrink-0">
              <CalendarDaysIcon className="size-4.5" />
            </div>

            <SelectInput
              className="min-w-60"
              options={statsMonthYearOptions}
              value={selectedStatsMonthYearValue}
              onValueChanged={handleStatsMonthYearInput}
            />
          </div>
        </FormField>

        {props.filterPanelChildren}
      </div>
    }/>
  );
}
