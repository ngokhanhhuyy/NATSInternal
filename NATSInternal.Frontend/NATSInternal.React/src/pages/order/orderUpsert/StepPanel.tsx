import React from "react";
import { joinClassName } from "@/helpers";

// Child components.
import { CheckIcon } from "@heroicons/react/24/outline"; 

// Props.
type StepPanelProps = {
  currentStep: number;
  steps: Map<number, string>;
  onStepClicked(step: number): any;
};

// Components.
export default function StepPanel(props: StepPanelProps): React.ReactNode {
  // Computed.
  const computeButtonClassName = (key: number): string => {
    const classNames = ["flex justify-center items-center text-lg p-1 rounded-full size-10 border cursor-pointer"];
    if (props.currentStep >= key) {
      classNames.push("bg-blue-600 dark:bg-blue-500 border-transparent dark:border-blue-400 text-white");
    } else {
      classNames.push("bg-neutral-50 dark:bg-neutral-800 border-black/25 dark:border-white/25");
    }

    return classNames.join(" ");
  };

  // Template.
  return (
    <div className="panel">
      <div className="panel-body p-3">
        <div className="flex justify-center w-full relative select-none">
          {Array.from(props.steps).map(([key, displayName], index) => (
            <React.Fragment key={index}>
              <div className="flex flex-col justify-start items-center w-30 sm:w-40 gap-y-1.5" key={key}>
                <div className={computeButtonClassName(key)} onClick={() => props.onStepClicked(key)}>
                  {key >= props.currentStep ? (
                    <span>{key}</span>
                  ) : (
                    <CheckIcon className="size-5" />
                  )}
                </div>
                <span className="text-center">{displayName}</span>
              </div>

              {index < props.steps.size - 1 && (
                <div className="flex justify-stretch items-center h-10">
                  <div className={joinClassName(
                    "w-15 sm:w-20 md:w-30 h-1 scale-x-150 rounded-full",
                    key >= props.currentStep
                      ? "bg-neutral-900/10 dark:bg-neutral-50/15"
                      : "bg-blue-600 dark:bg-blue-500"
                  )} />
                </div>
              )}
            </React.Fragment>
          ))}
        </div>
      </div>
    </div>
  );
}
