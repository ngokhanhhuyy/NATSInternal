import React from "react";
import { Link } from "react-router";
import { joinClassName } from "@/helpers";

// Child components.
import { ArchiveBoxIcon, BeakerIcon } from "@heroicons/react/24/outline";

// Props.
type ItemProps = {
  model: OrderProductItemDetailModel | OrderServiceItemDetailModel;
  index: number;
};

// Components.
export default function Item(props: ItemProps): React.ReactNode {
  // Template.
  const renderIcon = () => {
    if (isProduct(props.model)) {
      if (props.model.product.thumbnailUrl) {
        return (
          <img
            src={props.model.product.thumbnailUrl}
            className="img-thumbnail size-12"
            alt={props.model.product.name}
          />
        );
      }

      return (
        <div className="img-thumbnail size-12 flex justify-center items-center">
          <ArchiveBoxIcon className="size-6 opacity-50" />
        </div>
      );
    }

    return (
      <div className="img-thumbnail size-12 flex justify-center items-center">
        <BeakerIcon className="size-6 opacity-50" />
      </div>
    );
  };

  return (
    <li className="list-group-item grid grid-cols-[auto_1fr] gap-3 p-2">
      {renderIcon()}

      <div className="flex flex-col justify-start min-w-0">
        <div className="font-bold min-w-0">
          <div className={joinClassName(
            "w-fit",
            isProduct(props.model)
              ? "text-blue-700 dark:text-blue-400"
              : "text-emerald-700 dark:text-emerald-400"
          )}>
            <span className={joinClassName(
              "alert alert-sm me-2 py-0 -translate-y-px inline-flex",
              isProduct(props.model) ? "alert-blue-outline dark:alert-blue" : "alert-emerald-outline dark:alert-emerald"
            )}>
              {isProduct(props.model) ? "SP" : "DV"}
            </span>
            
            <span className="me-1">{props.index + 1}.</span>

            {isProduct(props.model) ? (
              <Link
                to={props.model.product.detailRoutePath}
                className="overflow-hidden whitespace-nowrap text-ellipsis min-w-0"
              >
                {props.model.product.name}
              </Link>
            ) : (
              <span className="min-w-0">
                {props.model.name}
              </span>
            )}
          </div>
        </div>

        <div className="grid grid-cols-[2fr_2fr_0.75fr_2fr] justify-start items-center gap-5 max-w-100">
          <span className="opacity-50">{props.model.displayAmountBeforeVatPerUnit}</span>
          <span className="opacity-50">+{props.model.vatPercentagePerUnit}% VAT</span>
          <span className="opacity-50">×{props.model.quantity}</span>
          <span className="font-bold opacity-50">
            ＝ {props.model.displayAmountAfterVat}
          </span>
        </div>
      </div>
    </li>
  );
}

type OrderItemModel = OrderProductItemDetailModel | OrderServiceItemDetailModel;
function isProduct(model: OrderItemModel): model is OrderProductItemDetailModel {
  return Object.hasOwn(model, "product");
}
