import React, { useState, useCallback } from "react";
import { useTsxHelper } from "@/helpers";

// Child component.
import { Button } from "@/components/ui";
import { ChevronUpIcon } from "@heroicons/react/24/outline";

// Props.
type Props = {
  title: string;
  isReloading: boolean;
  children: React.ReactNode | React.ReactNode[];
};

// Component.
export default function OptionsPanel(props: Props): React.ReactNode {
  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // States.
  const [isBodyExpanded, setIsBodyExpanded] = useState(true);

  // Callbacks.
  const handleExpandButtonClicked = useCallback(() => {
    setIsBodyExpanded(isExpanded => !isExpanded);
  }, []);

  // Template.
  return (
    <div className="panel">
      <div className={joinClassName("panel-header", !isBodyExpanded && "rounded-b-xl")}>
        <div className="panel-header-title">
          Chế độ hiển thị
        </div>

        <Button className="btn btn-panel-header btn-sm aspect-square" onClick={handleExpandButtonClicked}>
          <ChevronUpIcon className={joinClassName(
            "size-4.5",
            !isBodyExpanded && "rotate-180" )}
          />
        </Button>
      </div>

      <div className={joinClassName(
        "panel-body transition-all duration-200 px-3",
        props.isReloading && "pointer-events-none",
        !isBodyExpanded ? "opacity-0 grid-rows-[0fr] py-0" : "opacity-100 grid-rows-[1fr] py-3"
      )}>
        <div className="overflow-hidden">
          {props.children}
        </div>
      </div>
    </div>
  );
}