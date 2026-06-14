import React from "react";
import { joinClassName, compute } from "@/helpers";

// Child components.
import DebtAlert from "@/pages/shared/alerts/DebtAlert";
import { UserIcon } from "@heroicons/react/24/outline";

// Props.
type AvatarAndNamePanelProps = {
  model: CustomerDetailModel;
};

// Components.
export default function AvatarAndNamePanel(props: AvatarAndNamePanelProps): React.ReactNode {
  // Computed.
  const linkClassName = compute<string>(() => {
    const names: string[] = ["font-bold", "whitespace-nowrap"];
    if (props.model.debtAmount === 0) {
      names.push("text-blue-700 dark:text-blue-400");
    } else if (props.model.debtAmount > 0) {;
      names.push("text-yellow-600 dark:text-yellow-400");
    } else {
      names.push("text-red-600 dark:text-red-400");
    }

    return names.join(" ");
  });

  // Template.
  return (
    <div className="panel">
      <div className="panel-body p-3">
        <div className="grid grid-cols-[auto_1fr] gap-3">
          {/* Avatar */}
          <div className="img-thumbnail size-16 flex justify-center items-center">
            <UserIcon className="size-8 opacity-50" />
          </div>

          {/* Names */}
          <div className="flex flex-col justify-start pt-1">
            <div className="flex flex-wrap gap-2 items-center">
              <span className={joinClassName("text-xl", linkClassName)}>
                {props.model.fullName}
              </span>

              <DebtAlert debtAmount={props.model.debtAmount} />
            </div>
            <span className="text-lg opacity-50">{props.model.nickName}</span>
          </div>
        </div>
      </div>
    </div>
  );
}
