import React, { useState, useRef, useMemo, createContext } from "react";
import { ValidationError, OperationError } from "@/api";
import { createErrorCollectionModel } from "@/models";
import { joinClassName, compute } from "@/helpers";

// Type.
export type SubmissionState = "notSubmitting" | "submitting" | "submissionSucceeded";

// Payload.
type FormContextPayload = {
  errorCollection: ErrorCollectionModel;
  submissionState: SubmissionState;
  isModelDirty?: boolean;
};

// Context.
export const FormContext = createContext<FormContextPayload | null>(null);

// Props.
type FormProps<TUpsertResult> = {
  upsertAction: () => Promise<TUpsertResult>;
  onUpsertingSucceeded?: (result: TUpsertResult) => any;
  onUpsertingFailed?: (error: Error, errorHandled: boolean) => any;
  isModelDirty?: boolean;
  submitOnEnterKeyPressed?: boolean;
} & React.ComponentPropsWithoutRef<"form">;

// Component.
export default function Form<TUpsertResult>(props: FormProps<TUpsertResult>) {
  // Props.
  const {
    upsertAction,
    onUpsertingSucceeded,
    onUpsertingFailed,
    isModelDirty,
    autoComplete = "off",
    ...domProps
  } = props;

  // States.
  const [errorCollection, setErrorCollection] = useState(createErrorCollectionModel);
  const [submissionState, setSubmissionState] = useState<SubmissionState>("notSubmitting");
  const elementRef = useRef<HTMLFormElement | null>(null);

  // Computed.
  const submittingClassName = compute(() => {
    if (submissionState === "submitting") {
      return "opacity-50 pointer-events-none";
    }
  });
  
  const contextValue = useMemo<FormContextPayload>(() => {
    return {
      errorCollection,
      submissionState,
      isModelDirty
    };
  }, [errorCollection, submissionState, isModelDirty]);

  // Callbacks.
  function handleKeyPressed(event: React.KeyboardEvent): void {
    if (event.key === "Enter") {
      if (!props.submitOnEnterKeyPressed) {
        event.preventDefault();
      }

      if (!document.activeElement || !elementRef.current?.contains(document.activeElement)) {
        return;
      }

      const typesToCheck = [HTMLInputElement, HTMLButtonElement, HTMLSelectElement, HTMLTextAreaElement] as const;
      for (const typeToCheck of typesToCheck) {
        if (document.activeElement instanceof typeToCheck) {
          document.activeElement.blur();
          return;
        }
      }
    }
  }

  async function handleUpsertingAsync(event: React.FormEvent): Promise<void> {
    event.preventDefault();
    setErrorCollection(errorCollection => errorCollection.clear());
    setSubmissionState("submitting");

    try {
      const result = await upsertAction();
      onUpsertingSucceeded?.(result);
      setSubmissionState("submissionSucceeded");
    } catch (error) {
      setSubmissionState("notSubmitting");
      if (error instanceof ValidationError || error instanceof OperationError) {
        setErrorCollection(errorCollection => errorCollection.mapFromApiErrorDetails(error.errors));
        onUpsertingFailed?.(error, true);
        return;
      }

      onUpsertingFailed?.(error as Error, false);
      throw error;
    }
  }

  // Template.
  return (
    <FormContext.Provider value={contextValue}>
      <form
        {...domProps}
        autoComplete={autoComplete}
        ref={elementRef}
        className={joinClassName(
          domProps.className,
          submittingClassName,
          "transition transition-500",
          submissionState === "submitting" && "cursor-wait"
        )}
        noValidate
        onSubmit={handleUpsertingAsync}
        onKeyDown={handleKeyPressed}
      >
        {domProps.children}
      </form>
    </FormContext.Provider>
  );
}
