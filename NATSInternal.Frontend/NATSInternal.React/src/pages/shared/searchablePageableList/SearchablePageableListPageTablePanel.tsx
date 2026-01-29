import React from "react";

// Props.
type Props<
    TListModel extends
      ISearchableListModel<TItemModel> &
      ISortableListModel<TItemModel> &
      IPageableListModel<TItemModel> &
      IUpsertableListModel<TItemModel>,
    TItemModel extends object> = {
  model: TListModel;
  onPageChanged(page: number): any;
  onResultsPerPageChanged(resultsPerPage: number): any;
  isReloading: boolean;
  renderHeaderRowChildren?(): React.ReactNode;
  renderBodyRowChildren?(itemModel: TItemModel): React.ReactNode;
};

// Component.
export default function SearchablePageableListPageTableBlock<
      TListModel extends
        ISearchableListModel<TItemModel> &
        ISortableListModel<TItemModel> &
        IPageableListModel<TItemModel> &
        IUpsertableListModel<TItemModel>,
      TItemModel extends object>
    (props: Props<TListModel, TItemModel>): React.ReactNode {
  // Template.
  return (
    <div className="panel">
      {/* Header */}
      <div className="panel-header">
        {/* Title */}
        <div className="panel-header-title">
          Danh sách kết quả
        </div>
      </div>

      {/* Body */}
      <div className="panel-body flex-1 min-w-0">
        <div className="w-full overflow-x-auto">
          {props.model.items.length ? (
            <table className="data-table min-w-max w-full">
              <thead className="whitespace-nowrap">
                <tr className="text-black/50 dark:text-white/50 font-bold">
                  {props.renderHeaderRowChildren?.()}
                </tr>
              </thead>
              <tbody>
                {props.model.items.map((customer, index) => (
                  <tr key={index} className="whitespace-nowrap">
                    {props.renderBodyRowChildren?.(customer)}
                  </tr>
                ))}
              </tbody>
            </table>
          ) : (
            <div className="flex justify-center items-center w-full p-8 opacity-50">
              Không có kết quả
            </div>
          )}
        </div>
      </div>
    </div>
  );
}