import React from "react";
import { joinClassName } from "@/helpers";

// Props.
type StepPanelProps = {
  currentStep: number;
  steps: Map<number, string>;
};

// Components.
export default function StepPanel(props: StepPanelProps): React.ReactNode {
  // Template.
  return (
    <div className="panel">
      <div className="panel-body p-3">
        <div className="flex justify-center w-full relative pointer-events-none select-none">
          {Array.from(props.steps).map(([key, displayName], index) => (
            <React.Fragment key={index}>
              <div className="flex flex-col justify-start items-center w-30 sm:w-40 gap-y-1.5" key={key}>
                <div
                  className={joinClassName(
                    "flex justify-center items-center text-lg p-1 rounded-full size-10 border",
                    props.currentStep === key
                      ? "bg-blue-600 dark:bg-blue-500 border-transparent dark:border-blue-400 text-white"
                      : "bg-neutral-50 dark:bg-neutral-800 border-black/25 dark:border-white/25",
                  )}
                >
                  {key}
                </div>
                <span className="text-center">{displayName}</span>
              </div>

              {index < props.steps.size - 1 && (
                <div className="flex justify-stretch items-center h-10">
                  <div className="bg-neutral-900/15 dark:bg-neutral-50/15 w-15 sm:w-20 md:w-30 h-1 scale-x-150" />
                </div>
              )}
            </React.Fragment>
          ))}
        </div>
      </div>
    </div>
  );
}
