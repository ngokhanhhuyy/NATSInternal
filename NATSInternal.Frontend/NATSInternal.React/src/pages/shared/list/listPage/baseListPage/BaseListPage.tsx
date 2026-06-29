import React, { useMemo } from "react";
import { Link } from "react-router";
import { getDisplayName } from "@/metadata";
import { joinClassName } from "@/helpers";

// Child components.
import FilterOptionsArea from "./BaseListFilterOptionsArea";
import { MainContainer } from "@/components/layouts";
import { SelectInput, type SelectInputOption } from "@/components/form";
import { Paginator } from "@/components/ui";
import { PlusIcon } from "@heroicons/react/24/outline";

// Props.
type ListModel<TItemModel extends object> = IListModel<TItemModel> & IUpsertableListModel<TItemModel>;

export type BaseListPageProps<TListModel extends ListModel<TItemModel>, TItemModel extends object> = {
  resourceName: string;
  model: TListModel;
  onModelUpdated: (updatedData: Partial<TListModel>) => any;
  isReloading: boolean;
  children: React.ReactNode;
  linkButtons?: React.ReactNode | React.ReactNode[];
  filterPanelChildren?: React.ReactNode | React.ReactNode[];
  sideBarPanels?: React.ReactNode | React.ReactNode[];
  canCreate: boolean;
};

// Components.
export default function IListModel<TListModel extends ListModel<TItemModel>, TItemModel extends object>
  (props: BaseListPageProps<TListModel, TItemModel>): React.ReactNode
{
  // Computed.
  const displayName = useMemo(() => getDisplayName(props.resourceName), []);

  const resultsPerPageOptions = useMemo<SelectInputOption[]>(() => {
    return [5, 10, 15, 20, 30, 40, 50].map(resultsPerPage => ({
      value: resultsPerPage.toString(),
      displayName: resultsPerPage.toString()
    }));
  }, []);
  
  // Template.
  return (
    <MainContainer className="gap-3">
      <div className={joinClassName(
        "grid grid-cols-1 gap-3 relative",
        props.sideBarPanels != null ? "lg:grid-cols-[1fr_20rem]" : null
      )}>
        <div className="flex flex-col items-stretch gap-3">
          <div className="panel">
            <div className="panel-header">
              <span className="panel-header-title">
                Danh sách {getDisplayName(props.resourceName)}
              </span>
            </div>

            <div className="panel-body flex flex-col p-3 gap-3">
              <FilterOptionsArea
                model={props.model}
                onModelUpdated={props.onModelUpdated}
                canCreate={props.canCreate}
                displayName={displayName}
              >
                {props.filterPanelChildren}
              </FilterOptionsArea>

              <div className="panel-body-area">
                {props.children}
              </div>

              <div className="flex justify-between">
                <SelectInput
                  className="w-22.5"
                  options={resultsPerPageOptions}
                  value={props.model.resultsPerPage.toString()}
                  onValueChanged={(resultsPerPageAsString) => {
                    props.onModelUpdated({
                      resultsPerPage: parseInt(resultsPerPageAsString)
                    } as Partial<TListModel>);
                  }}
                />

                {props.model.pageCount > 1 && (
                  <Paginator
                    page={props.model.page}
                    pageCount={props.model.pageCount}
                    onPageChanged={(page) => props.onModelUpdated({ page } as Partial<TListModel>)}
                    getPageButtonClassName={(_, isActive) => isActive ? "btn-primary" : undefined}
                  />
                )}
              </div>
            </div>
          </div>

          <div className="flex justify-end gap-3">
            {props.linkButtons}
            {props.model.createRoutePath && (
              <Link className="btn gap-1 shrink-0" to={props.model.createRoutePath}>
                <PlusIcon className="size-4.5" />
                <span>Tạo {displayName?.toLowerCase()} mới</span>
              </Link>
            )}
          </div>
        </div>

        {props.sideBarPanels}
      </div>
    </MainContainer>
  );
}
