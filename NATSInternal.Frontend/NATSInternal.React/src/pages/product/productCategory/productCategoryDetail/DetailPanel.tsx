import React from "react";

// Child components.
import { BuildingStorefrontIcon } from "@heroicons/react/24/outline";

// Props.
type Props = {
  model: ProductCategoryDetailModel;
};

// Component.
export default function DetailPanel(props: Props): React.ReactNode {
  // Template.
  return (
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">
          Chi tiết thương hiệu
        </span>
      </div>

      <div className="panel-body flex flex-col p-3 gap-3">
        <div className="flex gap-3 justify-start items-start">
          <div className="img-thumbnail size-14 flex justify-center items-center">
            <BuildingStorefrontIcon className="size-7 opacity-50" />
          </div>

          <div className="flex flex-col justify-start items-start">
            <div className="text-2xl text-blue-700 dark:text-blue-400">
              {props.model.name}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}