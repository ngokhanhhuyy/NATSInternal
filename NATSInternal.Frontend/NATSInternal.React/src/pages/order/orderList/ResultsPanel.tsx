import React from "react";
import { Link } from "react-router";
import { getDisplayName } from "@/metadata";
import { joinClassName, compute } from "@/helpers";

// Child components.
import { ShoppingCartIcon } from "@heroicons/react/24/outline";

// Props.
type ResultsPanelProps = {
  model: OrderListModel;
  isReloading: boolean;
};

// Components.
export default function ResultsPanel(props: ResultsPanelProps): React.ReactNode {
  // Templates.
  return (
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">
          Danh sách kết quả
        </span>
      </div>

      <div className={joinClassName("panel-body transition-opacity", props.isReloading && "opacity-50")}>
        <ul className="list-group list-group-flush">
          {props.model.items.length > 0 ? props.model.items.map((order, index) => (
            <ResultItem model={order} key={index} />
          )) : (
            <li className="list-group-item opacity-50 px-3 py-10">
              Không có kết quả
            </li>
          )}
        </ul>
      </div>
    </div>
  );
}

function ResultItem(props: { model: OrderBasicModel }): React.ReactNode {
  // Computed.
  const displayId = compute<string>(() => {
    return `#${props.model.id.toString()} ${getDisplayName(props.model.type)}`;
  });

  // Template.
  return (
    <li className="list-group-item items-center px-3 py-1.5">
      <div className="grid grid-cols-[auto_1fr_auto] gap-3">
        {props.model.thumbnailUrl ? (
          <img
            src={props.model.thumbnailUrl}
            className="img-thumbnail size-12"
            alt={displayId}
          />
        ) : (
          <div className="img-thumbnail size-12 flex justify-center items-center">
            <ShoppingCartIcon className="size-6 opacity-50" />
          </div>
        )}

        <div className="flex flex-col">
          <Link to={props.model.detailRoutePath} className="font-bold text-blue-700 dark:text-blue-400">
            {displayId}
          </Link>

          <span className="opacity-50">{props.model.displayStatsDate}</span>
        </div>

        <div className="flex flex-col items-end">
          <Link className="text-blue-700 dark:text-blue-400" to={props.model.customer.detailRoutePath}>
            {props.model.customer.fullName}
          </Link>

          <span className="opacity-50">{props.model.displayAmountAfterVat}</span>
        </div>
      </div>
    </li>
  );
}
