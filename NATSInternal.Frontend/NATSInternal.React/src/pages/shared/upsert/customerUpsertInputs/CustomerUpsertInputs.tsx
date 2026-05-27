import React from "react";
import { validatePhoneNumber } from "@/helpers";

// Child components.
import { FormField, TextInput, TextAreaInput, SelectInput, DateTimeInput } from "@/components/form";
import CustomerPickerInput from "@/pages/shared/upsert/customerPicker";

// Props.
type CustomerUpsertInputsProps = {
  id?: number;
  model: CustomerUpsertModel;
  onModelChanged(updatedData: Partial<CustomerUpsertModel>): any;
  isForCreating: boolean;
  pathPrefix?: string;
};

// Components.
export default function CustomerUpsertInputs(props: CustomerUpsertInputsProps): React.ReactNode {
  // Computed.
  const computePath = (path: string) => {
    if (props.pathPrefix && path) {
      return [props.pathPrefix, path].join(".");
    }

    return path;
  };

  // Template.
  return (
    <>
      <div className="grid grid-cols-1 sm:grid-cols-6 gap-3">
        {/* FirstName */}
        <FormField path={computePath("firstName")} className="sm:col-span-2">
          <TextInput
            placeholder="Nguyễn"
            value={props.model.firstName}
            onValueChanged={(firstName) => props.onModelChanged({ firstName })}
          />
        </FormField>
        
        {/* MiddleName */}
        <FormField path={computePath("middleName")} className="sm:col-span-2">
          <TextInput
            placeholder="Văn"
            value={props.model.middleName}
            onValueChanged={(middleName) => props.onModelChanged({ middleName })}
          />
        </FormField>

        {/* LastName */}
        <FormField path={computePath("lastName")} className="sm:col-span-2">
          <TextInput
            placeholder="An"
            value={props.model.lastName}
            onValueChanged={(lastName) => props.onModelChanged({ lastName })}
          />
        </FormField>
      </div>

      <div className="grid grid-cols-1 sm:grid-cols-6 gap-3">
        {/* NickName */}
        <FormField path={computePath("nickName")} className="sm:col-span-2">
          <TextInput
            placeholder="Anh An"
            value={props.model.nickName}
            onValueChanged={(nickName) => props.onModelChanged({ nickName })}
          />
        </FormField>

        {/* Gender */}
        <FormField path={computePath("gender")} className="sm:col-span-2">
          <SelectInput
            options={[{ value: "Male", displayName: "Nam" }, { value: "Female", displayName: "Nữ" }]}
            value={props.model.gender}
            onValueChanged={(gender: Gender) => props.onModelChanged({ gender })}
          />
        </FormField>

        {/* Birthday */}
        <FormField path={computePath("birthday")} className="sm:col-span-2">
          <DateTimeInput
            type="date"
            value={props.model.birthday}
            onValueChanged={(birthday) => props.onModelChanged({ birthday })}
          />
        </FormField>

        {/* PhoneNumber */}
        <FormField path={computePath("phoneNumber")} className="sm:col-span-2">
          <TextInput
            type="tel"
            placeholder="0123 456 789"
            value={props.model.phoneNumber}
            onValueChanged={(phoneNumber) => {
              if (!phoneNumber.length || validatePhoneNumber(phoneNumber)) {
                props.onModelChanged({ phoneNumber });
              }
            }}
          />
        </FormField>

        {/* ZaloNumber */}
        <FormField path={computePath("zaloNumber")} className="sm:col-span-2">
          <TextInput
            type="tel"
            placeholder="0123 456 789"
            value={props.model.zaloNumber}
            onValueChanged={(zaloNumber) => {
              if (!zaloNumber.length || validatePhoneNumber(zaloNumber)) {
                props.onModelChanged({ zaloNumber });
              }
            }}
          />
        </FormField>

        {/* Email */}
        <FormField path={computePath("email")} className="sm:col-span-2">
          <TextInput
            placeholder="nguyenvanan@gmail.com"
            value={props.model.email}
            onValueChanged={(email) => props.onModelChanged({ email })}
          />
        </FormField>

        {/* FacebookURL */}
        <FormField path={computePath("facebookUrl")} className="sm:col-span-3">
          <TextInput
            placeholder="https://facebook.com/nguyenvana"
            value={props.model.facebookUrl}
            onValueChanged={(facebookUrl) => props.onModelChanged({ facebookUrl })}
          />
        </FormField>

        {/* Address */}
        <FormField path={computePath("address")} className="sm:col-span-3">
          <TextInput
            placeholder="123 Nguyễn Tất Thành"
            value={props.model.address}
            onValueChanged={(address) => props.onModelChanged({ address })}
          />
        </FormField>

        {/* Introducer */}
        <FormField path={computePath("introducer")} className="sm:col-span-6">
          <CustomerPickerInput
            resourceName="introducer"
            value={props.model.introducer}
            onValueChanged={introducer => props.onModelChanged({ introducer })}
            excludedId={(!props.isForCreating && props.id)  ? props.id : null}
          />
        </FormField>

        {/* Note */}
        <FormField path={computePath("note")} className="sm:col-span-6">
          <TextAreaInput
            placeholder="Ghi chú về khách hàng ..."
            value={props.model.note}
            onValueChanged={(note) => props.onModelChanged({ note })}
          />
        </FormField>
      </div>
    </>
  );
}
