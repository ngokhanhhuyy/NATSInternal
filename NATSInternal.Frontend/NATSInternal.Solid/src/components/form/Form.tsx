import { createMutable, createContext, createMemos, batch, Show } from "@/solid";
import { ValidationError, OperationError } from "@/api";
import { createErrorCollectionModel } from "@/models";
import { useTsxHelper } from "@/helpers";

// Type.
type SubmissionState = "notSubmitting" | "submitting" | "submissionSucceeded";

// Payload.
type FormContextPayload = Readonly<{
  errorCollection: ErrorCollectionModel;
  submissionState: SubmissionState;
}>;

// Context.
export const FormContext = createContext<FormContextPayload>();

// Props.
type FormProps<T> = {
  submitAction: () => Promise<T>;
  onSubmissionSucceeded?: (result: T) => any;
  onSubmissionFailed?: (error: Error, errorHandled: boolean) => any;
  submissionSucceededText?: string;
} & JSX.HTMLFormAttributes;

// Component.
export default function Form<T>(props: FormProps<T>) {
  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // States.
  const states = createMutable({
    errorCollection: createErrorCollectionModel(),
    submissionState: "notSubmitting" as SubmissionState
  });

  // Computed.
  const memos = createMemos({
    opacityClassName: (): string | undefined => {
      if (states.submissionState === "submitting") {
        return "opacity-50";
      }
    }
  });

  const contextPayload: FormContextPayload = {
    get errorCollection() { return states.errorCollection; },
    get submissionState() { return states.submissionState; }
  };

  // Callbacks.
  async function handleSubmitAsync(event: SubmitEvent): Promise<void> {
    event.preventDefault();
    batch(() => {
      states.errorCollection.clear();
      states.submissionState = "submitting";
    });

    try {
      const result = await props.submitAction();
      props.onSubmissionSucceeded?.(result);
      states.submissionState = "submissionSucceeded";
    } catch (error) {
      if (error instanceof ValidationError || error instanceof OperationError) {
        states.errorCollection = states.errorCollection.mapFromApiErrorDetails(error.errors);
        props.onSubmissionFailed?.(error, true);
        return;
      }

      props.onSubmissionFailed?.(error, false);
      throw error;
    } finally {
      states.submissionState = "notSubmitting";
    }
  }

  // Template.
  function renderFallback() {
    return (
      <div class={joinClassName(
        "bg-success-subtle border border-success rounded-3",
        "d-flex justify-content-center align-items-center p-5"
      )}>
        <i class="bi bi-check-circle-fill me-1" />
        {props.submissionSucceededText ?? "Lưu thành công"}
      </div>
    );
  }

  return (
    <FormContext.Provider value={contextPayload}>
      <Show
        when={states.submissionState !== "submissionSucceeded"}
        fallback={renderFallback()}
      >
        <form
          {...props}
          class={joinClassName(props.class, memos.opacityClassName)}
          noValidate
          onSubmit={handleSubmitAsync}
        >
          {props.children}
        </form>
      </Show>
    </FormContext.Provider>
  );
}