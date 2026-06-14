import React, { useMemo } from "react";
import { joinClassName, compute } from "@/helpers";

// Child components.
import PickedItem from "./PickedItem";
import { ExclamationCircleIcon } from "@heroicons/react/24/outline";

// Props.
type PickedItemListContentProps = {
  model: OrderUpsertModel;
  onModelUpdated(updatedData: Partial<OrderUpsertModel>): any;
  errorCollection: ErrorCollectionModel;
};

// Components.
export default function PickedItemListPanel(props: PickedItemListContentProps): React.ReactNode {
  // Computed.
  const displayAmountAfterVat = useMemo<string>(() => {
    return props.model.computeDisplayAmountAfterVat();
  }, [props.model.productItems, props.model.serviceItems]);

  const emptyValidationMessages = compute<string[] | null>(() => {
    if (!props.errorCollection.isValidated || !props.errorCollection.details.length) {
      return null;
    }

    if (props.model.productItems.length + props.model.serviceItems.length) {
      return null;
    }

    return props.errorCollection.details
      .filter(detail => detail.propertyPath === "productItems" || detail.propertyPath === "serviceItems")
      .map(detail => detail.message);
  });

  // Template.
  if (emptyValidationMessages?.length) {
    return (
      <div className={joinClassName(
        "bg-red-600/15 dark:bg-red-500/5 grid grid-cols-[auto_auto] justify-center items-center gap-3 py-10",
        "border border-red-600 dark:border-red-500 rounded-xl h-full text-red-600 dark:text-red-500"
      )}>
        <ExclamationCircleIcon className="size-6" />
        <div className="flex flex-col">
          {emptyValidationMessages.map((message, index) => (
            <span key={index}>
              {message}
            </span>
          ))}
        </div>
      </div>
    );
  }

  if (props.model.productItems.length + props.model.serviceItems.length) {
    return (
      <div className="flex flex-col justify-between gap-3 h-full">
        <div className="panel-body-area">
          <ul className="list-group list-group-flush border-blue-600 dark:border-blue-400">
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
        </div>

        <div className={joinClassName(
          "bg-black/2.5 dark:bg-white/10",
          "border border-black/15 dark:border-white/15 rounded-lg p-3 flex justify-end gap-10"
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
      "panel-body-area flex justify-center items-center py-10",
      "border border-blue-500 dark:border-blue-400 rounded-xl h-full"
    )}>
      <span className="opacity-50">
        Chưa chọn sản phẩm và dịch vụ
      </span>
    </div>
  );
}
