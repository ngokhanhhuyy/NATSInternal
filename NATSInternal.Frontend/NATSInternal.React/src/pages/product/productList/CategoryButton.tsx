import React from "react";
import { joinClassName } from "@/helpers";

// Props.
type CategoryButtonProps = {
  model: ProductCategoryBasicModel | null;
  isSelected: boolean;
  onClick(model: ProductCategoryBasicModel | null): any;
};

// Components.
export default function CategoryButton(props: CategoryButtonProps): React.ReactNode {
  // Template.
  return (
    <button
      type="button"
      className={joinClassName(
        "btn btn-sm",
        props.isSelected && "border-blue-700 text-blue-700 dark:border-blue-400 dark:text-blue-400",
        props.isSelected && "outline outline-blue-700 dark:outline-blue-400 shadow"
      )}
      onClick={() => props.onClick(props.model)}
    >
      {props.model?.name ?? "Tất cả phân loại"}
    </button>
  );
}
