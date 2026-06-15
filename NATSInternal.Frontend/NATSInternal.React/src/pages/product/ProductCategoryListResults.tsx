import React from "react";
import { Link } from "react-router";
import { joinClassName } from "@/helpers";

// Child components.
import { TagIcon } from "@heroicons/react/24/outline";

// Props.
type ProductCategoryListResultsProps = {
  model: ProductCategoryBasicModel[];
  showRemainingCount?: boolean;
  className?: string;
};

// Components.
export default function ProductCategoryListResults(props: ProductCategoryListResultsProps): React.ReactNode {
  // Template.
  return (
    <ul className={joinClassName("list-group", props.className)}>
      {props.model.map((category) => (
        <li className="list-group-item grid grid-cols-[auto_1fr] gap-3 p-2" key={category.id}>
          <div className="img-thumbnail flex justify-center items-center size-12">
            <TagIcon className="opacity-50 size-6" />
          </div>

          <div className="flex flex-col justify-start items-start min-w-0">
            <Link
              to={category.detailRoutePath}
              className={joinClassName(
                "text-blue-700 dark:text-blue-400 font-bold w-full",
                "overflow-hidden text-ellipsis whitespace-nowrap"
              )}
            >
              {category.name}
            </Link>

            <span className="opacity-50">
              {category.productCount} sản phẩm
            </span>
          </div>
        </li>
      ))}
    </ul>
  );
}
