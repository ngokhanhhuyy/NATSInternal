import React, { useMemo, createContext, useContext } from "react";
import { getDisplayName } from "@/metadata";
import { useTsxHelper } from "@/helpers";

// Shared components.
import { FormContext } from "@/components/form/Form";

// Context.
export type FormFieldContextPayload = {
  isValidated: boolean;
  hasError: boolean;
  path?: string;
  displayName?: string
};

export const FormFieldContext = createContext<FormFieldContextPayload | null>(null);

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
  const { joinClassName, compute } = useTsxHelper();

  // Computed.
  const errorMessage = useMemo(() => {
    if (!formContext || !formContext.errorCollection.isValidated || !props.path) {
      return;
    }

    const messages = formContext.errorCollection.details
      .filter(d => d.propertyPath === props.path)
      .map(d => d.message);

    if (messages.length === 0) {
      return;
    }

    return messages[0];
  }, [formContext?.errorCollection.details]);

  const displayName = useMemo(() => {
    if (props.displayName) {
      return props.displayName;
    }

    if (!props.path) {
      return;
    }
    
    const pathElements = props.path.split(".");
    if (pathElements.length === 0) {
      return;
    }

    const lastIndexerOmittedPathElement = pathElements[pathElements.length - 1].replace(/\[[0-9]]/g, "");
    return getDisplayName(lastIndexerOmittedPathElement);
  }, []);

  const validationMessageClassName = compute<string | undefined>(() => {
    if (formContext?.errorCollection.isValidated) {
      if (errorMessage) {
        return "field-validation-error";
      }

      return "field-validation-valid";
    }
  });

  const contextPayload = useMemo<FormFieldContextPayload>(() => {
    return {
      isValidated: !!formContext?.errorCollection.isValidated,
      hasError: !!errorMessage,
      path: props.path,
      displayName: displayName ?? undefined
    };
  }, [formContext?.errorCollection.details, displayName]);

  // Template.
  return (
    <div className={joinClassName(
      props.className,
      "form-field flex flex-col justify-stretched",
    )}>
      {/* Label */}
      {displayName && (
        <label htmlFor={props.path}>
          {displayName}
        </label>
      )}

      {/* Input */}
      <FormFieldContext.Provider value={contextPayload}>
        {props.children}
      </FormFieldContext.Provider>

      {/* Message */}
      <span className={validationMessageClassName}>
        {errorMessage}
      </span>
    </div>
  );
}