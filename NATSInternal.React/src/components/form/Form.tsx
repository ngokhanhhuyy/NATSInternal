import { createSignal, createContext, batch, Show } from "solid-js";
import { ValidationError, OperationError } from "@/api";
import { createErrorCollectionModel } from "@/models";
import { useHTMLHelper } from "@/helpers";

// Type.
type SubmissionState = "notSubmitting" | "submitting" | "submissionSucceeded";

// Payload.
type FormContextPayload = {
  getErrorCollection: () => ErrorCollectionModel;
  getSubmissionState: () => SubmissionState;
};

// Context.
export const FormContext = createContext<FormContextPayload>();

// Props.
type FormProps<T> = {
  submitAction: () => Promise<T>;
  onSubmissionSucceeded?: (result: T) => any;
  onSubmissionFailed?: (error: Error, errorHandled: boolean) => any;
  submissionSucceededText?: string;
} & JSX.FormElementProps;

// Component.
export default function Form<T>(props: FormProps<T>) {
  // Dependencies.
  const htmlHelper = useHTMLHelper();

  // States.
  const [getErrorCollection, setErrorCollection] = createSignal(createErrorCollectionModel());
  const [getSubmissionState, setSubmissionState] = createSignal<SubmissionState>("notSubmitting");

  // Computed.
  const computeOpacityClassName = (): string | undefined => {
    if (getSubmissionState() === "submitting") {
      return "opacity-50";
    }
  };

  // Callbacks.
  async function handleSubmitAsync(event: SubmitEvent): Promise<void> {
    event.preventDefault();
    batch(() => {
      setErrorCollection(errorCollection => errorCollection.clear());
      setSubmissionState("submitting");
    });

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

      props.onSubmissionFailed?.(error, false);
      throw error;
    } finally {
      setSubmissionState("notSubmitting");
    }
  }

  // Template.
  function renderFallback() {
    return (
      <div class={htmlHelper.joinClassName(
        "bg-success-subtle border border-success rounded-3",
        "d-flex justify-content-center align-items-center p-5"
      )}>
        <i class="bi bi-check-circle-fill me-1" />
        {props.submissionSucceededText ?? "Lưu thành công"}
      </div>
    );
  }

  return (
    <FormContext.Provider value={{ getErrorCollection, getSubmissionState }}>
      <Show
        when={getSubmissionState() !== "submissionSucceeded"}
        fallback={renderFallback()}
      >
        <form
          {...props}
          class={htmlHelper.joinClassName(props.class, computeOpacityClassName())}
          noValidate
          onSubmit={handleSubmitAsync}
        >
          {props.children}
        </form>
      </Show>
    </FormContext.Provider>
  );
}