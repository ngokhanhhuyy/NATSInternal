import React, { useState, useMemo, useEffect, useTransition } from "react";
import { api } from "@/api";
import { createProductListModel } from "@/models";
import { joinClassName } from "@/helpers";

// Child components.
import ProductList from "@/pages/shared/list/productListResults";
import { Paginator } from "@/components/ui";
import { CheckIcon, PlusIcon } from "@heroicons/react/24/outline";

// Props.
export type PickedProduct = {
  product: ProductBasicModel;
  quantity: number;
};

export type ProductPickerProps = {
  onProductPicked(product: ProductBasicModel): any;
  pickedProducts: PickedProduct[];
};

// Components.
export default function ProductPicker(props: ProductPickerProps): React.ReactNode {
  // States.
  const [isLoading, startTransition] = useTransition();
  const [model, setModel] = useState<ProductListModel>(() => {
    const m = createProductListModel();
    m.resultsPerPage = 10;
    m.outOfStockProductsIncluded = false;
    return m;
  });

  // Computed.
  const pickedProductIds = useMemo<number[]>(() => {
    return props.pickedProducts.map(pi => pi.product.id);
  }, [props.pickedProducts]);

  // Effect.
  useEffect(() => {
    startTransition(async () => {
      const requestDto = model.toRequestDto();
      const responseDto = await api.product.getListAsync(requestDto);
      setModel(m => m.mapFromResponseDto(responseDto));
    });
  }, [model.sortByAscending, model.sortByFieldName, model.page]);

  // Templates.
  const renderItemButton = (product: ProductBasicModel): React.ReactNode => {
    const quantity = props.pickedProducts.find(p => p.product.id === product.id)?.quantity;
    
    return (
      <div className="flex gap-2 justify-center items-center">
        {quantity != null && (
          <span className="text-yellow-700 dark:text-yellow-400">
            {quantity}
          </span>
        )}

        <button
          type="button"
          className={joinClassName("btn", pickedProductIds.includes(product.id) && "btn-primary-outline")}
          onClick={() => props.onProductPicked(product)}
        >
          {pickedProductIds.includes(product.id) ? (
            <PlusIcon />
          ) : (
            <CheckIcon />
          )}
        </button>
      </div>
    );
  };

  return (
    <div className="panel h-fit sticky top-[calc(var(--topbar-height)+--spacing(3))]">
      <div className="panel-header">
        <span className="panel-header-title">
          Chọn sản phẩm
        </span>
      </div>

      <div className={joinClassName("panel-body flex relative", isLoading && "pointer-events-none")}>
        <div className="flex flex-col w-full gap-3 p-3">
          <ProductList model={model} renderItemButton={renderItemButton} />

          <Paginator
            page={model.page}
            pageCount={model.pageCount}
            onPageChanged={(page) => setModel(m => ({ ...m, page }))}
            getPageButtonClassName={(_, isActive) => isActive ? "btn-primary" : null}
          />
        </div>
      </div>
    </div>
  );
}
