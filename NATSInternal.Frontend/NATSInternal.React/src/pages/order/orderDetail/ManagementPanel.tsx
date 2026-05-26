import React from "react";
import { Link } from "react-router";

// Child components.
import { Field, FieldContainer } from "@/pages/shared/detail";

// Props.
type ManagementBlockProps = {
  model: OrderDetailModel;
};

// Components.
export default function ManagementBlock(props: ManagementBlockProps): React.ReactNode {
  // Template.
  function renderUser(user: UserBasicModel): React.ReactNode {
    if (user.isDeleted) {
      return (
        <span className="line-through">
          @{user.userName}
        </span>
      );
    }

    return (
      <Link to={user.detailRoute}>
        @{user.userName}
      </Link>
    );
  }

  return (
    <div className="panel flex-1">
      <div className="panel-header">
        <span className="panel-header-title">
          Quản lý
        </span>
      </div>
      <div className="panel-body p-3 pt-2">
        <FieldContainer>
          {/* CreatedUser */}
          <Field name="createdUser">
            {renderUser(props.model.createdUser)}
          </Field>
  
          {/* CreatedDateTime */}
          <Field name="createdDateTime">
            {props.model.displayCreatedDateTime}
          </Field>
  
          {/* LastUpdatedUser */}
          {props.model.lastUpdatedUser && (
            <Field name="lastUpdatedUser">
              {renderUser(props.model.lastUpdatedUser)}
            </Field>
          )}
  
          {/* LastUpdatedDateTime */}
          {props.model.displayLastUpdatedDateTime && (
            <Field name="lastUpdatedDateTime">
              {props.model.displayLastUpdatedDateTime}
            </Field>
          )}
  
          {/* DeletedUser */}
          {props.model.deletedUser && (
            <Field name="deletedUser">
              {renderUser(props.model.deletedUser)}
            </Field>
          )}
  
          {props.model.displayDeletedDateTime && (
            <Field name="deletedDateTime">
              {props.model.displayDeletedDateTime}
            </Field>
          )}
        </FieldContainer>
      </div>
    </div>
  );
}
