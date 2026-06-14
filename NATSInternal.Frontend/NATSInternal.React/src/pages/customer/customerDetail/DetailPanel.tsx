import React from "react";
import { Link } from "react-router";
import { compute, joinClassName, getTextClassNameBasedOnDebtAmount } from "@/helpers";

// Child components
import { Field, FieldContainer } from "@/pages/shared/detail";
import { NewTabWebsiteLink, NewTabEmailLink, NewTabPhoneLink } from "@/components/ui";

// Props.
type CustomerDetailProps = {
  model: CustomerDetailModel;
};

// Components.
export default function DetailPanel(props: CustomerDetailProps): React.ReactNode {
  // Computed.
  const areaClassName = joinClassName(
    "bg-black/2.5 dark:bg-white/2.5",
    "border border-black/15 dark:border-white/15 p-3 py-2 rounded-lg"
  );
  const genderClassName = compute<string>(() => {
    if (props.model.gender === "Male") {
      return "text-blue-600 dark:text-blue-400";
    }

    return "text-red-600 dark:text-red-400"; 
  });

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
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">
          Thông tin khách hàng
        </span>
      </div>

      <div className="panel-body grid grid-cols-1 lg:grid-cols-2 gap-3 p-3">
        <FieldContainer className={areaClassName}>
          {/* Gender */}
          <Field name="gender">
            <span className={genderClassName}>
              {props.model.gender === "Male" ? "Nam" : "Nữ"}
            </span>
          </Field>

          {/* Birthday */}
          {props.model.birthday && (
            <Field name="birthday">{props.model.birthday}</Field>
          )}

          {/* PhoneNumber */}
          {props.model.phoneNumber && (
            <Field name="phoneNumber">
              <NewTabPhoneLink phoneNumber={props.model.phoneNumber} />
            </Field>
          )}

          {/* ZaloNumber */}
          {props.model.zaloNumber && (
            <Field name="zaloNumber">
              <NewTabWebsiteLink href={"https://zalo.me/" + props.model.zaloNumber}>
                {props.model.zaloNumber}
              </NewTabWebsiteLink>
            </Field>
          )}

          {/* FacebookUrl */}
          {props.model.facebookUrl && (
            <Field name="facebookUrl">
              <NewTabWebsiteLink href={props.model.facebookUrl}>
                {props.model.facebookUrl}
              </NewTabWebsiteLink>
            </Field>
          )}

          {/* Email */}
          {props.model.email && (
            <Field name="email">
              <NewTabEmailLink email={props.model.email} />
            </Field>
          )}

          {/* Address */}
          {props.model.address && (
            <Field name="address">
              {props.model.address}
            </Field>
          )}
        </FieldContainer>

        <div className="flex flex-col gap-3">
          <FieldContainer className={areaClassName}>
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
          
          <FieldContainer className={areaClassName}>
            <Field name="debtRemainingAmount">
              <span className={getTextClassNameBasedOnDebtAmount(props.model.debtAmount)}>
                {props.model.displayDebtAmountText}
              </span>
            </Field>
          </FieldContainer>
        </div>
      </div>
    </div>
  );
}
