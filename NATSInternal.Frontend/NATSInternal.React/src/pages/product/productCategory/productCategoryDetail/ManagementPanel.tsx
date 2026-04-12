import React from "react";

// Child components.
import Field from "@/pages/shared/detail/Field";

// Props.
type Props = {
  model: ProductCategoryDetailModel;
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

      <div className="panel-body flex flex-col p-3 gap-3">
        <Field propertyName="createdDateTime">
          {props.model.createdDateTime}
        </Field>

        <Field propertyName="createdUser">
          {props.model.createdUser.userName}
        </Field>

        {props.model.lastUpdatedDateTime && (
          <Field propertyName="lastUpdatedDateTime">
            {props.model.lastUpdatedDateTime}
          </Field>
        )}

        {props.model.lastUpdatedUser && (
          <Field propertyName="lastUpdatedUser">
            {props.model.lastUpdatedUser.userName}
          </Field>
        )}
      </div>
    </div>
  );
}