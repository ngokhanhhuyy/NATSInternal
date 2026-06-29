import React from "react";
import { Link } from "react-router";

// Child components.
import { ArchiveBoxArrowDownIcon } from "@heroicons/react/24/outline";

// Props.
type SupplyItemProps = {
  model: SupplyBasicModel;
  hideIcon?: boolean;
};

// Components.
export default function SupplyItem(props: SupplyItemProps): React.ReactNode {
  return (
    <li className="list-group-item items-center px-3 py-1.5">
      <div className="grid grid-cols-[auto_1fr] gap-3 items-start">
        {props.model.thumbnailUrl ? (
          <img
            src={props.model.thumbnailUrl}
            className="img-thumbnail size-12"
            alt={`#${props.model.id.toString()}`}
          />
        ) : (
          <div className="img-thumbnail size-12 flex justify-center items-center">
            <ArchiveBoxArrowDownIcon className="size-6 opacity-50" />
          </div>
        )}

        <div className="grid grid-cols-1 md:grid-cols-[1.3fr_1fr] lg:grid-cols-2 gap-x-3">
          <div className="flex flex-col">
            <div className="flex gap-x-2 justify-start items-center">
              <Link to={props.model.detailRoutePath} className="text-blue-700 dark:text-blue-400">
                <span className="font-bold">#{props.model.id}</span>
              </Link>
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
            
            <div className="flex items-center gap-1">
              <UserIcon className="size-5 opacity-50" />
              {customerLink}
            </div>
          </div>
        </div>
      </div>
    </li>
  );
}
