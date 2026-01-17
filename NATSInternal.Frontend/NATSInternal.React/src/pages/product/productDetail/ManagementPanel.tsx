import React from "react";
import { Link } from "react-router";

// Child components.
import Field from "@/pages/product/productDetail/Field.tsx";

// Props.
type Props = {
  model: ProductDetailModel;
};

// Components.
export default function ManagementPanel(props: Props): React.ReactNode {
  // Template.
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
            {props.model.createdUser.isDeleted ? (
              <span className="line-through">
                Đã xoá
              </span>
            ) : (
              <Link to={props.model.createdUser.detailRoute}>
                @{props.model.createdUser.userName}
              </Link>
            )}
          </Field>

          {/* CreatedDateTime */}
          <Field propertyName="createdDateTime">
            {props.model.createdDateTime}
          </Field>

          {/* LastUpdatedUser */}
          {props.model.lastUpdatedUser && (
            <Field propertyName="lastUpdatedUser">
              {props.model.lastUpdatedUser.isDeleted ? (
                <span className="line-through">
                  Đã xoá
                </span>
              ) : (
                <Link to={props.model.lastUpdatedUser.detailRoute}>
                  {props.model.lastUpdatedUser.userName}
                </Link>
              )}
            </Field>
          )}

          {/* LastUpdatedDateTime */}
          {props.model.lastUpdatedDateTime && (
            <Field propertyName="lastUpdatedDateTime">
              {props.model.lastUpdatedDateTime}
            </Field>
          )}
        </div>
      </div>
    </div>
  );
}