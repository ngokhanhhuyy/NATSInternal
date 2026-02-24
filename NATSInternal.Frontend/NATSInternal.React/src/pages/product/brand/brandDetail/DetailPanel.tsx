import React from "react";

// Child components.
import Field from "./Fields";
import { NewTabWebsiteLink, NewTabPhoneLink, NewTabEmailLink } from "@/components/ui";
import { BuildingStorefrontIcon } from "@heroicons/react/24/outline";

// Props.
type Props = {
  model: BrandDetailModel;
};

// Component.
export default function DetailPanel(props: Props): React.ReactNode {
  // Template.
  return (
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">
          Chi tiết thương hiệu
        </span>
      </div>

      <div className="panel-body flex flex-col p-3 gap-3">
        <div className="flex gap-3 justify-start items-start">
          <div className="img-thumbnail size-14 flex justify-center items-center">
            <BuildingStorefrontIcon className="size-7 opacity-50" />
          </div>

          <div className="flex flex-col justify-start items-start">
            <div className="text-2xl text-blue-700 dark:text-blue-400">
              {props.model.name}
            </div>
          </div>
        </div>

        <div className="flex flex-col gap-3">
          {/* Website */}
          {props.model.website && (
            <Field propertyName="website">
              <NewTabWebsiteLink href={props.model.website}>
                {props.model.website}
              </NewTabWebsiteLink>
            </Field>
          )}

          {/* SocialMediaUrl */}
          {props.model.socialMediaUrl && (
            <Field propertyName="socialMediaUrl">
              <NewTabWebsiteLink href={props.model.socialMediaUrl}>
                {props.model.socialMediaUrl}
              </NewTabWebsiteLink>
            </Field>
          )}

          {/* PhoneNumber */}
          {props.model.phoneNumber && (
            <Field propertyName="phoneNumber">
              <NewTabPhoneLink phoneNumber={props.model.phoneNumber} />
            </Field>
          )}

          {/* Email */}
          {props.model.email && (
            <Field propertyName="email">
              <NewTabEmailLink email={props.model.email} />
            </Field>
          )}

          {/* Address */}
          {props.model.address && (
            <Field propertyName="address">
              {props.model.address}
            </Field>
          )}

          {props.model.country && (
            <Field propertyName="country">
              {props.model.country.name}
            </Field>
          )}

          <Field propertyName="createdDateTime">
            {props.model.createdDateTime}
          </Field>
        </div>
      </div>
    </div>
  );
}