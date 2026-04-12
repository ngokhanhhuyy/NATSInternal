import React from "react";
import { Link } from "react-router";
import { useTsxHelper } from "@/helpers";

// Child components.
import { BuildingStorefrontIcon } from "@heroicons/react/24/outline";

// Props.
type ResultsPanelProps = {
  model: BrandListModel;
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

      <div className={joinClassName("panel-body", props.isReloading && "opacity-50")}>
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

function ResultItem(props: { model: BrandListBrandModel }): React.ReactNode {
  // Template.
  return (
    <li className="list-group-item grid grid-cols-[auto_auto_1fr] items-center gap-3 px-3 py-1.5">
      <div className="img-thumbnail size-10 flex justify-center items-center">
        <BuildingStorefrontIcon className="size-6 opacity-50" />
      </div>
      <div className="flex flex-col justify-start items-start">
        <Link to={props.model.detailRoutePath} className="font-bold text-blue-700 dark:text-blue-400">
          {props.model.name}
        </Link>

        <span className="text-sm opacity-50">
          {props.model.productCount} sản phẩm
        </span>
      </div>
    </li>
  );
}