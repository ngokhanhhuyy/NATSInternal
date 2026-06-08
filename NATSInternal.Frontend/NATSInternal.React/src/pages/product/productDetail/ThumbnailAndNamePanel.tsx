import React from "react";

// Child components.
import { ArchiveBoxIcon } from "@heroicons/react/24/outline";

// Props.
type ThumbnailAndNamePanelProps = {
  model: ProductDetailModel;
};

// Components.
export default function ThumbnailAndNamePanel(props: ThumbnailAndNamePanelProps): React.ReactNode {
  // Template.
  return (
    <div className="panel">
      <div className="panel-body p-3">
        <div className="grid grid-cols-[auto_1fr] gap-3">
          {/* Avatar */}
          <div className="img-thumbnail size-16 flex justify-center items-center">
            <ArchiveBoxIcon className="size-8 opacity-50" />
          </div>

          {/* Names */}
          <div className="flex flex-col justify-start pt-1">
            <span className="text-blue-600 dark:text-blue-400 text-2xl">
              {props.model.name}
            </span>

            <div className="flex flex-wrap gap-2">
              <TargetOrderTypeAlert isForRetail={props.model.isForRetail}  />
              <StatusAlert model={props.model} />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

function TargetOrderTypeAlert(props: { isForRetail: boolean }): React.ReactNode {
  if (props.isForRetail) {
    return (
      <div className="alert alert-emerald-outline dark:alert-emerald dark:font-bold">
        Cả liệu trình và bán lẻ
      </div>
    );
  }

  return (
    <div className="alert alert-blue-outline dark:alert-blue dark:font-bold">
      Chỉ liệu trình
    </div>
  );
}


function StatusAlert(props: { model: ProductDetailModel }): React.ReactNode {
  if (props.model.isDiscontinued) {
    return (
      <div className="alert alert-neutral-outline dark:alert-neutral dark:font-bold alert-sm">
        Đã ngưng kinh doanh
      </div>
    );
  }
  
  if (props.model.stockingQuantity === 0) {
    return (
      <div className="alert alert-red-outline dark:alert-red dark:font-bold alert-sm">
        Đã hết hàng
      </div>
    );
  }

  const resupplyThresholdQuantity = props.model.resupplyThresholdQuantity;
  if (resupplyThresholdQuantity != null && props.model.stockingQuantity <= resupplyThresholdQuantity) {
    return (
      <div className="alert alert-yellow-outline dark:alert-yellow dark:font-bold alert-sm">
        Sắp hết hàng
      </div>
    );
  }
}
