import React, { useMemo } from "react";
import { flushSync } from "react-dom";
import { getDisplayName } from "@/metadata";
import { useDirtyModelChecker } from "@/hooks";
import { useTsxHelper } from "@/helpers";

// Child component.
import { Button } from "@/components/ui";
import { FormField, TextInput, SelectInput, type SelectInputOption } from "@/components/form";
import { ArrowPathIcon } from "@heroicons/react/24/outline";
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
  onModelUpdated(updatedData: Partial<TListModel>): any;
  onReloadButtonClicked(): any;
  additionalDirtyModelComparer?: (originalModel: TListModel, currentModel: TListModel) => boolean;
  children?: React.ReactNode | React.ReactNode[];
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
  const [isModelDirty, setModelDirtyCheckerOriginalModel] = useDirtyModelChecker(props.model, (om, m) => {
    return (
      m.sortByAscending === om.sortByAscending &&
      m.sortByFieldName === om.sortByFieldName &&
      m.searchContent === om.searchContent &&
      m.page === om.page &&
      m.resultsPerPage === om.resultsPerPage
    );
  });

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

      <div className="panel-body p-3">
        <div className="flex flex-col gap-3">
          {/* Search content and advanced filter toggle button */}
          <FormField path="searchContent" displayName="Tìm kiếm">
            <TextInput
              placeholder="Tìm kiếm"
              autoComplete="off"
              value={props.model.searchContent}
              onValueChanged={(searchContent) => props.onModelUpdated({ searchContent } as Partial<TListModel>)}
            />
          </FormField>

          <div className={"grid grid-cols-1 sm:grid-cols-3 lg:grid-cols-1 xl:grid-cols-3 gap-3"}>
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

          {props.children}

          <div className="flex justify-end">
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

export default DisplayOptionsPanel;