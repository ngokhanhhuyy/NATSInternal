import React, { useMemo } from "react";
import { joinClassName } from "@/helpers";

// Child components.
import { FormField, NumberInput } from "@/components/form";
import { PhotoIcon, PlusIcon, MinusIcon, XMarkIcon } from "@heroicons/react/24/outline";

// Props.
type ItemListPanelProps = {
  model: OrderUpsertModel;
  onModelUpdated(updatedData: Partial<OrderUpsertModel>): any;
};

type OrderItemUpsertModel = OrderProductItemUpsertModel | OrderServiceItemUpsertModel;
type ItemProps<T extends OrderItemUpsertModel> = {
  index: number;
  model: T;
  onModelUpdated(onModelUpdated: Partial<T>): any;
  onModelDeleted(): any;
};

// Components.
export default function ItemListPanel(props: ItemListPanelProps): React.ReactNode {
  // Computed.
  const displayAmountAfterVat = useMemo<string>(() => {
    return props.model.computeDisplayAmountAfterVat();
  }, [props.model.productItems, props.model.serviceItems]);

  // Template.
  return (
    <div className="panel h-full">
      <div className="panel-header">
        <span className="panel-header-title">
          Danh sách sản phẩm và dịch vụ
        </span>
      </div>

      <div className="panel-body flex flex-col justify-between gap-3 p-3 h-full">
        {(props.model.productItems.length + props.model.serviceItems.length) ? (
          <>
            <ul className="list-group">
              {props.model.productItems.map((productItem, index) => (
                <OrderUpsertItem
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
                <OrderUpsertItem
                  index={props.model.serviceItems.length + index}
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

            <div className="border border-black/10 dark:border-white/10 rounded-lg p-3 flex justify-end gap-10">
              <span className="text-blue-700 dark:text-blue-400 font-bold">
                Thành tiền
              </span>

              <span>{displayAmountAfterVat}</span>
            </div>
          </>
        ) : (
          <div className={joinClassName(
            "flex justify-center items-center",
            "border border-black/10 dark:border-white/10 rounded-xl h-full"
          )}>
            <span className="opacity-50">
              Chưa chọn sản phẩm và dịch vụ
            </span>
          </div>
        )}
      </div>
    </div>
  );
}

function OrderUpsertItem<T extends OrderItemUpsertModel>(props: ItemProps<T>): React.ReactNode {
  // Computed.
  function computePath(path: string): string {
    const prefix = isOrderProductItemUpsertModel(props.model) ? "productItems" : "serviceItems";
    return `${prefix}.[${props.index}].${path}`;
  }

  // Callbacks.
  function incrementQuantity(): void {
    props.onModelUpdated?.({ quantity: props.model.quantity + 1 } as Partial<T>);
  }
  
  function decrementQuantity(): void {
    props.onModelUpdated?.({ quantity: props.model.quantity - 1 } as Partial<T>);
  }

  // Template.
  return (
    <li className="list-group-item flex py-2 ps-3">
      <div className="grid grid-cols-[auto_1fr_auto] items-center gap-3">
        <div className="hidden xl:block">
          {(isOrderProductItemUpsertModel(props.model) && props.model.product.thumbnailUrl) ? (
            <img
              src={props.model.product.thumbnailUrl}
              className="img-thumbnail size-12"
              alt={props.model.product.name}
            />
          ) : (
            <div className="img-thumbnail size-12 flex justify-center items-center">
              <PhotoIcon className="size-6 opacity-50" />
            </div>
          )}
        </div>

        <div className="overflow-x-hidden">
          <div className="text-blue-700 dark:text-blue-400 whitespace-nowrap text-ellipsis overflow-hidden ms-0.5">
            <span className="font-bold">
              {`${props.index + 1}. `}
              {isOrderProductItemUpsertModel(props.model) ? props.model.product.name : props.model.name}
            </span>
          </div>

          <div className="grid grid-cols-[2fr_1.25fr_2fr_auto] gap-3 text-sm ms-0.5 mb-0.5">
            <FormField path={computePath("amountBeforeVatPerUnit")} hideLabel hideValidationMessage>
              <div className="form-input-group">
                <NumberInput
                  className="form-control-sm rounded-r-none"
                  value={props.model.amountBeforeVatPerUnit}
                  onValueChanged={(amountBeforeVatPerUnit) => {
                    props.onModelUpdated({ amountBeforeVatPerUnit } as Partial<T>);
                  }}
                />
                <span className="form-input-group-text border-s-0">đ</span>
              </div>
            </FormField>
            
            <FormField path={computePath("vatPercentagePerUnit")} hideLabel hideValidationMessage>
              <div className="form-input-group">
                <NumberInput
                  className="form-control-sm rounded-r-none"
                  value={props.model.vatPercentagePerUnit}
                  onValueChanged={(vatPercentagePerUnit) => {
                    props.onModelUpdated({ vatPercentagePerUnit } as Partial<T>);
                  }}
                />
                <span className="form-input-group-text border-s-0">%</span>
              </div>
            </FormField>
            
            <FormField path={computePath("quantity")} hideLabel hideValidationMessage>
              <div className="form-input-group">
                <button
                  type="button"
                  className="btn border-e-0"
                  onClick={decrementQuantity}
                  disabled={props.model.quantity === 1}
                >
                  <MinusIcon />
                </button>

                <NumberInput
                  className="form-control-sm rounded-none text-center z-1"
                  value={props.model.quantity}
                  onValueChanged={(quantity) => {
                    props.onModelUpdated({ quantity } as Partial<T>);
                  }}
                  min={1}
                  max={100}
                />

                <button
                  type="button"
                  className="btn border-s-0"
                  onClick={incrementQuantity}
                  disabled={props.model.quantity === 100}
                >
                  <PlusIcon />
                </button>
              </div>
            </FormField>

            <button
              type="button"
              className="btn btn-danger"
              onClick={props.onModelDeleted}
              disabled={props.model.quantity === 100}
            >
              <XMarkIcon />
            </button>
          </div>
        </div>

        <div></div>
      </div>
    </li>
  );
}

function isOrderProductItemUpsertModel(model: OrderItemUpsertModel): model is OrderProductItemUpsertModel {
  return Object.hasOwn(model, "product");
}
