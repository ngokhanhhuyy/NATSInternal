import React from "react";
import { Link } from "react-router";

// Child components.
import { Field, FieldContainer } from "@/pages/shared/detail";
import { Block } from "@/components/ui";

// Props.
type ManagementPanelProps = { model: CustomerDetailModel };

// Component.
export default function ManagementPanel(props: ManagementPanelProps): React.ReactNode {
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
    <Block title="Quản lý" bodyClassName="p-3">
      <FieldContainer>
        {/* CreatedUser */}
        <Field name="createdUser">
            {renderUser(props.model.createdUser)}
        </Field>

        {/* CreatedDateTime */}
        <Field name="createdDateTime">
          {props.model.createdDateTime}
        </Field>

        {/* LastUpdatedUser */}
        {props.model.lastUpdatedUser && (
          <Field name="lastUpdatedUser">
            {renderUser(props.model.lastUpdatedUser)}
          </Field>
        )}

        {/* LastUpdatedDateTime */}
        {props.model.lastUpdatedDateTime && (
          <Field name="lastUpdatedDateTime">
            {props.model.lastUpdatedDateTime}
          </Field>
        )}

        {/* DeletedUser */}
        {props.model.deletedUser && (
          <Field name="deletedUser">
            {renderUser(props.model.deletedUser)}
          </Field>
        )}

        {props.model.deletedDateTime && (
          <Field name="deletedDateTime">
            {props.model.deletedDateTime}
          </Field>
        )}
      </FieldContainer>
    </Block>
  );
}
