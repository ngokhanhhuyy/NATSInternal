import React, { useState } from "react";
import { compute, joinClassName } from "@/helpers";
import style from "./ItemTablePanel.module.css";

// Child components.
import { NumberInput } from "@/components/form";
import { PencilSquareIcon, XMarkIcon } from "@heroicons/react/24/outline";

// Props.
type ItemListPanelProps = {
  model: OrderUpsertModel;
  onModelUpdated?(updatedData: Partial<OrderUpsertModel>): any;
};

// Components.
export default function ItemTablePanel(props: ItemListPanelProps): React.ReactNode {
  // Computed.
  const isEmpty = compute(() => props.model.productItems.length + props.model.serviceItems.length === 0);

  // Template.
  return (
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">
          Sản phẩm và dịch vụ đã chọn
        </span>
      </div>

      <div className="panel-body">
        {!isEmpty ? (
          <table className={joinClassName("data-table w-full table-fixed", style.dataTable)}>
            <colgroup>
              <col className="w-12" />
              <col className="w-1/2" />
              <col className="w-25" />
              <col />
              <col />
              <col className="w-25" />
              <col className="w-10" />
            </colgroup>

            <thead>
              <tr>
                <th>STT</th>
                <th>Tên</th>
                <th>Loại</th>
                <th>Đơn giá</th>
                <th>VAT</th>
                <th>Số lượng</th>
                <th></th>
              </tr>
            </thead>
            
            <tbody>
              {props.model.productItems.map((productItem, index) => (
                <ItemRow
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
                <ItemRow
                  index={index + props.model.productItems.length}
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
            </tbody>
          </table>
        ) : (
          <div className="flex justify-center items-center px-5 py-20 opacity-50">
            Chưa chọn sản phẩm và dịch vụ
          </div>
        )}
      </div>
    </div>
  );
}

type OrderItemUpsertModel = OrderProductItemUpsertModel | OrderServiceItemUpsertModel;
type ItemRowProps<T extends OrderItemUpsertModel> = {
  index: number;
  model: T;
  onModelUpdated(updatedData: Partial<T>): any;
  onModelDeleted(): any;
};

function ItemRow<T extends OrderItemUpsertModel>(props: ItemRowProps<T>): React.ReactNode {
  // Template.
  return (
    <tr>
      <td>{props.index + 1}</td>
      
      {isOrderProductItemUpsertModel(props.model) ? (
        <>
          <td className="overflow-hidden whitespace-nowrap text-ellipsis">{props.model.product.name}</td>
          <td className="text-center">Sản phẩm</td>
        </>
      ) : (
        <>
          <td className="overflow-hidden whitespace-nowrap text-ellipsis">{props.model.name}</td>
          <td className="text-center">Dịch vụ</td>
        </>
      )}
      
      <td>
        <EditableTableCell content={props.model.displayAmountBeforeVatPerUnit} render={(switchToViewMode) => (
          <NumberInput
            value={props.model.amountBeforeVatPerUnit}
            onValueChanged={(amountBeforeVatPerUnit) => props.onModelUpdated({ amountBeforeVatPerUnit } as Partial<T>)}
            onBlur={switchToViewMode}
            autoFocus
          />
        )}/>
      </td>

      <td>
        <EditableTableCell content={`${props.model.vatPercentagePerUnit}%`} render={(switchToViewMode) => (
          <NumberInput
            value={props.model.vatPercentagePerUnit}
            onValueChanged={(vatPercentagePerUnit) => props.onModelUpdated({ vatPercentagePerUnit } as Partial<T>)}
            min={0}
            max={200}
            onBlur={switchToViewMode}
            autoFocus
          />
        )}/>
      </td>

      <td>
        <EditableTableCell content={props.model.quantity} render={(switchToViewMode) => (
          <NumberInput
            value={props.model.quantity}
            onValueChanged={(quantity) => props.onModelUpdated({ quantity } as Partial<T>)}
            min={1}
            max={100}
            onBlur={switchToViewMode}
            autoFocus
          />
        )}/>
      </td>

      <td>
        <div className="flex justify-center items-center">
          <button type="button" className="btn btn-danger btn-sm" onClick={props.onModelDeleted}>
            <XMarkIcon />
          </button>
        </div>
      </td>
    </tr>
  );
}

type EditableTableCellProps = {
  content: string | number;
  render(switchToViewMode: () => void): React.ReactNode;
};

function EditableTableCell(props: EditableTableCellProps): React.ReactNode {
  // States.
  const [mode, setMode] = useState<"view" | "edit">("view");

  // Template.
  if (mode === "view") {
    return (
      <div className="flex gap-2 justify-end items-center" onClick={() => setMode("edit")}>
        <span>{props.content}</span>
        <PencilSquareIcon className="size-3.5 opacity-50" />
      </div>
    );
  }

  return props.render(() => setMode("view"));
}

function isOrderProductItemUpsertModel(model: OrderItemUpsertModel): model is OrderProductItemUpsertModel {
  return Object.hasOwn(model, "product");
}
