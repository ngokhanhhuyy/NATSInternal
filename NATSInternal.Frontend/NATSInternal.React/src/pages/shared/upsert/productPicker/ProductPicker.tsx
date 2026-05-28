import React, { useState, useCallback, useEffect, useTransition } from "react";
import { api } from "@/api";
import { createProductListModel } from "@/models";
import { joinClassName } from "@/helpers";

// Child components.
import ProductList from "@/pages/shared/list/productListResults";
import { Paginator } from "@/components/ui";
import { CheckIcon, ArrowsRightLeftIcon } from "@heroicons/react/24/outline";

// Props.
export type ProductPickerProps = {
  onProductPicked(product: ProductBasicModel): any;
  renderView(switchToProductList: () => any): React.ReactNode;
};

// Components.
export default function ProductPicker(props: ProductPickerProps): React.ReactNode {
  // States.
  const [isProductListVisible, setIsProductListVisible] = useState<boolean>(true);
  const [isLoading, startTransition] = useTransition();
  const [model, setModel] = useState<ProductListModel>(() => {
    const m = createProductListModel();
    m.resultsPerPage = 10;
    m.outOfStockProductsIncluded = false;
    return m;
  });

  // Callbacks.
  const handlePickButtonClicked = useCallback((product: ProductBasicModel) => {
    setIsProductListVisible(false);
    props.onProductPicked(product);
  }, []);

  const switchToProductList = useCallback(() => {
    setIsProductListVisible(true);
  }, []);

  // Effect.
  useEffect(() => {
    startTransition(async () => {
      const requestDto = model.toRequestDto();
      const responseDto = await api.product.getListAsync(requestDto);
      setModel(m => m.mapFromResponseDto(responseDto));
    });
  }, [model.sortByAscending, model.sortByFieldName, model.page]);

  // Templates.
  return (
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">
          Chọn sản phẩm
        </span>

        <div className="flex gap-1 justify-center items-center">
          <span>{isProductListVisible.toString()}</span>
          <button type="button" className="btn btn-sm" onClick={() => setIsProductListVisible(visible => !visible)}>
            <ArrowsRightLeftIcon />
          </button>
        </div>
      </div>

      <div className={joinClassName("panel-body flex relative", isLoading && "pointer-events-none")}>
        <div className={joinClassName(
          "flex gap-2 w-[200%] shrink-0 relative transition-[left] duration-300",
          !isProductListVisible ? "-left-full" : "left-0"
        )}>
          <div className={joinClassName(
            "flex flex-col gap-3 p-3 w-full transition-opacity duration-300",
            !isProductListVisible ? "opacity-0" : "opacity-100"
          )}>
            <ProductList
              model={model}
              renderItem={(product: ProductBasicModel) => (
                <div className="flex justify-center items-center">
                  <button type="button" className="btn" onClick={() => handlePickButtonClicked(product)}>
                    <CheckIcon />
                  </button>
                </div>
              )}
            />

            <Paginator
              page={model.page}
              pageCount={model.pageCount}
              onPageChanged={(page) => setModel(m => ({ ...m, page }))}
              getPageButtonClassName={(_, isActive) => isActive ? "btn-primary" : null}
            />
          </div>

          <div className={joinClassName(
            "flex transition duration-300 w-full",
            isProductListVisible ? "opacity-0" : "opacity-100"
          )}>
            {props.renderView(switchToProductList)}
          </div>
        </div>
      </div>
    </div>
  );
}
