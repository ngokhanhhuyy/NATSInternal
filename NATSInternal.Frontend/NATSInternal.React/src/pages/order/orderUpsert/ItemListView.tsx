import React, { useMemo } from "react";
import { createOrderProductItemUpsertModel } from "@/models";

// Child components.
import ProductPicker, { type PickedProduct } from "@/pages/shared/upsert/productPicker";
import ItemListPanel from "./ItemListPanel";

// Props.
type ItemListViewProps = {
  model: OrderUpsertModel;
  onModelUpdated(updatedData: Partial<OrderUpsertModel>): any;
};

// Components.
export default function ItemListView(props: ItemListViewProps): React.ReactNode {
  // Computed.
  const pickedProducts = useMemo<PickedProduct[]>(() => {
    return props.model.productItems.map(pi => ({
      product: pi.product,
      quantity: -pi.quantity
    }));
  }, [props.model.productItems]);

  // Callbacks.
  function handleProductPicked(product: ProductBasicModel): void {
    let alreadyAdded = false;
    const productItems: OrderProductItemUpsertModel[] = props.model.productItems.map(pi => {
      if (pi.product.id === product.id) {
        alreadyAdded = true;
        return { ...pi, quantity: pi.quantity + 1 };
      }

      return pi;
    });

    if (!alreadyAdded) {
      const item = createOrderProductItemUpsertModel(product);
      productItems.push(item);
    }

    props.onModelUpdated({ productItems });
  }

  // Templates.
  return (
    <div className="flex flex-col gap-3">
      <div className="grid grid-cols-2 gap-3 w-full static">
        <ProductPicker
          onProductPicked={handleProductPicked}
          pickedProducts={pickedProducts}
        />

        <ItemListPanel model={props.model} onModelUpdated={props.onModelUpdated} />
      </div>
    </div>
  );
}
