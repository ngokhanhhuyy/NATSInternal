import React, { useState, useMemo, createContext } from "react";
import { ValidationError, OperationError } from "@/api";
import { createErrorCollectionModel } from "@/models";
import { useTsxHelper } from "@/helpers";
import styles from "./Form.module.css";

// Type.
type SubmissionState = "notSubmitting" | "submitting" | "submissionSucceeded";

// Payload.
type FormContextPayload = {
  errorCollection: ErrorCollectionModel;
  submissionState: SubmissionState;
};

// Context.
export const FormContext = createContext<FormContextPayload>(undefined!);

// Props.
type FormProps<T> = {
  submitAction: () => Promise<T>;
  onSubmissionSucceeded?: (result: T) => any;
  onSubmissionFailed?: (error: Error, errorHandled: boolean) => any;
  submissionSucceededText?: string;
} & React.ComponentPropsWithoutRef<"form">;

// Component.
export default function Form<T>(props: FormProps<T>) {
  // Props.
  const { submitAction, onSubmissionSucceeded, onSubmissionFailed, submissionSucceededText, ...domProps } = props;
  
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
  const contextValue = useMemo(() => ({ errorCollection, submissionState }), [errorCollection, submissionState]);

  // Callbacks.
  async function handleSubmitAsync(event: React.FormEvent): Promise<void> {
    event.preventDefault();
    setErrorCollection(errorCollection => errorCollection.clear());
    setSubmissionState("submitting");

    try {
      const result = await submitAction();
      onSubmissionSucceeded?.(result);
      setSubmissionState("submissionSucceeded");
    } catch (error) {
      if (error instanceof ValidationError || error instanceof OperationError) {
        setErrorCollection(errorCollection => errorCollection.mapFromApiErrorDetails(error.errors));
        onSubmissionFailed?.(error, true);
        return;
      }

      onSubmissionFailed?.(error as Error, false);
      throw error;
    } finally {
      setSubmissionState("notSubmitting");
    }
  }

  // Template.
  return (
    <FormContext.Provider value={contextValue}>
      <form
        {...domProps}
        className={joinClassName(domProps.className, submittingClassName, "transition transition-500")}
        noValidate
        onSubmit={handleSubmitAsync}
      >
        {submissionState !== "submissionSucceeded" && domProps.children}
        {submissionState === "submissionSucceeded" && (
          <div className={styles.submissionSucceededAnnouncement}>
            {submissionSucceededText ?? "Lưu thành công"}
          </div>
        )}
      </form>
      
    </FormContext.Provider>
  );
}