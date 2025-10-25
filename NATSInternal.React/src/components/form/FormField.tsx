import React, { useMemo, createContext, useContext } from "react";
import { getDisplayName } from "@/metadata";
import { useTsxHelper } from "@/helpers";

// Shared components.
import { FormContext } from "@/components/form/Form";

// Context.
export type FormFieldContextPayload = { isValidated: boolean; hasError: boolean; displayName?: string };
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

    return messages[0];
  }, [formContext.errorCollection.details]);

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

    const lastIndexerOmittedPathElement = pathElements[pathElements.length - 1].replace(/\[[0-9]\]/g, "");
    return getDisplayName(lastIndexerOmittedPathElement);
  }, []);

  const contextPayload = useMemo<FormFieldContextPayload>(() => ({
    isValidated: !!formContext.errorCollection.isValidated,
    hasError: !!errorMessage,
    displayName: displayName ?? undefined
  }), [formContext.errorCollection.details, displayName]);
  
  const labelClassName = compute(() => contextPayload.hasError ? "text-red-300" : "text-black/50");

  // Template.
  return (
    <div className={joinClassName(props.className, "form-field flex flex-col justify-stretched")}>
      {/* Label */}
      {displayName && (
        <label htmlFor={props.path} className={joinClassName(labelClassName, "block text-sm")}>
          {displayName}
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