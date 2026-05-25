import React from "react";

// Child components.
import { UserIcon } from "@heroicons/react/24/outline";

// Props.
type AvatarAndNamePanelProps = {
  model: CustomerDetailModel;
};

// Components.
export default function AvatarAndNamePanel(props: AvatarAndNamePanelProps): React.ReactNode {
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
            <span className="text-blue-600 dark:text-blue-400 text-2xl">
              {props.model.fullName}
            </span>
            <span className="text-lg opacity-50">{props.model.nickName}</span>
          </div>
        </div>
      </div>
    </div>
  );
}
