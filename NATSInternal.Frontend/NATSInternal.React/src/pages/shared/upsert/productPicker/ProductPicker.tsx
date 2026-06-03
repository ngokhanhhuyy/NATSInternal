import React, { useState, useMemo, useEffect, useTransition } from "react";
import { api } from "@/api";
import { createProductListModel } from "@/models";
import { compute, joinClassName } from "@/helpers";

// Child components.
import ProductList from "@/pages/shared/list/productListResults";
import { TextInput } from "@/components/form";
import { Paginator } from "@/components/ui";
import { CheckIcon, PlusIcon, MagnifyingGlassIcon, FunnelIcon } from "@heroicons/react/24/outline";

// Props.
export type PickedProduct = {
  product: ProductBasicModel;
  quantity: number;
};

export type ProductPickerProps = {
  onProductPicked(product: ProductBasicModel): any;
  pickedProducts: PickedProduct[];
  className?: string;
};

// Components.
export default function ProductPicker(props: ProductPickerProps): React.ReactNode {
  // States.
  const [isLoading, startTransition] = useTransition();
  const [renderingKey, setRenderingKey] = useState<number>(0);
  const [model, setModel] = useState<ProductListModel>(() => {
    const m = createProductListModel();
    m.resultsPerPage = 10;
    m.outOfStockProductsIncluded = false;
    m.discontinuedProductsIncluded = false;
    return m;
  });

  // Computed.
  const pickedProductIds = useMemo<number[]>(() => {
    return props.pickedProducts.map(pi => pi.product.id);
  }, [props.pickedProducts]);

  const isSearchContentValidationMessageVisible = compute<boolean>(() => {
    return !!model.searchContent && model.searchContent.length < 3;
  });

  // Effect.
  useEffect(() => {
    startTransition(async () => {
      if (model.searchContent && model.searchContent.length < 3) {
        return;
      }

      const requestDto = model.toRequestDto();
      const responseDto = await api.product.getListAsync(requestDto);
      setModel(m => m.mapFromResponseDto(responseDto));
    });
  }, [model.sortByAscending, model.sortByFieldName, model.page, renderingKey]);

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
    <div className={joinClassName(
      "flex flex-col w-full gap-3 p-3",
      isLoading && "pointer-events-none",
      props.className
    )}>
      <div className="flex flex-col">
        <div className="grid grid-cols-[1fr_auto] gap-2">
          <div className="grid grid-cols-[1fr_auto]">
            <TextInput
              className="rounded-e-none z-1"
              placeholder="Tìm kiếm sản phẩm ..."
              value={model.searchContent}
              onValueChanged={(searchContent) => setModel(m => ({ ...m, searchContent }))}
              onKeyDown={(event) => event.key === "Enter" && setRenderingKey(k => k + 1)}
            />
            <button type="button" className="btn rounded-s-none border-s-0">
              <MagnifyingGlassIcon />
            </button>
          </div>

          <button type="button" className="btn">
            <FunnelIcon />
          </button>
        </div>

        {isSearchContentValidationMessageVisible && (
          <span className="text-red-700 dark:text-red-400 text-sm">
            Nội dung tìm kiếm phải chứa ít nhất 3 ký tự.
          </span>
        )}
      </div>

      <ProductList model={model} renderItemButton={renderItemButton} />

      <Paginator
        page={model.page}
        pageCount={model.pageCount}
        onPageChanged={(page) => setModel(m => ({ ...m, page }))}
        getPageButtonClassName={(_, isActive) => isActive ? "btn-primary" : null}
      />
    </div>
  );
}
