import React from "react";
import { Link } from "react-router";

// Child components.
import MainBlock from "./MainBlock";
import FieldContainer from "./FieldContainer";
import Field from "./Field";

// Props.
type ManagementBlock = { model: CustomerDetailModel };

// Component.
export default function ManagementBlock(props: ManagementBlock): React.ReactNode {
  // Template.
  return (
    <MainBlock title="Quản lý">
      <FieldContainer>
        {/* CreatedUser */}
        <Field name="createdUser">
          {!props.model.createdUser.isDeleted ? (
            <Link to={props.model.createdUser.detailRoute}>
              @{props.model.createdUser.userName}
            </Link>
          ) : (
            <span className="line-through">
              Tài khoản đã bị xoá
            </span>
          )}
        </Field>

        {/* CreatedDateTime */}
        <Field name="createdDateTime" marginBottom={!!props.model.lastUpdatedUser || !!props.model.lastUpdatedDateTime}>
          {props.model.createdDateTime}
        </Field>

        {/* LastUpdatedUser */}
        {props.model.lastUpdatedUser && (
          <Field name="lastUpdatedUser">
            {!props.model.createdUser.isDeleted ? (
              <Link to={props.model.createdUser.detailRoute}>
                @{props.model.createdUser.userName}
              </Link>
            ) : (
              <span className="line-through">
                Tài khoản đã bị xoá
              </span>
            )}
          </Field>
        )}

        {/* LastUpdatedDateTime */}
        {props.model.lastUpdatedDateTime && (
          <Field name="lastUpdatedDateTime" marginBottom={false}>
            {props.model.lastUpdatedDateTime}
          </Field>
        )}
      </FieldContainer>
    </MainBlock>
  );
}
