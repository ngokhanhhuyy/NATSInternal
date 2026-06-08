import React from "react";
import { Link } from "react-router";
import { getDisplayName } from "@/metadata";
import { joinClassName, compute } from "@/helpers";

// Child components.
import { ClockIcon, CurrencyDollarIcon, TagIcon, ShoppingCartIcon } from "@heroicons/react/24/outline";
import { CheckCircleIcon, ExclamationTriangleIcon } from "@heroicons/react/24/outline";

// Props.
type OrderItemProps = {
  model: OrderBasicModel;
  children?: React.ReactNode;
  hideIcon?: boolean;
};

// Components.
export default function OrderItem(props: OrderItemProps): React.ReactNode {
  // Computed.
  const className = compute<string>(() => {
    if (props.hideIcon) {
      if (props.children) {
        return "grid-cols-[auto_2fr_1fr]";
      } else {
        return "grid-cols-[auto_1fr]";
      }
    } else {
      if (props.children) {
        return "grid-cols-[auto_auto_2fr_1fr]";
      } else {
        return "grid-cols-[auto_auto_1fr]";
      }
    }
  });

  // Template.
  const renderIcon = () => {
    if (props.model.isDebtOrder) {
      return <ExclamationTriangleIcon className="text-yellow-600 dark:text-yellow-400 size-6" />;
    }

    return <CheckCircleIcon className="text-emerald-600 dark:text-emerald-400 size-6" />;
  };

  // Template.
  return (
    <li className="list-group-item items-center px-3 py-1.5">
      <div className={joinClassName("grid gap-3", className)}>
        <div className="flex items-center">
          {!props.hideIcon && renderIcon()}
        </div>

        {props.model.thumbnailUrl ? (
          <img
            src={props.model.thumbnailUrl}
            className="img-thumbnail size-12"
            alt={`#${props.model.id.toString()} ${getDisplayName(props.model.type)}`}
          />
        ) : (
          <div className="img-thumbnail size-12 flex justify-center items-center">
            <ShoppingCartIcon className="size-6 opacity-50" />
          </div>
        )}

        <div className={joinClassName(
          "grid grid-cols-1 gap-x-3",
          props.children ? "md:grid-cols-[1.3fr_1fr]" : "md:grid-cols-[1fr_2fr]"
        )}>
          <div className="flex flex-col">
            <div className="flex gap-3 justify-start items-center">
              <Link
                to={props.model.detailRoutePath}
                className={joinClassName(
                  "font-bold",
                  props.model.isDebtOrder
                    ? "text-yellow-600 dark:text-yellow-400"
                    : "text-blue-700 dark:text-blue-400"
                )}
              >
                #{props.model.id}
              </Link>
              
              {props.model.isDebtOrder && (
                <span className="alert alert-sm alert-yellow-outline dark:alert-yellow font-bold px-2">
                  Nợ
                </span>
              )}
            </div>
            
            <div className="flex items-center gap-1 opacity-50">
              <TagIcon className="size-4" />
              <span>{getDisplayName(props.model.type)}</span>
            </div>
          </div>
          

          <div className="flex flex-col gap-x-10 w-fit">
            <div className="flex items-center gap-1">
              <ClockIcon className="size-5 opacity-50" />
              <span className="opacity-50">{props.model.displayStatsDate}</span>
            </div>
            
            <div className="flex items-center gap-1">
              <CurrencyDollarIcon className="size-5 opacity-50" />
              <span className="opacity-50">{props.model.displayAmountAfterVat}</span>
            </div>
          </div>
        </div>

        {props.children}
      </div>
    </li>
  );
}
