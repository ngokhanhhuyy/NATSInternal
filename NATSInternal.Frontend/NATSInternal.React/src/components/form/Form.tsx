import React, { useState, useMemo, createContext } from "react";
import { ValidationError, OperationError } from "@/api";
import { createErrorCollectionModel } from "@/models";
import { useTsxHelper } from "@/helpers";

// Type.
export type SubmissionState = "notSubmitting" | "submitting" | "submissionSucceeded";
export type SubmissionType = "upsert" | "delete";

// Payload.
type FormContextPayload = {
  errorCollection: ErrorCollectionModel;
  submissionState: SubmissionState;
  submissionType: SubmissionType | null;
  handleDeletionAsync?(): Promise<void>;
};

// Context.
export const FormContext = createContext<FormContextPayload | null>(null);

// Props.
type FormProps<TUpsertResult> = {
  upsertAction: () => Promise<TUpsertResult>;
  onUpsertingSucceeded?: (result: TUpsertResult) => any;
  onUpsertingFailed?: (error: Error, errorHandled: boolean) => any;
  deleteAction?: () => Promise<void>;
  onDeletionSucceeded?: () => any;
  onDeletionFailed?: (error: Error, errorHandled: boolean) => any;
  submissionSucceededText?: string;
  showValidState?: boolean;
  showSucceededAnnouncement?: boolean;
} & React.ComponentPropsWithoutRef<"form">;

// Component.
export default function Form<TUpsertResult>(props: FormProps<TUpsertResult>) {
  // Props.
  const {
    upsertAction,
    onUpsertingSucceeded,
    onUpsertingFailed,
    deleteAction,
    onDeletionSucceeded,
    onDeletionFailed,
    submissionSucceededText,
    showSucceededAnnouncement,
    ...domProps
  } = props;
  
  // Dependencies.
  const { compute, joinClassName } = useTsxHelper();

  // States.
  const [errorCollection, setErrorCollection] = useState(createErrorCollectionModel);
  const [submissionState, setSubmissionState] = useState<SubmissionState>("notSubmitting");
  const [submissionType, setSubmissionType] = useState<SubmissionType | null>(null);

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
      submissionType,
      handleDeletionAsync
    };
  }, [errorCollection, submissionState]);

  // Callbacks.
  async function handleUpsertingAsync(event: React.FormEvent): Promise<void> {
    event.preventDefault();
    setErrorCollection(errorCollection => errorCollection.clear());
    setSubmissionState("submitting");
    setSubmissionType("upsert");

    try {
      const result = await upsertAction();
      onUpsertingSucceeded?.(result);
      setSubmissionState("submissionSucceeded");
    } catch (error) {
      setSubmissionState("notSubmitting");
      setSubmissionType(null);
      if (error instanceof ValidationError || error instanceof OperationError) {
        setErrorCollection(errorCollection => errorCollection.mapFromApiErrorDetails(error.errors));
        onUpsertingFailed?.(error, true);
        return;
      }

      onUpsertingFailed?.(error as Error, false);
      throw error;
    }
  }

  async function handleDeletionAsync(): Promise<void> {
    setSubmissionState("submitting");
    setSubmissionType("upsert");

    try {
      await deleteAction?.();
      onDeletionSucceeded?.();
      setSubmissionState("submissionSucceeded");
    } catch (error) {
      setSubmissionState("notSubmitting");
      setSubmissionType(null);
      if (error instanceof ValidationError || error instanceof OperationError) {
        setErrorCollection(errorCollection => errorCollection.mapFromApiErrorDetails(error.errors));
        onDeletionFailed?.(error, true);
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
        {submissionState === "submissionSucceeded" && showSucceededAnnouncement
          ? (
            <div className="bg-success/20 border border-success rounded-lg flex justify-center items-center">
              <span className="text-success brightness-80 font-lg mx-2.5 my-7.5">
                {submissionSucceededText ?? "Lưu thành công"}
              </span>
            </div>
          ) : domProps.children}
      </form>
    </FormContext.Provider>
  );
}