import React from "react";

// Child components
import FieldContainer from "./FieldContainer";
import Field from "./Field";
import { Block } from "@/components/ui";

// Props.
type CustomerPersonalInformationProps = { model: CustomerDetailModel };

// Component.
export default function CustomerPersonalInformation(props: CustomerPersonalInformationProps): React.ReactNode {
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
          <span className={props.model.gender === "Male" ? "text-blue-500" : "text-red-500"}>
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
            <a href={`tel:${props.model.phoneNumber}`} target="_blank" rel="noopener noreferrer">
              {props.model.phoneNumber}
            </a>
          </Field>
        )}

        {/* ZaloNumber */}
        {props.model.zaloNumber && (
          <Field name="zaloNumber">
            <a href={"https://zalo.me/" + props.model.zaloNumber} target="_blank" rel="noopener noreferrer">
              {props.model.zaloNumber}
            </a>
          </Field>
        )}

        {/* FacebookUrl */}
        {props.model.facebookUrl && (
          <Field name="facebookUrl">
            <a href={props.model.facebookUrl} target="_blank" rel="noopener noreferrer">
              {props.model.facebookUrl}
            </a>
          </Field>
        )}

        {/* Email */}
        {props.model.email && (
          <Field name="email">
            <a href={"mailto:" + props.model.email.toLowerCase()} target="_blank" rel="noopener noreferrer">
              {props.model.email.toLowerCase()}
            </a>
          </Field>
        )}

        {/* Address */}
        {props.model.address && (
          <Field name="address" marginBottom={false}>
            {props.model.address}
          </Field>
        )}
      </FieldContainer>
    </Block>
  );
}