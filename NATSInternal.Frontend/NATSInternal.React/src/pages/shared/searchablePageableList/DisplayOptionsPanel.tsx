import React, { useMemo } from "react";
import { getDisplayName } from "@/metadata";

// Child component.
import OptionsPanel from "./OptionsPanel";
import { Button } from "@/components/ui";
import { FormField, SelectInput } from "@/components/form";
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
  isReloading: boolean;
};

// Component.
export default function DisplayOptionsPanel<
      TListModel extends
        ISearchableListModel<TItemModel> &
        ISortableListModel<TItemModel> &
        IPageableListModel<TItemModel> &
        IUpsertableListModel<TItemModel>,
      TItemModel extends object>
    (props: Props<TListModel, TItemModel>): React.ReactNode {
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
      displayName: `${resultsPagePage.toString()} kết quả`
    }));
  }, []);

  // Template.
  return (
    <OptionsPanel title="Tuỳ chọn hiển thị" isReloading={props.isReloading}>
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
    </OptionsPanel>
  );
}