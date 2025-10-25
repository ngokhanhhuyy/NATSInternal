import React, { useMemo, createContext, useContext } from "react";
import { useTsxHelper } from "@/helpers";

// Shared components.
import { FormContext } from "@/components/form/Form";

// Context.
export type FormFieldContextPayload = { isValidated: boolean; hasError: boolean; };
export const FormFieldContext = createContext<FormFieldContextPayload>(undefined!);

// Props.
export type FormFieldProps = {
  path?: string;
  displayName?: string;
  children: React.ReactNode;
} & React.ComponentPropsWithoutRef<"div">;

// Components.
export default function FormField(props: FormFieldProps) {
  // Dependencies.
  const formContext = useContext(FormContext);
  const { compute, joinClassName } = useTsxHelper();

  // Computed.
  const errorMessage = useMemo(() => {
    if (!formContext || !props.path) {
      return;
    }

    const messages = formContext.errorCollection.details
      .filter(d => d.propertyPath === props.path)
      .map(d => d.message);

    if (messages.length === 0) {
      return;
    }

    const message = messages[0];

    if (!props.displayName) {
      return message;
    }

    return `${props.displayName} ${message[0].toLowerCase()}${message.substring(1)}`;
  }, [formContext.errorCollection.details]);

  const contextPayload = useMemo<FormFieldContextPayload>(() => ({
    isValidated: !!formContext.errorCollection.isValidated,
    hasError: !!errorMessage
  }), [formContext.errorCollection.details]);
  
  const labelClassName = compute(() => contextPayload.hasError ? "text-red-300" : "text-black/50");

  // Template.
  return (
    <div className={joinClassName(props.className, "form-field flex flex-col justify-stretched")}>
      {/* Label */}
      {props.displayName && (
        <label htmlFor={props.path} className={joinClassName(labelClassName, "block text-sm")}>
          {props.displayName}
        </label>
      )}

      {/* Input */}
      <FormFieldContext.Provider value={contextPayload}>
        {props.children}
      </FormFieldContext.Provider>

      {/* Message */}
      {!!errorMessage && <div className="text-red-500">{errorMessage}</div>}
    </div>
  );
}