import React from "react";
import { Link } from "react-router";

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
          <img className="img-thumbnail h-12" src={props.model.avatarUrl} />
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
