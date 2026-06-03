import React, { useState, useMemo, useEffect } from "react";
import { createOrderProductItemUpsertModel, createOrderServiceItemUpsertModel } from "@/models";
import { joinClassName, compute } from "@/helpers";

// Child components.
import ProductPicker, { type PickedProduct } from "@/pages/shared/upsert/productPicker";
import { FormField, TextInput, NumberInput, NumberInputWithControlButtons } from "@/components/form";
import { CheckIcon } from "@heroicons/react/24/outline";

// Props.
type ProductServicePickerContentProps = {
  model: OrderUpsertModel;
  onProductItemCreated(productItem: OrderProductItemUpsertModel): any;
  onProductItemUpdated(productId: number, quantity: number): any;
  onServiceItemCreated(serviceItem: OrderServiceItemUpsertModel): any;
};

// Components.
export default function ProductServicePickerContent(props: ProductServicePickerContentProps): React.ReactNode {
  // States.
  const [serviceItemModel, setServiceItemModel] = useState(createOrderServiceItemUpsertModel);
  const [isServiceItemValidated, setIsServiceItemValidated] = useState(false);
  const [mode, setMode] = useState<"Product" | "Service">(() => {
    return props.model.type === "Consultant" ? "Service" : "Product";
  });

  // Computed.
  const pickedProducts = useMemo<PickedProduct[]>(() => {
    return props.model.productItems.map(pi => ({
      product: pi.product,
      quantity: -pi.quantity
    }));
  }, [props.model.productItems]);

  const isServiceItemNameLengthValid = compute<boolean>(() => serviceItemModel.name.length > 0);
  const isServiceItemNameUnique = compute<boolean>(() => {
    return isServiceItemNameLengthValid && !props.model.serviceItems.map(si => si.name).includes(serviceItemModel.name);
  });
  const isServiceItemQuantityValid = compute<boolean>(() => serviceItemModel.quantity >= 1);
  const isServiceItemAmountBeforeVatPerUnitValid = compute<boolean>(() => {
    return serviceItemModel.amountBeforeVatPerUnit >= 1000;
  });
  const isServiceItemVatPercentagePerUnitValid = compute<boolean>(() => {
    return serviceItemModel.vatPercentagePerUnit >= 0 && serviceItemModel.vatPercentagePerUnit <= 200;
  });

  // Callbacks.
  function handleProductPicked(product: ProductBasicModel): void {
    const pickedProduct = pickedProducts.find(pp => pp.product.id === product.id);
    console.log(pickedProduct);
    if (pickedProduct) {
      props.onProductItemUpdated(product.id, -pickedProduct.quantity + 1);
      return;
    }

    props.onProductItemCreated(createOrderProductItemUpsertModel(product));
  }

  function handleServicePicked(): void {
    const isValid = (
      isServiceItemNameLengthValid &&
      isServiceItemNameUnique &&
      isServiceItemQuantityValid &&
      isServiceItemAmountBeforeVatPerUnitValid &&
      isServiceItemVatPercentagePerUnitValid
    );

    if (!isValid) {
      setIsServiceItemValidated(true);
      return;
    }

    props.onServiceItemCreated(serviceItemModel);
    setServiceItemModel(() => createOrderServiceItemUpsertModel());
    setIsServiceItemValidated(false);
  }

  // Effect.
  useEffect(() => {
    if (mode === "Product" && props.model.type === "Consultant") {
      setMode("Service");
    } else if (mode === "Service" && props.model.type === "Retail") {
      setMode("Product");
    }
  }, [props.model.type]);

  // Templates.
  return (
    <div className={joinClassName(
      "border border-black/10 dark:border-white/10 rounded-xl h-fit",
      "sticky top-[calc(var(--topbar-height)+--spacing(3))]"
    )}>
      {props.model.type === "Treatment" && (
        <div className="flex justify-center p-2 border-b border-b-black/10 dark:border-b-white/10">
          <div className="grid grid-cols-2">
            <button
              type="button"
              className={joinClassName(
                "btn btn-sm rounded-e-none px-3",
                mode === "Product" ? "btn-primary" : "not-dark:btn-primary-outline border-e-transparent"
              )}
              onClick={() => setMode("Product")}
            >
              Sản phẩm
            </button>

            <button
              type="button"
              className={joinClassName(
                "btn btn-sm rounded-s-none px-3",
                mode === "Service" ? "btn-primary" : "not-dark:btn-primary-outline border-s-transparent"
              )}
              onClick={() => setMode("Service")}
            >
              Dịch vụ
            </button>
          </div>
        </div>
      )}

      <ProductPicker
        className={joinClassName(mode !== "Product" && "hidden")}
        onProductPicked={handleProductPicked}
        pickedProducts={pickedProducts}
      />

      <div className={joinClassName(
        "grid grid-cols-6 gap-3 p-3 pt-2 items-start",
        mode !== "Service" && "hidden"
      )}>
        <FormField className="col-span-4" path="serviceItem.name" displayName="Tên dịch vụ">
          <TextInput
            className={joinClassName(
              (isServiceItemValidated && !(isServiceItemNameLengthValid && isServiceItemNameUnique)) && "is-invalid"
            )}
            placeholder="Tên dịch vụ"
            value={serviceItemModel.name}
            onValueChanged={(name) => setServiceItemModel(m => ({ ...m, name }))}
          />

          {isServiceItemValidated && !isServiceItemNameLengthValid && (
            <span className="text-red-600 dark:text-red-400 text-sm">
              Tên dịch vụ không được để trống.
            </span>
          )}

          {isServiceItemValidated && !isServiceItemNameUnique && (
            <span className="text-red-600 dark:text-red-400 text-sm">
              Tên dịch vụ đã tồn tại.
            </span>
          )}
        </FormField>

        <FormField className="col-span-2" path="serviceItem.quantity" displayName="Số lượng">
          <NumberInputWithControlButtons
            className={joinClassName(isServiceItemValidated && !isServiceItemQuantityValid && "is-invalid")}
            value={serviceItemModel.quantity}
            onValueChanged={(quantity) => setServiceItemModel(m => ({ ...m, quantity }))}
            min={1}
            max={99}
            step={1}
          />

          {isServiceItemValidated && !isServiceItemQuantityValid && (
            <span className="text-red-600 dark:text-red-400 text-sm">
              Số lượng phải nằm trong khoảng từ 1 đến 99.
            </span>
          )}
        </FormField>

        <FormField className="col-span-3" path="serviceItem.amountBeforeVatPerUnit" displayName="Đơn giá">
          <div className="form-input-group">
            <NumberInput
              className={joinClassName(
                isServiceItemValidated && !isServiceItemAmountBeforeVatPerUnitValid && "is-invalid"
              )}
              value={serviceItemModel.amountBeforeVatPerUnit}
              onValueChanged={(amountBeforeVatPerUnit) => {
                setServiceItemModel(m => ({ ...m, amountBeforeVatPerUnit }));
              }}
            />
            <span className="form-input-group-text">vnđ</span>
          </div>

          {isServiceItemValidated && !isServiceItemAmountBeforeVatPerUnitValid && (
            <span className="text-red-600 dark:text-red-400 text-sm">
              Đơn giá dịch vụ phải có giá trị ít nhất là 1000đ.
            </span>
          )}
        </FormField>

        <FormField className="col-span-3" path="serviceItem.vatPercentagePerUnit" displayName="Thuế VAT">
          <div className="form-input-group">
            <NumberInput
              className={joinClassName(
                isServiceItemValidated && !isServiceItemVatPercentagePerUnitValid && "is-invalid"
              )}
              value={serviceItemModel.vatPercentagePerUnit}
              onValueChanged={(vatPercentagePerUnit) => {
                setServiceItemModel(m => ({ ...m, vatPercentagePerUnit }));
              }}
            />
            <span className="form-input-group-text">%</span>
          </div>

          {isServiceItemValidated && !isServiceItemVatPercentagePerUnitValid && (
            <span className="text-red-600 dark:text-red-400 text-sm">
              Đơn giá dịch vụ phải có giá trị ít nhất là 1000đ.
            </span>
          )}
        </FormField>

        <div className="flex justify-end mt-4 col-span-6">
          <button type="button" className="btn gap-1.5" onClick={handleServicePicked}>
            <CheckIcon />
            <span>Thêm vào giỏ hàng</span>
          </button>
        </div>
      </div>
    </div>
  );
}
