import React, { useState } from "react";
import { createOrderProductItemUpsertModel } from "@/models";

// Child components.
import ProductPicker from "@/pages/shared/upsert/productPicker";
import { FormField, NumberInput } from "@/components/form";

// Props.
type ItemListViewProps = {
  onOrderProductItemCreated?(orderProductItem: OrderProductItemUpsertModel): any;
};

// Components.
export default function ItemListView(_: ItemListViewProps): React.ReactNode {
  // Model.
  const [model, setModel] = useState<OrderProductItemUpsertModel | null>(null);

  // Templates.
  return (
    <div className="grid grid-cols-2 gap-3 w-full">
      <ProductPicker
        onProductPicked={(product) => setModel(() => createOrderProductItemUpsertModel(product))}
        renderView={(_) => model && (
          <div className="grid grid-cols-1 xl:grid-cols-2 items-start gap-3 p-3 pt-2 w-full">
            <FormField path="amountBeforeVatPerUnit" displayName="Giá sản phẩm mỗi đơn vị (trước VAT)">
              <div className="form-input-group">
                <NumberInput
                  value={model.amountBeforeVatPerUnit}
                  onValueChanged={(amountBeforeVatPerUnit) => setModel(m => ({ ...m!, amountBeforeVatPerUnit }))}
                />

                <span className="form-input-group-text border-s-0">vnđ</span>
              </div>
            </FormField>
          </div>
        )}
      />
    </div>
  );
}
