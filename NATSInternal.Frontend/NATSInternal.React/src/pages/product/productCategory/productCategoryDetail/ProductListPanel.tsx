import React, { useState, useEffect, startTransition } from "react";
import { api } from "@/api";
import { createProductListModel } from "@/models";
import { useInitialRendering } from "@/hooks";
import { joinClassName } from "@/helpers";

// Child components.
import ProductListResults from "@/pages/shared/list/productListResults";
import { ChevronLeftIcon, ChevronRightIcon } from "@heroicons/react/24/outline";

// Props.
type Props = {
  productCategoryModel: ProductCategoryDetailModel;
};

// Component.
export default function ProductListPanel(props: Props): React.ReactNode {
  // States.
  const isInitialRendering = useInitialRendering();
  const [model, setModel] = useState<ProductListModel>(() => {
    const m = createProductListModel();
    m.resultsPerPage = 9;
    m.category = props.productCategoryModel.toBasicModel();
    return m;
  });

  // Effect.
  useEffect(() => {
    startTransition(async () => {
      const responseDto = await api.product.getListAsync(model.toRequestDto());
      setModel(m => m.mapFromResponseDto(responseDto));
    });
  }, [model.page]);

  // Template.
  return (
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">Danh sách sản phẩm</span>
      </div>

      <div className="panel-body flex flex-col gap-3 p-3">
        {!isInitialRendering && (
          <div className="flex justify-start items-center gap-3">
            <div className="flex justify-start self-start">
              <button
                type="button"
                className="btn rounded-e-none z-1"
                onClick={() => setModel(m => ({ ...m, page: m.page - 1 }))}
                disabled={model.page === 1}
              >
                <ChevronLeftIcon className="size-4" />
              </button>

              <span className={joinClassName(
                "form-control border-x-0 rounded-none",
                model.pageCount === 1 && "opacity-50"
              )}>
                Trang {model.page} / {model.pageCount}
              </span>

              <button
                type="button"
                className="btn rounded-s-none z-1"
                onClick={() => setModel(m => ({ ...m, page: m.page + 1 }))}
                disabled={model.page === model.pageCount}
              >
                <ChevronRightIcon className="size-4" />
              </button>
            </div>

            <span className="opacity-50">
              Hiển thị {Math.min(model.resultsPerPage, model.itemCount)} trên tổng số {model.itemCount} kết quả
            </span>
          </div>
        )}

        <div className="panel-body-area w-full">
          <ProductListResults model={model} className="list-group-flush" />
        </div>
      </div>
    </div>
  );
}
