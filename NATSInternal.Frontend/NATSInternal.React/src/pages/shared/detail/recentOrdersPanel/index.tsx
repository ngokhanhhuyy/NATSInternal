import React, { useState, useMemo, useEffect, useTransition } from "react";
import { api } from "@/api";
import { createOrderListModel, createCustomerBasicModel, createProductBasicModel } from "@/models";
import { joinClassName } from "@/helpers";

// Child components.
import { SelectInput, type SelectInputOption } from "@/components/form";
import OrderListResults from "@/pages/shared/list/orderListResults";
import { ChevronLeftIcon, ChevronRightIcon } from "@heroicons/react/24/outline"; 

// Props.
type RecentOrdersPanelProps = {
  customerModel: CustomerDetailModel;
  productModel?: ProductDetailModel;
} | {
  customerModel?: CustomerDetailModel;
  productModel: ProductDetailModel;
};

// Component.
export default function RecentOrdersPanel(props: RecentOrdersPanelProps): React.ReactNode {
  // States.
  const [isInitialRendering, setIsInitialRendering] = useState(true);
  const [isLoading, startTransition] = useTransition();
  const [listModel, setListModel] = useState<OrderListModel>(() => {
    const m = createOrderListModel();
    m.resultsPerPage = 10;
    m.customer = props.customerModel ? createCustomerBasicModel(props.customerModel) : null;
    m.product = props.productModel ? createProductBasicModel(props.productModel) : null;
    return m;
  });

  // Computed.
  const resultsPerPageOptions = useMemo<SelectInputOption[]>(() => {
    return [5, 10, 15, 20, 50].map(page => ({
      value: page.toString(),
      displayName: page.toString()
    }));
  }, []);

  // Callbacks.
  function handleResultsPerPageChanged(resultsPerPageAsString: string): void {
    const resultsPerPage = parseInt(resultsPerPageAsString);
    setListModel(m => ({ ...m, resultsPerPage }));
  }

  // Effect.
  useEffect(() => {
    startTransition(async () => {
      const responseDto = await api.order.getListAsync(listModel.toRequestDto());
      setListModel(m => m.mapFromResponseDto(responseDto));

      setIsInitialRendering(false);
    });
  }, [listModel.page, listModel.resultsPerPage]);

  // Template.
  const renderContent = (): React.ReactNode => {
    const emptyOrLoadingClassName = "flex justify-center items-center py-10";
    if (isInitialRendering) {
      return (
        <div className={emptyOrLoadingClassName}>
          Đang tải ...
        </div>
      );
    }

    if (!listModel.items.length) {
      return (
        <div className={emptyOrLoadingClassName}>
          Không có giao dịch nào
        </div>
      );
    }

    return <OrderListResults model={listModel} hideCustomer={props.customerModel != null} />;
  };

  return (
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">
          Giao dịch gần nhất
        </span>
      </div>

      <div className={joinClassName("panel-body p-3", isLoading && "pointer-events-none")}>
        <div className="flex flex-col gap-3">
          <div className="flex gap-2 w-fit">
            <div className="form-input-group">
              <span className="form-input-group-text border-e-0 shrink-0">Kết quả mỗi trang</span>
              <SelectInput
                className="py-0 w-25"
                options={resultsPerPageOptions}
                value={listModel.resultsPerPage.toString()}
                onValueChanged={handleResultsPerPageChanged}
              />
            </div>

            {listModel.pageCount > 1 && (
              <>
                <button
                  type="button"
                  className="btn shrink-0 gap-0"
                  disabled={listModel.page - 1 < 1}
                  onClick={() => setListModel(m => ({ ...m, page: m.page - 1 }))}
                >
                  <ChevronLeftIcon />
                  <span className="hidden sm:inline">Trang trước</span>
                </button>

                <button
                  type="button"
                  className="btn shrink-0 gap-0"
                  disabled={listModel.page + 1 > listModel.pageCount}
                  onClick={() => setListModel(m => ({ ...m, page: m.page + 1 }))}
                >
                  <span className="hidden sm:inline">Trang sau</span>
                  <ChevronRightIcon />
                </button>
              </>
            )}
          </div>
          <div className={joinClassName(
            "bg-black/2.5 dark:bg-white/2.5",
            "border border-black/15 dark:border-white/15 rounded-lg"
          )}>
            {renderContent()}
          </div>
        </div>
      </div>
    </div>
  );
}
