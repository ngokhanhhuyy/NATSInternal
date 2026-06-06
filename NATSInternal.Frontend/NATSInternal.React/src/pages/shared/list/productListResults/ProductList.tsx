import React from "react";
import { joinClassName } from "@/helpers";

// Child components.
import ProductItem from "./ProductItem";

// Props.
type ProductListProps = {
  model: ProductListModel;
  className?: string;
  renderItemLabel?(product: ProductBasicModel): React.ReactNode;
  renderItemButton?(product: ProductBasicModel): React.ReactNode;
  openLinkInNewTab?: boolean;
  hideStatusIcon?: boolean;
};

// Components.
export default function ProductList(props: ProductListProps): React.ReactNode {
  // Templates.
  return (
    <ul className={joinClassName("list-group", props.className)}>
      {props.model.items.length > 0 ? props.model.items.map((product, index) => (
        <ProductItem
          model={product}
          label={props.renderItemLabel?.(product)}
          openLinkInNewTab={props.openLinkInNewTab}
          hideStatusIcon={props.hideStatusIcon}
          key={index}
        >
          {props.renderItemButton?.(product)}
        </ProductItem>
      )) : (
        <li className="list-group-item opacity-50 px-3 py-10 flex justify-center">
          Không có kết quả
        </li>
      )}
    </ul>
  );
}
