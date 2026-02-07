import React from "react";
import { Link } from "react-router";
import { useTsxHelper } from "@/helpers";

// Props.
type ResultsPanelProps = {
  model: CustomerListModel;
  isReloading: boolean;
};

// Components.
export default function ResultsPanel(props: ResultsPanelProps): React.ReactNode {
  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // Templates.
  return (
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">
          Danh sách kết quả
        </span>
      </div>

      <div className={joinClassName("panel-body transition-opacity", props.isReloading && "opacity-50")}>
        <ul className="list-group list-group-flush">
          {props.model.items.length > 0 ? props.model.items.map((customer, index) => (
            <ResultItem model={customer} key={index} />
          )) : (
            <li className="list-group-item opacity-50 px-3 py-10">
              Không có kết quả
            </li>
          )}
        </ul>
      </div>
    </div>
  );
}

function ResultItem(props: { model: CustomerListCustomerModel }): React.ReactNode {
  // Template.
  return (
    <li className="list-group-item items-center px-3 py-1.5">
      <div className="grid grid-cols-[auto_1fr] gap-3">
        <img src={props.model.avatarUrl} className="img-thumbnail size-12" alt={props.model.fullName} />

        <div className="flex flex-col">
          <Link to={props.model.detailRoute} className="font-bold text-blue-700 dark:text-blue-400">
            {props.model.fullName}
          </Link>

          <span className="text-sm opacity-50">
            {props.model.nickName}
          </span>
        </div>
      </div>
    </li>
  );
}