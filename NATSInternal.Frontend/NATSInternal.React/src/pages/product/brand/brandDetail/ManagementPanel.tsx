import React from "react";

// Child components.
import Field from "./Fields";

// Props.
type Props = {
  model: BrandDetailModel;
};

// Component.
export default function DetailPanel(props: Props): React.ReactNode {
  // Template.
  return (
    <div className="panel h-full">
      <div className="panel-header">
        <span className="panel-header-title">
          Quản lý
        </span>
      </div>

      <div className="panel-body flex flex-col p-3">
        <Field propertyName="createdDateTime">
          {props.model.createdDateTime}
        </Field>
      </div>
    </div>
  );
}