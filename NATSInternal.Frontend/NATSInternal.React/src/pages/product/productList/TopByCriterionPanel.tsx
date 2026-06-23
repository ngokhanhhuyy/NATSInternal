import React, { useState, useEffect } from "react";
import { Link } from "react-router";
import { api, type IApi } from "@/api";
import { createTopModel, createProductBasicModel } from "@/models";
import { getDisplayName } from "@/metadata";
import { compute, joinClassName, getDisplayAmountText } from "@/helpers";

// Props.
type TopPanelProps = {
  getTopAsync(api: IApi, requestDto: TopOverTimeRangeRequestDto): Promise<TopOverTimeRangeResponseDto<ProductBasicResponseDto, number>>;
  criterion: "SoldQuantity" | "Revenue";
};

// Components.
export default function TopByCriterionPanel(props: TopPanelProps): React.ReactNode {
  // States.
  const [isInitialRendering, setIsInitialRendering] = useState<boolean>(true);
  const [model, setModel] = useState<TopModel<ProductBasicResponseDto, ProductBasicModel, number>>(() => {
    return createTopModel<ProductBasicResponseDto, ProductBasicModel, number>(createProductBasicModel);
  });

  // Computed.
  const title = compute<string>(() => {
    let name = `Top ${model.resultsCount} `;
    if (props.criterion === "SoldQuantity") {
      name += "bán chạy";
    } else {
      name += "doanh thu cao";
    }

    name += ` ${model.timeRangeUnitCount} ${getDisplayName(model.timeRangeUnitType)} gần nhất`;
    return name;
  });

  function computeDisplayMetric(item: TopItemModel<ProductBasicModel, number>): string {
    if (props.criterion === "SoldQuantity") {
      return `${item.metric} ${item.item.unit}`;
    }

    return getDisplayAmountText(item.metric, { suffix: " vnđ" });
  };

  // Effect.
  useEffect(() => {
    const loadAsync = async () => {
      try {
        const responseDto = await props.getTopAsync(api, model.toRequestDto());
        setModel(m => m.mapFromResponseDto(responseDto, createProductBasicModel));
      } finally {
        setIsInitialRendering(false);
      }
    };

    loadAsync();
  }, []);

  // Template.
  return (
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">{title}</span>
      </div>

      <div className="panel-body p-3">
        <div className="panel-body-area">
          {isInitialRendering ? (
            <div className="flex justify-center items-center px-3 py-10 opacity-50">
              Đang tải ...
            </div>
          ) : (
            <ul className="list-group list-group-flush">
              {(isInitialRendering || !model.items.length) ? (
                <li className="list-group-item flex justify-center items-center px-3 py-10">
                  <span className="opacity-50">
                    {isInitialRendering ? "Đang tải ..." : "Không có sản phẩm nào"}
                  </span>
                </li>
              ) : Array.from({ length: model.resultsCount }).map((_, index) => (
                <li className="list-group-item px-2 py-1 flex flex-col justify-center items-start min-w-0" key={index}>
                  {model.items[index] ? (
                    <Link
                      className={joinClassName(
                        "text-blue-700 dark:text-blue-400 font-bold",
                        "overflow-hidden text-ellipsis whitespace-nowrap w-full"
                      )}
                      to={model.items[index].item.detailRoutePath}
                    >
                      {model.items[index].item.name}
                    </Link>
                  ) : (
                    <span className="opacity-50">{index}</span>
                  )}

                  <span className={joinClassName("text-sm", model.items[index] ? "opacity-50" : "opacity-0")}>
                    {computeDisplayMetric(model.items[index])}
                  </span>
                </li>
              ))}
            </ul>
          )}
        </div>
      </div>
    </div>
  );
}
