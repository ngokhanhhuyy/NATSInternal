import React from "react";
import Paginator from "@/components/ui/Paginator";

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
      <div className="panel-body">
        {props.model.items.length ? (
          <table className="data-table">
            <thead className="whitespace-nowrap">
              <tr className="text-black/50 dark:text-white/50 font-bold">
                {props.renderHeaderRowChildren?.()}
              </tr>
            </thead>
            <tbody>
              {props.model.items.map((customer, index) => (
                <tr key={index}>
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

      {/* Footer */}
      {props.model.pageCount > 1 && (
        <div className="panel-footer flex justify-center px-3 py-2">
          <Paginator
            page={props.model.page}
            pageCount={props.model.pageCount}
            onPageChanged={props.onPageChanged}
            isReloading={props.isReloading}
            getPageButtonClassName={() => "btn-panel-footer"}
          />
        </div>
      )}
    </div>
  );
}