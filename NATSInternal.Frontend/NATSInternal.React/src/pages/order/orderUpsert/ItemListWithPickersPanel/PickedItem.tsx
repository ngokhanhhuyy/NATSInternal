import React from "react";

// Child components.
import { FormField, NumberInput } from "@/components/form";
import { PhotoIcon, PlusIcon, MinusIcon, XMarkIcon } from "@heroicons/react/24/outline";

// Props.
type OrderItemUpsertModel = OrderProductItemUpsertModel | OrderServiceItemUpsertModel;
type ItemProps<T extends OrderItemUpsertModel> = {
  index: number;
  model: T;
  onModelUpdated(onModelUpdated: Partial<T>): any;
  onModelDeleted(): any;
};

// Components.
export default function PickedItem<T extends OrderItemUpsertModel>(props: ItemProps<T>): React.ReactNode {
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
