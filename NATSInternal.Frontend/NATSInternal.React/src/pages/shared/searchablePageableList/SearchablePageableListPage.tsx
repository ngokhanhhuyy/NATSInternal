import React, { useState, useEffect, useTransition } from "react";
import { Link } from "react-router";

// Child components.
import { MainContainer } from "@/components/layouts";
import { Paginator } from "@/components/ui";
import SearchablePageableListPageFilterBlock from "./SearchablePageableListPageFilterPanel";
import SearchablePageableListPageTableBlock from "./SearchablePageableListPageTablePanel";
import { PlusIcon } from "@heroicons/react/24/outline";

// Props.
type Props<
    TListModel extends
      ISearchableListModel<TItemModel> &
      ISortableListModel<TItemModel> &
      IPageableListModel<TItemModel> &
      IUpsertableListModel<TItemModel>,
    TItemModel extends object> = {
  description: string;
  initialModel: TListModel;
  loadDataAsync(model?: TListModel): Promise<TListModel>;
  renderTableHeaderRowChildren?(): React.ReactNode;
  renderTableBodyRowChildren?(itemModel: TItemModel): React.ReactNode;
  children?: React.ReactNode | React.ReactNode[];
};

// Components.
export default function SearchablePageableListPage<
      TListModel extends
        ISearchableListModel<TItemModel> &
        ISortableListModel<TItemModel> &
        IPageableListModel<TItemModel> &
        IUpsertableListModel<TItemModel>,
      TItemModel extends object>
    (props: Props<TListModel, TItemModel>): React.ReactNode {
  // States.
  const [model, setModel] = useState(() => props.initialModel);
  const [isInitialRendering, setIsInitialRendering] = useState(() => true);
  const [isReloading, startTransition] = useTransition();

  // Callbacks.
  async function reloadAsync(): Promise<void> {
    const reloadedModel = await props.loadDataAsync(model);
    setModel(reloadedModel);
    window.scrollTo({ top: 0, behavior: "smooth" });
  }

  // Effect.
  useEffect(() => {
    if (isInitialRendering) {
      setIsInitialRendering(false);
      return;
    }

    startTransition(reloadAsync);
  }, [model.searchContent, model.sortByAscending, model.sortByFieldName, model.page, model.resultsPerPage]);

  // Template.
  return (
    <MainContainer
      description={props.description}
      className="gap-3"
      isLoading={isReloading}
    >
      <div className="flex flex-col items-stretch gap-3">
        <SearchablePageableListPageTableBlock
          model={model}
          onPageChanged={page => setModel(m => ({ ...m, page }))}
          onResultsPerPageChanged={resultsPerPage => setModel(m => ({ ...m, page: 1, resultsPerPage }))}
          isReloading={isReloading}
          renderHeaderRowChildren={props.renderTableHeaderRowChildren}
          renderBodyRowChildren={props.renderTableBodyRowChildren}
        />

        <div className="flex justify-end gap-3 mb-3 md:mb-5">
          <Paginator
            page={model.page}
            pageCount={model.pageCount}
            onPageChanged={(page) => setModel(m => ({ ...m, page }))}
            isReloading={isReloading}
            getPageButtonClassName={(_, isActive) => isActive ? "btn-primary" : undefined}
          />

          <div className="border-r border-black/25 dark:border-white/25 w-px" />

          <Link className="btn gap-1 shrink-0" to={model.createRoutePath}>
            <PlusIcon className="size-4.5" />
            <span>Tạo sản phẩm mới</span>
          </Link>
        </div>

        <SearchablePageableListPageFilterBlock
          model={model}
          onModelChanged={changedData => setModel(m => ({ ...m, ...changedData }))}
          onSearchButtonClicked={reloadAsync}
          isReloading={false}
        />
      </div>

      {props.children}
    </MainContainer>
  );
}