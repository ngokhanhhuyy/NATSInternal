import React, { useState, useEffect, useTransition } from "react";
import { Link } from "react-router";

// Child components.
import { MainContainer } from "@/components/layouts";
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
    startTransition(async () => {
      const reloadedModel = await props.loadDataAsync(model);
      setModel(reloadedModel);
    });

    await Promise.resolve();
    window.scrollTo({ top: 0, behavior: "smooth" });
  }

  // Effect.
  useEffect(() => {
    if (isInitialRendering) {
      setIsInitialRendering(false);
      return;
    }

    reloadAsync().then(() => { });
  }, [model.sortByAscending, model.sortByFieldName, model.page, model.resultsPerPage]);

  // Template.
  const CreateLink = () => (
    <Link className="btn btn-panel-header btn-sm gap-1 shrink-0" to={model.createRoutePath}>
      <PlusIcon className="size-4.5" />
      <span>Tạo mới</span>
    </Link>
  );

  return (
    <MainContainer
      description={props.description}
      className="gap-3"
      isLoading={isReloading}
    >
      <div className="flex justify-start">
        <CreateLink />
      </div>

      <div className="flex flex-col items-stretch gap-3">
        <SearchablePageableListPageFilterBlock
          model={model}
          onModelChanged={changedData => setModel(m => ({ ...m, ...changedData }))}
          onSearchButtonClicked={reloadAsync}
          isReloading={isReloading}
        />

        <SearchablePageableListPageTableBlock
          model={model}
          onPageChanged={page => setModel(m => ({ ...m, page }))}
          onResultsPerPageChanged={resultsPerPage => setModel(m => ({ ...m, page: 1, resultsPerPage }))}
          isReloading={isReloading}
          renderHeaderRowChildren={props.renderTableHeaderRowChildren}
          renderBodyRowChildren={props.renderTableBodyRowChildren}
        />
      </div>

      <div className="flex justify-end mb-3">
        <CreateLink />
      </div>

      {props.children}
    </MainContainer>
  );
}