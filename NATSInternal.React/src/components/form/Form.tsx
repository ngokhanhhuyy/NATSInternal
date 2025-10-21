import React, { useState, useMemo, createContext } from "react";
import { ValidationError, OperationError } from "@/api";
import { createErrorCollectionModel } from "@/models";
import { useTsxHelper } from "@/helpers";

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
  // Dependencies.
  const { compute, joinClassName } = useTsxHelper();

  // States.
  const [errorCollection, setErrorCollection] = useState(createErrorCollectionModel);
  const [submissionState, setSubmissionState] = useState<SubmissionState>("notSubmitting");

  // Computed.
  const opacityClassName = compute(() => submissionState === "submitting" ? "opacity-50" : undefined);
  const contextValue = useMemo(() => ({ errorCollection, submissionState }), [errorCollection, submissionState]);

  // Callbacks.
  async function handleSubmitAsync(event: React.FormEvent): Promise<void> {
    event.preventDefault();
    setErrorCollection(errorCollection => errorCollection.clear());
    setSubmissionState("submitting");

    try {
      const result = await props.submitAction();
      props.onSubmissionSucceeded?.(result);
      setSubmissionState("submissionSucceeded");
    } catch (error) {
      if (error instanceof ValidationError || error instanceof OperationError) {
        setErrorCollection(errorCollection => errorCollection.mapFromApiErrorDetails(error.errors));
        props.onSubmissionFailed?.(error, true);
        return;
      }

      props.onSubmissionFailed?.(error as Error, false);
      throw error;
    } finally {
      setSubmissionState("notSubmitting");
    }
  }

  // Template.
  return (
    <FormContext.Provider value={contextValue}>
      {submissionState === "submissionSucceeded" ? (
        <div className={joinClassName(
          "bg-success-subtle border border-success rounded-3",
          "d-flex justify-content-center align-items-center p-5"
        )}>
          <i className="bi bi-check-circle-fill me-1"/>
          {props.submissionSucceededText ?? "Lưu thành công"}
        </div>
      ) : (
        <form
          {...props}
          className={joinClassName(props.className, opacityClassName)}
          noValidate
          onSubmit={handleSubmitAsync}
        >
          {props.children}
        </form>
      )}
    </FormContext.Provider>
  );
}