import React, { useMemo, createContext, useContext } from "react";
import { useTsxHelper } from "@/helpers";

// Shared components.
import { FormContext } from "@/components/form/Form";

// Context.
export type FormFieldContextPayload = { isValidated: boolean; hasError: boolean; };
export const FormFieldContext = createContext<FormFieldContextPayload>(undefined!);

// Props.
export type FormFieldProps = {
  label?: string;
  propertyPath?: string;
  children: React.ReactNode;
} & React.ComponentPropsWithoutRef<"div">;

// Components.
export default function FormField(props: FormFieldProps) {
  // Dependencies.
  const formContext = useContext(FormContext);
  const { joinClassName } = useTsxHelper();

  // Computed.
  const errorMessage = useMemo(() => {
    console.log("Triggered");
    if (formContext && props.propertyPath) {
      const messages = formContext.errorCollection.details
        .filter(d => d.propertyPath === props.propertyPath)
        .map(d => d.message);

      return messages.length >= 1 ? messages[0] : undefined;
    }
  }, [formContext.errorCollection.details]);

  const contextPayload = useMemo<FormFieldContextPayload>(() => ({
    isValidated: !!formContext.errorCollection.isValidated,
    hasError: !!errorMessage
  }), [formContext.errorCollection.details]);

  return (
    <div className={joinClassName("form-group", props.className)}>
      <pre>{JSON.stringify(formContext?.errorCollection.details, null, 2)}</pre>
      {/* Label */}
      {props.label && <label className="form-label">{props.label}</label>}

      {/* Input */}
      <FormFieldContext.Provider value={contextPayload}>
        {props.children}
      </FormFieldContext.Provider>

      {/* Message */}
      {!!errorMessage && <span className="text-danger">{errorMessage}</span>}
    </div>
  );
}