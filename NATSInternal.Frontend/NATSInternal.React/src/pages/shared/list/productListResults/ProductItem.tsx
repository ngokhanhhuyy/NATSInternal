import React from "react";
import { Link } from "react-router";
import { joinClassName, compute } from "@/helpers";

// Child components.
import { NewTabLink } from "@/components/ui";
import { CheckCircleIcon, ExclamationCircleIcon } from "@heroicons/react/24/outline";
import { ExclamationTriangleIcon, MinusCircleIcon, TagIcon, ArchiveBoxIcon } from "@heroicons/react/24/outline";

// Props.
type ProductItemProps = {
  model: ProductBasicModel;
  label?: React.ReactNode;
  children?: React.ReactNode;
  openLinkInNewTab?: boolean;
  hideStatusIcon?: boolean;
};

// Components.
export default function ProductItem(props: ProductItemProps): React.ReactNode {
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
  const renderIcon = () => {
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
    <li className={joinClassName(
      "list-group-item grid items-center gap-3 px-3 py-1.5",
      !props.hideStatusIcon ? "grid-cols-[auto_auto_1fr_auto]" : "grid-cols-[auto_1fr_auto] ps-2"
    )}>
      {!props.hideStatusIcon && renderIcon()}

      {props.model.thumbnailUrl ? (
        <img src={props.model.thumbnailUrl} className="img-thumbnail size-12" alt={props.model.name} />
      ) : (
        <div className="img-thumbnail size-12 flex justify-center items-center">
          <ArchiveBoxIcon className="size-6 opacity-50" />
        </div>
      )}

      <div className="flex flex-col self-start">
        <div className="flex gap-3 items-center">
          {props.openLinkInNewTab ? (
            <NewTabLink
              url={props.model.detailRoutePath}
              className={joinClassName(
                "font-bold",
                !props.model.isDiscontinued && "text-blue-700 dark:text-blue-400"
              )}
            >
              {props.model.name}
            </NewTabLink>
          ) : (
            <Link
              to={props.model.detailRoutePath}
              className={joinClassName(
                "font-bold",
                !props.model.isDiscontinued && "text-blue-700 dark:text-blue-400"
              )}
            >
              {props.model.name}
            </Link>
          )}

          <div className={joinClassName("alert dark:font-bold dark:alert-sm min-w-8 text-center", alertClassName)}>
            {props.model.stockingQuantity}
          </div>

          {props.label}
        </div>

        <div className="flex flex-wrap gap-3 text-sm">
          {props.model.categories.map((category, index) => (
            <div className="flex justify-start items-center gap-1" key={index}>
              <TagIcon className="size-4" />
              <span>{category.name}</span>
            </div>
          ))}
        </div>
      </div>
      
      {props.children}
    </li>
  );
}
