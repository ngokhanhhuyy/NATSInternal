import React from "react";

// Child components.
import CustomerList from "@/pages/shared/list/customerListResults";

// Props.
type ResultsPanelProps = {
  model: CustomerListModel;
  isReloading: boolean;
};

// Components.
export default function ResultsPanel(props: ResultsPanelProps): React.ReactNode {
  // Templates.
  return (
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">
          Danh sách kết quả
        </span>
      </div>

      <div className="panel-body transition-opacity">
        <CustomerList
          model={props.model}
          isReloading={props.isReloading}
          className="list-group-flush"
          computeItemClassName={_ => "ps-3"}
        />
      </div>
    </div>
  );
}
