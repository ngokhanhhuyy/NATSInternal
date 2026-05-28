import React from "react";
import { joinClassName } from "@/helpers";

// Child components.
import ProductList from "@/pages/shared/list/productListResults";

// Props.
type ResultsPanelProps = {
  model: ProductListModel;
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

      <div className={joinClassName("panel-body", props.isReloading && "opacity-50")}>
        <ProductList model={props.model} className="list-group-flush" />
      </div>
    </div>
  );
}
