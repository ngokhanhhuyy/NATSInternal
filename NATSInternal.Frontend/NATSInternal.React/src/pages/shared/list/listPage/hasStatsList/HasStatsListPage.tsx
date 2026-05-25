import React, { useMemo } from "react";
import { compute } from "@/helpers";

// Child components.
import BaseListPage, { type BaseListPageProps } from "../baseListPage";
import { FormField, SelectInput, type SelectInputOption } from "@/components/form";

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
      <FormField path="statsMonthYear" displayName="Thời gian thống kê">
        <SelectInput
          options={statsMonthYearOptions}
          value={selectedStatsMonthYearValue}
          onValueChanged={handleStatsMonthYearInput}
        />
      </FormField>
    }/>
  );
}
