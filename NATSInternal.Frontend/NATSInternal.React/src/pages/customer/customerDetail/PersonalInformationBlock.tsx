import React from "react";
import { useTsxHelper } from "@/helpers";

// Child components
import FieldContainer from "./FieldContainer";
import Field from "./Field";
import { Block, NewTabWebsiteLink, NewTabEmailLink, NewTabPhoneLink } from "@/components/ui";

// Props.
type CustomerPersonalInformationProps = { model: CustomerDetailModel };

// Component.
export default function CustomerPersonalInformation(props: CustomerPersonalInformationProps): React.ReactNode {
  // Dependencies.
  const { compute } = useTsxHelper();

  // Computed.
  const genderClassName = compute<string>(() => {
    if (props.model.gender === "Male") {
      return "text-blue-600 dark:text-blue-400";
    }

    return "text-red-600 dark:text-red-400"; 
  });

  // Template.
  return (
    <Block title="Thông tin cá nhân" bodyClassName="flex flex-col gap-3 p-3">
      <div className="grid grid-cols-[auto_1fr] gap-3">
        {/* Avatar */}
        <img
          className="bg-black/5 dark:bg-white/5 border border-black/15 dark:border-white/25 p-1 rounded-lg"
          src={props.model.avatarUrl}
          alt={props.model.fullName}
        />

        {/* Names */}
        <div className="flex flex-col justify-start pt-1">
          <span className="text-2xl">{props.model.fullName}</span>
          <span className="text-lg opacity-50">{props.model.nickName}</span>
        </div>
      </div>
      
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
            <NewTabWebsiteLink href={props.model.facebookUrl}>123</NewTabWebsiteLink>
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
    </Block>
  );
}