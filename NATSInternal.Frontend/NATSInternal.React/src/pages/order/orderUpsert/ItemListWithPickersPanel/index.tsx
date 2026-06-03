import React from "react";
import { compute } from "@/helpers";

// Child components.
import ProductServicePickersContent from "./ProductServicePickersContent";
import PickedItemListContent from "./PickedItemListContent";

// Props.
type ItemListPanelProps = {
  model: OrderUpsertModel;
  onModelUpdated(updatedData: Partial<OrderUpsertModel>): any;
  errorCollection: ErrorCollectionModel;
};

// Components.
export default function ItemListPanel(props: ItemListPanelProps): React.ReactNode {
  // Computed.
  const panelTitle = compute<string>(() => {
    switch (props.model.type) {
      case "Retail":
        return "Sản phẩm";
      case "Treatment":
        return "Sản phẩm và dịch vụ";
      case "Consultant":
        return "Dịch vụ";
    }
  });

  // Template.
  return (
    <div className="panel h-full">
      <div className="panel-header">
        <span className="panel-header-title">
          {panelTitle}
        </span>
      </div>

      <div className="panel-body grid grid-cols-2 items-start gap-3 p-3 relative overflow-visible">
        <ProductServicePickersContent
          model={props.model}
          onProductItemCreated={(productItem) => {
            props.onModelUpdated({ productItems: [...props.model.productItems, productItem] });
          }}
          onProductItemUpdated={(productId, quantity) => {
            props.onModelUpdated({
              productItems: props.model.productItems.map(pi => {
                if (pi.product.id === productId) {
                  return { ...pi, quantity };
                }

                return pi;
              })
            });
          }}
          onServiceItemCreated={(serviceItem) => {
            props.onModelUpdated({ serviceItems: [...props.model.serviceItems, serviceItem] });
          }}
        />

        <PickedItemListContent
          model={props.model}
          onModelUpdated={props.onModelUpdated}
          errorCollection={props.errorCollection}
        />
      </div>
    </div>
  );
}
