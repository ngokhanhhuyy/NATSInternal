import React from "react";
import { Link } from "react-router";

// Child components.
import { UserIcon } from "@heroicons/react/24/outline";

// Props.
type CustomerPanelProps = {
  model: CustomerBasicModel;
};

// Components.
export default function CustomerPanel(props: CustomerPanelProps): React.ReactNode {
  // Template.
  return (
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">
          Khách hàng
        </span>
      </div>

      <div className="panel-body p-3">
        <div className="grid grid-cols-[auto_1fr] gap-3">
          <div className="img-thumbnail size-12 flex justify-center items-center">
            <UserIcon className="size-6 opacity-50" />
          </div>
          <div className="flex flex-col">
            <Link className="text-blue-600 dark:text-blue-400 font-bold" to={props.model.detailRoutePath}>
              {props.model.fullName}
            </Link>
            <span className="opacity-50">{props.model.nickName}</span>
          </div>
        </div>
      </div>
    </div>
  );
}
