import React from "react";
import { compute } from "@/helpers";

// Child components
import { Field, FieldContainer } from "@/pages/shared/detail";
import { NewTabWebsiteLink, NewTabEmailLink, NewTabPhoneLink } from "@/components/ui";


// Props.
type PersonalInformationPanelProps = { model: CustomerDetailModel };

// Component.
export default function PersonalInformationPanel(props: PersonalInformationPanelProps): React.ReactNode {
  // Computed.
  const genderClassName = compute<string>(() => {
    if (props.model.gender === "Male") {
      return "text-blue-600 dark:text-blue-400";
    }

    return "text-red-600 dark:text-red-400"; 
  });

  // Template.
  return (
    <div className="panel h-full">
      <div className="panel-header">
        <span className="panel-header-title">
          Thông tin cá nhân
        </span>
      </div>

      <div className="panel-body flex flex-col gap-3 p-3">
        <FieldContainer>
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
      </div>
    </div>
  );
}
