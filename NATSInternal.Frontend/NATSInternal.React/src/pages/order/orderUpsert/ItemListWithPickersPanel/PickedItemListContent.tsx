import React, { useMemo } from "react";
import { joinClassName } from "@/helpers";

// Child components.
import PickedItem from "./PickedItem";

// Props.
type PickedItemListContentProps = {
  model: OrderUpsertModel;
  onModelUpdated(updatedData: Partial<OrderUpsertModel>): any;
};

// Components.
export default function PickedItemListPanel(props: PickedItemListContentProps): React.ReactNode {
  // Computed.
  const displayAmountAfterVat = useMemo<string>(() => {
    return props.model.computeDisplayAmountAfterVat();
  }, [props.model.productItems, props.model.serviceItems]);

  // Template.
  if (props.model.productItems.length + props.model.serviceItems.length) {
    return (
      <div className="flex flex-col justify-between gap-3 h-full">
        <ul className="list-group">
          {props.model.productItems.map((productItem, index) => (
            <PickedItem
              index={index}
              model={productItem}
              onModelUpdated={(updatedData) => {
                const productItems = props.model.productItems.map(item => {
                  if (item.guid !== productItem.guid) {
                    return item;
                  }

                  return { ...item, ...updatedData };
                });
                
                props.onModelUpdated?.({ productItems });
              }}
              onModelDeleted={() => {
                const productItems = props.model.productItems.filter(item => item.guid !== productItem.guid);
                props.onModelUpdated?.({ productItems });
                }}
              key={productItem.guid}
            />
          ))}
          
          {props.model.serviceItems.map((serviceItem, index) => (
            <PickedItem
              index={props.model.productItems.length + index}
              model={serviceItem}
              onModelUpdated={(updatedData) => {
                const serviceItems = props.model.serviceItems.map(item => {
                  if (item.guid !== serviceItem.guid) {
                    return item;
                  }

                  return { ...item, ...updatedData };
                });
                
                props.onModelUpdated?.({ serviceItems });
              }}
              onModelDeleted={() => {
                const serviceItems = props.model.serviceItems.filter(item => item.guid !== serviceItem.guid);
                props.onModelUpdated?.({ serviceItems });
              }}
              key={serviceItem.guid}
            />
          ))}
        </ul>

        <div className={joinClassName(
          "bg-black/7.5 dark:bg-white/10",
          "border border-black/10 dark:border-white/10 rounded-lg p-3 flex justify-end gap-10"
        )}>
          <span className="text-blue-700 dark:text-blue-400 font-bold">
            Thành tiền
          </span>

          <span>{displayAmountAfterVat}</span>
        </div>
      </div>
    );
  }

  return (
    <div className={joinClassName(
      "flex justify-center items-center",
      "border border-black/10 dark:border-white/10 rounded-xl h-full"
    )}>
      <span className="opacity-50">
        Chưa chọn sản phẩm và dịch vụ
      </span>
    </div>
  );
}
