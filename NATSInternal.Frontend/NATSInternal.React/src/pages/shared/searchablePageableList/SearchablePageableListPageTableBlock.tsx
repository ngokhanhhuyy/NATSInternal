import React, { useMemo } from "react";
import { Link } from "react-router";
import { useTsxHelper } from "@/helpers";
import styles from "./SearchablePageableListPageTableBlock.module.css";

// Child component.
import { Block, MainPaginator } from "@/components/ui";
import { SelectInput } from "@/components/form";
import { PlusIcon } from "@heroicons/react/24/outline";

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
  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // Computed.
  const resultsPerPageOptions = useMemo(() => {
    return [15, 20, 30, 40, 50].map(resultsPagePage => ({
      value: resultsPagePage.toString(),
      displayName: resultsPagePage.toString()
    }));
  }, []);

  // Template.
  return (
    <Block
      bodyClassName="overscroll-x-none h-fit overflow-x-auto w-full"
      title="Danh sách kết quả"
      headerChildren={(
        <div className="flex gap-3 items-center">
          <SelectInput
            className={joinClassName(
              "small bg-white dark:bg-neutral-800 border-black/20 dark:border-white/20",
              "focus:border-black focus:dark:border-white"
            )}
            options={resultsPerPageOptions}
            value={props.model.resultsPerPage.toString()}
            onValueChanged={resultsPerPageAsString => props.onResultsPerPageChanged(parseInt(resultsPerPageAsString))}
          />

          <Link className="button block-header-button small gap-1 shrink-0" to={props.model.createRoutePath}>
            <PlusIcon className="size-4.5" />
            <span>Tạo mới</span>
          </Link>
        </div>
      )}
      footerChildren={props.model.pageCount <= 1 ? null : (
        <div className="flex justify-center items-center w-full">
          <MainPaginator
            page={props.model.page}
            pageCount={props.model.pageCount}
            isReloading={props.isReloading}
            onPageChanged={props.onPageChanged}
          />
        </div>
      )}
    >
      {props.model.items.length ? (
        <table className={joinClassName("border-collapse min-w-max w-full p-0", styles.tableBlock)}>
          <thead className="whitespace-nowrap">
            <tr className="text-black/50 dark:text-white/50 font-bold">
              {props.renderHeaderRowChildren?.()}
            </tr>
          </thead>
          <tbody>
            {props.model.items.map((customer, index) => (
              <tr className="odd:bg-black/3 dark:odd:bg-white/3 whitespace-nowrap" key={index}>
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
    </Block>
  );
}