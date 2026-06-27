import React, { useState, useEffect, startTransition } from "react";
import { joinClassName, compute } from "@/helpers"; 

// Props.
export type CountPanelProps = {
  title: string;
  metricTextColor?: "yellow" | "red";
  getCountAsync(): Promise<number>;
};

// Components.
export default function CountPanel(props: CountPanelProps): React.ReactNode {
  // States.
  const [isInitialRendering, setIsInitialRendering] = useState<boolean>(true);
  const [model, setModel] = useState<number>(0);

  // Computed.
  const countClassName = compute<string>(() => {
    switch (props.metricTextColor) {
      case "yellow":
        return "text-yellow-600 dark:text-yellow-400";
      case "red":
        return "text-red-700 dark:text-red-400";
      default:
        return "text-blue-700 dark:text-blue-400";
    }
  });

  // Effect.
  useEffect(() => {
    startTransition(async () => {
      try {
        const count = await props.getCountAsync();
        setModel(count);
      } finally {
        setIsInitialRendering(false);
      }
    });
  }, []);

  // Templates.
  return (
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">
          {props.title}
        </span>
      </div>

      <div className="panel-body p-3">
        <div className={joinClassName(
          "panel-body-area p-3 flex",
          isInitialRendering ? "justify-center items-center px-3 py-7.5" : "justify-end items-end p-3 gap-1.5"
        )}>
          {isInitialRendering ? (
            <span className="opacity-50">Đang tải ...</span>
          ) : (
            <>
              <span className={joinClassName("text-4xl", countClassName)}>
                {model}
              </span>

              <span className="text-lg">
                khách
              </span>
            </>
          )}
        </div>
      </div>
    </div>
  );
}


