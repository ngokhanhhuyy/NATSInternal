import React from "react";
import { useValidationHelper } from "@/helpers";

// Child components.
import MainContainer from "@/components/layouts/MainContainer";
import IntroducerInput from "./introducerPicker.tsx/IntroducerInput";
import { Form, FormField, TextInput, TextAreaInput, SelectInput, DateTimeInput, SubmitButton } from "@/components/form";

// Props.
type CustomerUpsertPageProps<T> = {
  description: string;
  isForCreating: boolean;
  model: CustomerUpsertModel;
  onModelChanged(changedData: Partial<CustomerUpsertModel>): any;
  submitAction(): Promise<T>;
  onSubmissionSucceeded(submissionResult: T): any;
  renderButtons?(): React.ReactNode;
};

// Component.
export default function CustomerUpsertPage<T>(props: CustomerUpsertPageProps<T>): React.ReactNode {
  // Dependencies.
  const { validatePhoneNumber } = useValidationHelper();

  // Template;
  return (
    <MainContainer description={props.description}>
      <Form
        className="flex flex-col gap-3"
        submitAction={props.submitAction}
        onSubmissionSucceeded={props.onSubmissionSucceeded}
      >
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
              model={props.model.introducer}
              onModelChanged={introducer => props.onModelChanged({ introducer })}
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

        <div className="flex justify-end gap-3">
          <SubmitButton/>
        </div>
      </Form>
    </MainContainer>
  );
}