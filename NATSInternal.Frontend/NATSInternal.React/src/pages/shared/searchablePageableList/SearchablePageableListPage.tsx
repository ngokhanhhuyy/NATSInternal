import React, { useState, useEffect, useTransition } from "react";

// Child components.
import { MainContainer } from "@/components/layouts";
import SearchablePageableListPageFilterBlock from "./SearchablePageableListPageFilterPanel";
import SearchablePageableListPageTableBlock from "./SearchablePageableListPageTablePanel";

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
  }, [model.sortByAscending, model.sortByFieldName, model.page, model.resultsPerPage]);

  // Template.
  return (
    <MainContainer
      description={props.description}
      className="gap-3"
      isLoading={isReloading}
    >
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

      {props.children}
    </MainContainer>
  );
}