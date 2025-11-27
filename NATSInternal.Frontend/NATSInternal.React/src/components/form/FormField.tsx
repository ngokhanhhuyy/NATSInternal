import React, { useMemo, createContext, useContext } from "react";
import { getDisplayName } from "@/metadata";
import { useTsxHelper } from "@/helpers";

// Shared components.
import { FormContext } from "@/components/form/Form";

// Context.
export type FormFieldContextPayload = {
  isValidated: boolean;
  hasError: boolean;
  showValidState: boolean;
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
  const { compute, joinClassName } = useTsxHelper();

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

  const contextPayload = useMemo<FormFieldContextPayload>(() => {
    return {
      isValidated: !!formContext?.errorCollection.isValidated,
      hasError: !!errorMessage,
      showValidState: formContext?.showValidState ?? true,
      path: props.path,
      displayName: displayName ?? undefined
    };
  }, [formContext?.errorCollection.details, displayName]);
  
  const labelClassName = compute(() => {
    if (contextPayload.hasError) {
      return "text-red-500 dark:text-red-700";
    }

    if (contextPayload.isValidated && contextPayload.showValidState) {
      return "text-emerald-500";
    }

    return "text-black/50 dark:text-white/70";
  });

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
      {formContext?.errorCollection.isValidated && (
        <ErrorMessage message={errorMessage} showValidState={formContext.showValidState} />
      )}
    </div>
  );
}

function ErrorMessage(props: { message?: string; showValidState: boolean; }): React.ReactNode {
  const staticClassName = "text-sm";

  if (props.message) {
    return <div className={`text-red-500 dark:text-red-700 ${staticClassName}`}>{props.message}</div>;
  }

  if (props.showValidState) {
    return <div className={`text-emerald-500 dark:text-emerald-500 ${staticClassName}`}>Hợp lệ</div>;
  }

  return null;
}