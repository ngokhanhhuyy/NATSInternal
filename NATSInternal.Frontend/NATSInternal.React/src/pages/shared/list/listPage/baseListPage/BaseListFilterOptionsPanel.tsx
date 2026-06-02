import React, { useMemo } from "react";
import { flushSync } from "react-dom";
import { getDisplayName } from "@/metadata";
import { useJSONDirtyModelChecker } from "@/hooks";
import { joinClassName } from "@/helpers";

// Child component.
import { Button } from "@/components/ui";
import { FormField, SelectInput, type SelectInputOption } from "@/components/form";
import { ArrowPathIcon } from "@heroicons/react/24/outline";
import { BarsArrowUpIcon, BarsArrowDownIcon } from "@heroicons/react/24/outline";

// Props.
type BaseListFilterOptionsPanelProps<
    TListModel extends IListModel<TItemModel> & IUpsertableListModel<TItemModel>,
    TItemModel extends object> = {
  model: TListModel;
  onModelUpdated(updatedData: Partial<TListModel>): any;
  onReloadButtonClicked(): any;
  children?: React.ReactNode | React.ReactNode[];
};

// Component.
function BaseListFilterOptionsPanel<
      TListModel extends IListModel<TItemModel> & IUpsertableListModel<TItemModel>,
      TItemModel extends object>
    (props: BaseListFilterOptionsPanelProps<TListModel, TItemModel>): React.ReactNode {
  // States.
  const [isModelDirty, setModelDirtyCheckerOriginalModel] = useJSONDirtyModelChecker(
    () => props.model.toRequestDto(),
    ["page"]
  );

  // Computed.
  const sortByFieldNameOptions = useMemo<SelectInputOption[]>(() => {
    return props.model.sortByFieldNameOptions.map((fieldName) => ({
        value: fieldName,
        displayName: getDisplayName(fieldName) ?? undefined
      }));
  }, []);

  const resultsPerPageOptions = useMemo<SelectInputOption[]>(() => {
    return [10, 15, 20, 30, 40, 50].map(resultsPagePage => ({
      value: resultsPagePage.toString(),
      displayName: `${resultsPagePage.toString()} kết quả`
    }));
  }, []);

  // Callbacks.
  const handleReloadButtonClicked = () => {
    props.onReloadButtonClicked();
    flushSync(() => setModelDirtyCheckerOriginalModel(props.model));
  };

  // Template.
  return (
    <div className="panel mt-3 md:mt-5">
      <div className="panel-header">
        <div className="panel-header-title">
          Tuỳ chọn lọc và sắp xếp
        </div>
      </div>

      <div className="panel-body p-3 pt-2">
        <div className="flex flex-col gap-3">
          {props.children}
          
          <div className={"grid grid-cols-1 sm:grid-cols-3 gap-3"}>
            <FormField path="sortByFieldName">
              <SelectInput
                options={sortByFieldNameOptions}
                value={props.model.sortByFieldName}
                onValueChanged={(sortByFieldName) => props.onModelUpdated({ sortByFieldName } as Partial<TListModel>)}
              />
            </FormField>

            <FormField path="sortByAscending">
              <Button
                className="form-control justify-start gap-2"
                onClick={() => {
                  props.onModelUpdated({ sortByAscending: !props.model.sortByAscending } as Partial<TListModel>);
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
                  props.onModelUpdated({ resultsPerPage: parseInt(resultsPerPageAsString) } as Partial<TListModel>);
                }}
              />
            </FormField>
          </div>

          <div className="flex justify-end mt-4">
            <Button 
              className={joinClassName("gap-1", isModelDirty && "btn-primary")}
              onClick={handleReloadButtonClicked}
            >
              <ArrowPathIcon />
              <span>Tải lại kết quả</span>
            </Button>
          </div>
        </div>
      </div>
    </div>
  );
}

export default BaseListFilterOptionsPanel;
