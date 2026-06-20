import React, { useState, useEffect } from "react";
import { api, type IApi } from "@/api";
import { createCountModel } from "@/models";
import { getDisplayName } from "@/metadata";
import { compute, joinClassName } from "@/helpers";

// Child components.
import { ArrowLongRightIcon, ArrowTrendingDownIcon, ArrowTrendingUpIcon } from "@heroicons/react/24/outline";

// Props.
type CountByCriteriaPanelProps = {
  orderType: OrderType;
  unit: string;
  format?(count: number): string;
  getCountAsync(api: IApi, requestDto: CountRequestDto): Promise<CountResponseDto>;
};

// Components.
export default function CountByCriteria(props: CountByCriteriaPanelProps): React.ReactNode {
  // States.
  const [isInitialRendering, setIsInitialRendering] = useState<boolean>(true);
  const [model, setModel] = useState<CountModel>(() => {
    const m = createCountModel();
    m.timeRangeUnitType = "Day";
    m.timeRangeUnitCount = 7;
    return m;
  });

  // Computed.
  const title = compute<string>(() => {
    return (
      `${getDisplayName(props.orderType)} trong ${model.timeRangeUnitCount} ` +
      `${getDisplayName(model.timeRangeUnitType)} gần nhất`
    );
  });

  const percentageTextColor = compute<string>(() => {
    if (model.percentageComparedToPreviousTimeRange === 0) {
      return "opacity-50";
    }

    if (model.percentageComparedToPreviousTimeRange > 0) {
      return "text-emerald-700 dark:text-emerald-400";
    }

    return "text-red-700 dark:text-red-400";
  });

  // Effect.
  useEffect(() => {
    const loadAsync = async () => {
      try {
        const responseDto = await props.getCountAsync(api, model.toRequestDto());
        setModel(m => m.mapFromResponseDto(responseDto));
      } finally {
        setIsInitialRendering(false);
      }
    };

    loadAsync().then(() => { });
  }, []);

  // Template.
  function renderIcon(): React.ReactNode {
    const className = "size-4";
    if (model.percentageComparedToPreviousTimeRange === 0) {
      return <ArrowLongRightIcon className={className} />;
    }

    if (model.percentageComparedToPreviousTimeRange < 0) {
      return <ArrowTrendingDownIcon className={className} />;
    }

    return <ArrowTrendingUpIcon className={className} />;
  }

  return (
    <div className="panel">
      <div className="panel-header">
        <div className="panel-header-title min-w-0 overflow-hidden text-ellipsis whitespace-nowrap">
          {title}
        </div>
      </div>

      <div className="panel-body p-3">
        {isInitialRendering ? (
          <div className="panel-body-area px-3 py-5 flex justify-center items-center">
            <span className="opacity-50">Đang tải ...</span>
          </div>
        ) : (
          <div className="panel-body-area grid grid-cols-[1fr_auto] gap-1 p-3">
            <div className="flex flex-col">
              <div className={joinClassName("flex gap-2 items-center", percentageTextColor)}>
                <span>
                  {model.percentageComparedToPreviousTimeRange >= 0 && "+"}
                  {model.percentageComparedToPreviousTimeRange}%
                </span>
                {renderIcon()} 
              </div>
              <span className="opacity-50 text-sm">So với kì trước</span>
            </div>

            <div className="flex jusitfy-end items-end gap-1">
              <span className="text-blue-700 dark:text-blue-400 text-4xl">
                {props.format?.(model.currentTimeRangeCount) ?? model.currentTimeRangeCount}
              </span>
              <span className="text-lg">{props.unit}</span>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
