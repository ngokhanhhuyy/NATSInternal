import React, { useMemo } from "react";
import { Link } from "react-router";
import { getDisplayName } from "@/metadata";
import { useTsxHelper } from "@/helpers";

// Child components.
import FilterOptionsPanel from "./FilterOptionsPanel";
import { MainContainer } from "@/components/layouts";
import { Paginator } from "@/components/ui";
import { PlusIcon } from "@heroicons/react/24/outline";

// Props.
type ListModel<TItemModel extends object> =
  ISearchableListModel<TItemModel> &
  ISortableListModel<TItemModel> &
  IPageableListModel<TItemModel> &
  IUpsertableListModel<TItemModel>;

type Props<TListModel extends ListModel<TItemModel>, TItemModel extends object> = {
  resourceName: string;
  model: TListModel;
  onModelUpdated: (updatedData: Partial<TListModel>) => any;
  isReloading: boolean;
  children?: React.ReactNode | React.ReactNode[];
  onPaginatorPageChanged: (page: number) => any;
  onFilterPanelReloadButtonClicked: () => any;
  linkButtons?: React.ReactNode | React.ReactNode[];
  filterPanelChildren?: React.ReactNode | React.ReactNode[];
  sideBarPanels?: React.ReactNode | React.ReactNode[];
  additionalDirtyModelComparer?: (originalModel: TListModel, additionalModel: TListModel) => boolean;
};

// Components.
export default function SearchablePageableListPage<TListModel extends ListModel<TItemModel>,  TItemModel extends object>
  (props: Props<TListModel, TItemModel>): React.ReactNode
{
  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // Computed.
  const displayName = useMemo(() => getDisplayName(props.resourceName), []);
  
  // Template.
  return (
    <MainContainer className="gap-3" isLoading={props.isReloading}>
      <div className={joinClassName(
        "grid grid-cols-1 gap-3",
        props.sideBarPanels != null ? "xl:grid-cols-[1fr_25rem]" : null
      )}>
        <div className="flex flex-col items-stretch gap-3">
          {props.children}

          <div className="flex justify-end gap-3">
            <Paginator
              page={props.model.page}
              pageCount={props.model.pageCount}
              onPageChanged={props.onPaginatorPageChanged}
              getPageButtonClassName={(_, isActive) => isActive ? "btn-primary" : undefined}
            />

            {props.model.pageCount > 1 && (
              <div className="border-r border-black/25 dark:border-white/25 w-px" />
            )}
            
            <Link className="btn gap-1 shrink-0" to={props.model.createRoutePath}>
              <PlusIcon className="size-4.5" />
              <span>Tạo {displayName?.toLowerCase()} mới</span>
            </Link>
          </div>
          
          {props.linkButtons}

          <FilterOptionsPanel
            model={props.model}
            onModelUpdated={props.onModelUpdated}
            onReloadButtonClicked={props.onFilterPanelReloadButtonClicked}
            additionalDirtyModelComparer={props.additionalDirtyModelComparer}
          >
            {props.filterPanelChildren}
          </FilterOptionsPanel>
        </div>

        {props.sideBarPanels}
      </div>
    </MainContainer>
  );
}