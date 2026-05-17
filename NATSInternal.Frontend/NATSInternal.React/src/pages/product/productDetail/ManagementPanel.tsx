import React from "react";
import { Link } from "react-router";

// Child components.
import Field from "@/pages/product/productDetail/Field";

// Props.
type Props = {
  model: ProductDetailModel;
};

// Components.
export default function ManagementPanel(props: Props): React.ReactNode {
  // Template.
  function renderUser(user: UserBasicModel): React.ReactNode {
    if (user.isDeleted) {
      return (
        <span className="line-through">
          Đã xoá
        </span>
      );
    }

    return (
      <Link to={props.model.createdUser.detailRoute}>
        @{props.model.createdUser.userName}
      </Link>
    );
  }

  return (
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">
          Quản lý
        </span>
      </div>

      <div className="panel-body">
        <div className="flex flex-col gap-y-3 p-3">
          {/* CreatedUser */}
          <Field propertyName="createdUser">
            {renderUser(props.model.createdUser)}
          </Field>

          {/* CreatedDateTime */}
          <Field propertyName="createdDateTime">
            {props.model.createdDateTime}
          </Field>

          {/* LastUpdatedUser */}
          {props.model.lastUpdatedUser && (
            <Field propertyName="lastUpdatedUser">
              {renderUser(props.model.lastUpdatedUser)}
            </Field>
          )}

          {/* LastUpdatedDateTime */}
          {props.model.lastUpdatedDateTime && (
            <Field propertyName="lastUpdatedDateTime">
              {props.model.lastUpdatedDateTime}
            </Field>
          )}

          {/* DeletedUser */}
          {props.model.deletedUser && (
            <Field propertyName="deletedUser">
              {renderUser(props.model.deletedUser)}
            </Field>
          )}

          {/* DeletedDateTime */}
          {props.model.deletedDateTime && (
            <Field propertyName="deletedDateTime">
              {props.model.deletedDateTime}
            </Field>
          )}
        </div>
      </div>
    </div>
  );
}
