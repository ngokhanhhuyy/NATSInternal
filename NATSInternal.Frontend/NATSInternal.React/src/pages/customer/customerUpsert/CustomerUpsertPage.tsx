import React from "react";
import { useDirtyModelChecker } from "@/hooks/dirtyModelCheckerHook";
import { useValidationHelper } from "@/helpers";

// Child components.
import { FormContainer } from "@/components/layouts";
import IntroducerInput from "./introducerPicker/IntroducerInput";
import { FormField, TextInput, TextAreaInput, SelectInput } from "@/components/form";
import { DateTimeInput, SubmitButton, DeleteButton } from "@/components/form";
import { Block } from "@/components/ui";

// Props.
type CustomerUpsertPageProps<T> = {
  description: string;
  isForCreating: boolean;
  model: CustomerUpsertModel;
  onModelChanged(changedData: Partial<CustomerUpsertModel>): any;
  upsertAction(): Promise<T>;
  onUpsertingSucceeded(result: T): any;
  deleteAction?(): Promise<void>;
  onDeletionSucceeded?(): any;
  renderButtons?(): React.ReactNode;
};

// Component.
export default function CustomerUpsertPage<T>(props: CustomerUpsertPageProps<T>): React.ReactNode {
  // Dependencies.
  const { validatePhoneNumber } = useValidationHelper();

  // States.
  const isModelDirty = useDirtyModelChecker(props.model.toRequestDto(), props.model.toRequestDto());

  // Template;
  return (
    <FormContainer
      description={props.description}
      upsertAction={props.upsertAction}
      onUpsertingSucceeded={props.onUpsertingSucceeded}
      isModelDirty={isModelDirty}
    >
      <Block title="Thông tin cá nhân khách hàng" bodyClassName="flex flex-col gap-3 px-4 pt-2.5 pb-4">
        <div className="grid grid-cols-1 sm:grid-cols-6 gap-3">
          {/* FirstName */}
          <FormField path="firstName" className="sm:col-span-2">
            <TextInput
              placeholder="Nguyễn"
              value={props.model.firstName}
              onValueChanged={(firstName) => props.onModelChanged({ firstName })}
            />
          </FormField>

          {/* MiddleName */}
          <FormField path="middleName" className="sm:col-span-2">
            <TextInput
              placeholder="Văn"
              value={props.model.middleName}
              onValueChanged={(middleName) => props.onModelChanged({ middleName })}
            />
          </FormField>

          {/* LastName */}
          <FormField path="lastName" className="sm:col-span-2">
            <TextInput
              placeholder="An"
              value={props.model.lastName}
              onValueChanged={(lastName) => props.onModelChanged({ lastName })}
            />
          </FormField>
        </div>

        <div className="grid grid-cols-1 sm:grid-cols-6 gap-3">
          {/* NickName */}
          <FormField path="nickName" className="sm:col-span-2">
            <TextInput
              placeholder="Anh An"
              value={props.model.nickName}
              onValueChanged={(nickName) => props.onModelChanged({ nickName })}
            />
          </FormField>

          {/* Gender */}
          <FormField path="gender" className="sm:col-span-2">
            <SelectInput
              options={[{ value: "Male", displayName: "Nam" }, { value: "Female", displayName: "Nữ" }]}
              value={props.model.gender}
              onValueChanged={(gender: Gender) => props.onModelChanged({ gender })}
            />
          </FormField>

          {/* Birthday */}
          <FormField path="birthday" className="sm:col-span-2">
            <DateTimeInput
              type="date"
              value={props.model.birthday}
              onValueChanged={(birthday) => props.onModelChanged({ birthday })}
            />
          </FormField>

          {/* PhoneNumber */}
          <FormField path="phoneNumber" className="sm:col-span-2">
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
          <FormField path="zaloNumber" className="sm:col-span-2">
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
          <FormField path="email" className="sm:col-span-2">
            <TextInput
              placeholder="nguyenvanan@gmail.com"
              value={props.model.email}
              onValueChanged={(email) => props.onModelChanged({ email })}
            />
          </FormField>

          {/* Address */}
          <FormField path="address" className="sm:col-span-3">
            <TextInput
              placeholder="123 Nguyễn Tất Thành"
              value={props.model.address}
              onValueChanged={(address) => props.onModelChanged({ address })}
            />
          </FormField>

          {/* Introducer */}
          <FormField path="introducer" className="sm:col-span-3">
            <IntroducerInput
              value={props.model.introducer}
              onValueChanged={introducer => props.onModelChanged({ introducer })}
            />
          </FormField>

          {/* Note */}
          <FormField path="note" className="sm:col-span-6">
            <TextAreaInput
              placeholder="Ghi chú"
              value={props.model.note}
              onValueChanged={(note) => props.onModelChanged({ note })}
            />
          </FormField>
        </div>
      </Block>

      <div className="flex justify-end gap-3">
        {props.deleteAction && <DeleteButton/>}
        <SubmitButton/>
      </div>
    </FormContainer>
  );
}