import React, { useState, useMemo, createContext } from "react";
import { ValidationError, OperationError } from "@/api";
import { createErrorCollectionModel } from "@/models";
import { useTsxHelper } from "@/helpers";

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
} & React.ComponentPropsWithoutRef<"form">;

// Component.
export default function Form<TUpsertResult>(props: FormProps<TUpsertResult>) {
  // Props.
  const { upsertAction, onUpsertingSucceeded, onUpsertingFailed, isModelDirty, ...domProps } = props;
  
  // Dependencies.
  const { compute, joinClassName } = useTsxHelper();

  // States.
  const [errorCollection, setErrorCollection] = useState(createErrorCollectionModel);
  const [submissionState, setSubmissionState] = useState<SubmissionState>("notSubmitting");

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
        className={joinClassName(
          domProps.className,
          submittingClassName,
          "transition transition-500",
          submissionState === "submitting" && "cursor-wait"
        )}
        noValidate
        onSubmit={handleUpsertingAsync}
      >
        {domProps.children}
      </form>
    </FormContext.Provider>
  );
}