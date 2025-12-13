import React, { useState, useEffect, useTransition } from "react";

// Child components.
import { MainContainer } from "@/components/layouts";
import SearchablePageableListPageFilterBlock from "./SearchablePageableListPageFilterBlock";
import SearchablePageableListPageTableBlock from "./SearchablePageableListPageTableBlock";

// Props.
type Props<TListModel extends ISearchablePagableListModel<TItemModel>, TItemModel extends object> = {
  description: string;
  initialModel: TListModel;
  loadDataAsync(model?: TListModel): Promise<TListModel>;
  renderTableHeaderRowChildren?(): React.ReactNode;
  renderTableBodyRowChildren?(itemModel: TItemModel): React.ReactNode;
  children?: React.ReactNode | React.ReactNode[];
};

// Components.
export default function SearchablePageableListPage<
      TListModel extends ISearchablePagableListModel<TItemModel>,
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
  }

  // Effect.
  useEffect(() => {
    if (isInitialRendering) {
      setIsInitialRendering(false);
      return;
    }

    reloadAsync();
  }, [model.sortByAscending, model.sortByFieldName, model.page, model.resultsPerPage]);

  // Template.
  return (
    <MainContainer
      description={props.description}
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

        {props.children}
      </div>
    </MainContainer>
  );
}