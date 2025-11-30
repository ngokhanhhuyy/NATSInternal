import React from "react";
import { Link } from "react-router";

// Child components.
import MainContainer from "@/components/layouts/MainContainer";
import { Button } from "@/components/ui";
import { Form, FormField, TextInput, SelectInput, DateTimeInput } from "@/components/form";

// Props.
type CustomerUpsertPageProps<T> = {
  description: string;
  isForCreating: boolean;
  model: CustomerUpsertModel;
  onModelChanged(changedData: Partial<CustomerUpsertModel>): any;
  submitAction(): Promise<T>;
  onSubmissionSucceeded(submissionResult: T): any;
};

// Component.
export default function CustomerUpsertPage<T>(props: CustomerUpsertPageProps<T>): React.ReactNode {
  // Template;
  return (
    <MainContainer description={props.description}>
      <Form
        className="flex flex-col gap-3"
        submitAction={props.submitAction}
        onSubmissionSucceeded={props.onSubmissionSucceeded}
      >
        <div className="grid grid-cols-1 sm:grid-cols-3 gap-3">
          {/* FirstName */}
          <FormField path="firstName">
            <TextInput
              placeholder="Nguyễn"
              value={props.model.firstName}
              onValueChanged={(firstName) => props.onModelChanged({ firstName })}
            />
          </FormField>

          {/* MiddleName */}
          <FormField path="middleName">
            <TextInput
              placeholder="Văn"
              value={props.model.middleName}
              onValueChanged={(middleName) => props.onModelChanged({ middleName })}
            />
          </FormField>

          {/* LastName */}
          <FormField path="lastName">
            <TextInput
              placeholder="An"
              value={props.model.lastName}
              onValueChanged={(lastName) => props.onModelChanged({ lastName })}
            />
          </FormField>
        </div>

        <div className="grid grid-cols-1 sm:grid-cols-3 gap-3">
          {/* NickName */}
          <FormField path="nickName">
            <TextInput
              placeholder="Anh An"
              value={props.model.nickName}
              onValueChanged={(nickName) => props.onModelChanged({ nickName })}
            />
          </FormField>

          {/* Gender */}
          <FormField path="gender">
            <SelectInput
              disabled
              options={[{ value: "Male", displayName: "Nam" }, { value: "Female", displayName: "Nữ" }]}
              value={props.model.gender}
              onValueChanged={(gender: Gender) => props.onModelChanged({ gender })}
            />
          </FormField>

          {/* Birthday */}
          <FormField path="birthday">
            <DateTimeInput
              type="date"
              value={props.model.birthday}
              onValueChanged={(birthday) => props.onModelChanged({ birthday })}
            />
          </FormField>

          {/* PhoneNumber */}
          <FormField path="phoneNumber">
            <TextInput
              type="tel"
              placeholder="0123 456 789"
              value={props.model.phoneNumber}
              onValueChanged={(phoneNumber) => {
                if (!phoneNumber.length || /^[0-9+]+$/g.test(phoneNumber)) {
                  props.onModelChanged({ phoneNumber });
                }
              }}
            />
          </FormField>
        </div>

        <div className="flex justify-end gap-3">
          <Button type="submit" className="primary">Lưu</Button>
        </div>
      </Form>

      <div className="p-3 border border-black/10 dark:border-white/10 rounded-lg">
        <pre>{JSON.stringify(props.model.toRequestDto(), null, 2)}</pre>
      </div>
    </MainContainer>
  );
}