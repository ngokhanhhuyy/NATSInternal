import React from "react";
import { Link } from "react-router";
import { getDisplayName } from "@/metadata";
import { joinClassName, getTextClassNameBasedOnDebtAmount } from "@/helpers";

// Child components.
import DebtAlert from "@/pages/shared/alerts/DebtAlert";
import { ClockIcon, CurrencyDollarIcon, UserIcon, ShoppingCartIcon } from "@heroicons/react/24/outline";
import { CheckCircleIcon, ExclamationTriangleIcon } from "@heroicons/react/24/outline";

// Props.
type OrderItemProps = {
  model: OrderBasicModel;
  hideIcon?: boolean;
  hideCustomer?: boolean;
};

// Components.
export default function OrderItem(props: OrderItemProps): React.ReactNode {
  // Template.
  const renderIcon = () => {
    const iconClassName = getTextClassNameBasedOnDebtAmount(props.model.debtAmount, {
      noDebtClassName: "text-emerald-600 dark:text-emerald-400"
    });

    if (props.model.debtAmount !== 0) {
      return <ExclamationTriangleIcon className={joinClassName("size-6", iconClassName)} />;
    }

    return <CheckCircleIcon className={joinClassName("size-6", iconClassName)} />;
  };

  // Template.
  const customerLink = (
    <Link className="text-blue-700 dark:text-blue-400" to={props.model.customer.detailRoutePath}>
      {props.model.customer.fullName}
    </Link>
  );

  return (
    <li className="list-group-item items-center px-3 py-1.5">
      <div className="grid grid-cols-[auto_1fr] gap-3 items-start">
        <div className="flex gap-3 items-center">
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
        </div>

        <div className="grid grid-cols-1 md:grid-cols-[1.3fr_1fr] lg:grid-cols-2 gap-x-3">
          <div className="flex flex-col">
            <div className="flex gap-x-2 justify-start items-center">
              <Link
                to={props.model.detailRoutePath}
                className={getTextClassNameBasedOnDebtAmount(props.model.debtAmount)}
              >
                <span className="font-bold">#{props.model.id} {getDisplayName(props.model.type)}</span>
              </Link>
              
              <DebtAlert className="alert-sm" debtAmount={props.model.debtAmount} />
            </div>

            <div className="block md:hidden text-sm">
              <span className="opacity-50">Mua bởi</span> {customerLink}&nbsp;
              <span className="opacity-50">với giá</span> {props.model.displayAmountAfterVat}&nbsp;
              <span className="opacity-50">vào</span> {props.model.displayStatsDate.toLowerCase()}
            </div>
            
            <div className="hidden md:flex items-center gap-1">
              <CurrencyDollarIcon className="size-5 opacity-50" />
              <span className="opacity-50">{props.model.displayAmountAfterVat}</span>
            </div>
          </div>
          

          <div className="hidden md:flex flex-col gap-x-10 w-fit">
            <div className="flex items-center gap-1">
              <ClockIcon className="size-5 opacity-50" />
              <span className="opacity-50">{props.model.displayStatsDate}</span>
            </div>
            
            {!props.hideCustomer && (
              <div className="flex items-center gap-1">
                <UserIcon className="size-5 opacity-50" />
                {customerLink}
              </div>
            )}
          </div>
        </div>
      </div>
    </li>
  );
}
