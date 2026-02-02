import React from "react";
import { Link } from "react-router";
import { useTsxHelper } from "@/helpers";

// Child components.
import {
  BuildingStorefrontIcon, CheckCircleIcon,
  ExclamationCircleIcon,
  ExclamationTriangleIcon, MinusCircleIcon,
  TagIcon
} from "@heroicons/react/24/outline";

// Props.
type ResultsPanelProps = {
  model: ProductListModel;
  isReloading: boolean;
};

// Components.
export default function ResultsPanel(props: ResultsPanelProps): React.ReactNode {
  // Dependencies.
  const { joinClassName } = useTsxHelper();

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
          {props.model.items.length > 0 ? props.model.items.map((product, index) => (
            <ResultItem model={product} key={index} />
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

function ResultItem(props: { model: ProductListProductModel }): React.ReactNode {
  // Dependencies.
  const { joinClassName, compute } = useTsxHelper();

  // Computed.
  const alertClassName = compute<string>(() => {
    if (props.model.stockingQuantity === 0) {
      return "alert-red-outline dark:alert-red";
    }

    if (props.model.isResupplyNeeded) {
      return "alert-yellow-outline dark:alert-yellow";
    }

    if (props.model.isDiscontinued) {
      return "alert-neutral-outline dark:alert-neutral";
    }

    return "alert-emerald-outline dark:alert-emerald";
  });

  // Template.
  const Icon = () => {
    if (props.model.stockingQuantity === 0) {
      return <ExclamationCircleIcon className="text-red-600 dark:text-red-400 size-6" />;
    }

    if (props.model.isResupplyNeeded) {
      return <ExclamationTriangleIcon className="text-yellow-600 dark:text-yellow-400 size-6" />;
    }

    if (props.model.isDiscontinued) {
      return <MinusCircleIcon className="text-neutral-600 dark:text-neutral-400 size-6" />;
    }

    return <CheckCircleIcon className="text-emerald-600 dark:text-emerald-400 size-6" />;
  };

  return (
    <li className="list-group-item grid grid-cols-[auto_auto_1fr] items-center gap-3 px-3 py-1.5">
      <Icon />

      <img src={props.model.thumbnailUrl} className="img-thumbnail size-12" alt={props.model.name} />

      <div className="flex flex-col self-start">
        <div className="flex gap-3 items-center">
          <Link
            to={props.model.detailRoute}
            className={joinClassName(
              "font-bold",
              !props.model.isDiscontinued && "text-blue-700 dark:text-blue-400"
            )}
          >
            {props.model.name}
          </Link>

          <div className={joinClassName("alert dark:font-bold dark:alert-sm min-w-8 text-center", alertClassName)}>
            {props.model.stockingQuantity}
          </div>
        </div>

        <div className="text-sm">
          {props.model.brand && (
            <div className="flex justify-start items-center gap-1">
              <BuildingStorefrontIcon className="size-4" />
              <span>{props.model.brand.name}</span>
            </div>
          )}

          {props.model.category && (
            <div className="flex justify-start items-center gap-1">
              <TagIcon className="size-4" />
              <span>{props.model.category.name}</span>
            </div>
          )}
        </div>
      </div>
    </li>
  );
}