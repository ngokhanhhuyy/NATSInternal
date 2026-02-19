<script lang="ts" module>
import { createContext, type Snippet } from "svelte";
import type { HTMLFormAttributes } from "svelte/elements";

export type SubmissionState = "notSubmitting" | "submitting" | "submissionSucceeded";
export type FormContextPayload = {
  errorCollection: ErrorCollectionModel;
  submissionState: SubmissionState;
  handleDeletionAsync?(): Promise<void>;
  isModelDirty?: boolean;
};

const [getFormContext, setFormContext] = createContext<FormContextPayload>();
export { getFormContext };
</script>

<script lang="ts" generics="TUpsertResult">
import { ValidationError, OperationError } from "@/api";
import { createErrorCollectionModel } from "@/models";

// Props.
let props: {
  upsertAction: () => Promise<TUpsertResult>;
  onUpsertingSucceeded?: (result: TUpsertResult) => any;
  onUpsertingFailed?: (error: Error, errorHandled: boolean) => any;
  isModelDirty?: boolean;
  children: Snippet;
} & HTMLFormAttributes = $props();

// States.
const states = $state<FormContextPayload>({
  errorCollection: createErrorCollectionModel(),
  submissionState: "notSubmitting"
});

// Computed.
const className = $derived.by<any[]>(() => {
  return [
    states.submissionState === "submitting" && "opacity-50 pointer-events-none",
    "transition transition-500",
    states.submissionState === "submitting" && "cursor-wait"
  ];
});

// Callbacks.
async function handleUpsertingAsync(event: Event): Promise<void> {
  event.preventDefault();
  states.errorCollection.clear();
  states.submissionState = "submitting";

  try {
    const result = await props.upsertAction();
    props.onUpsertingSucceeded?.(result);
    states.submissionState = "submissionSucceeded";
  } catch (error) {
    states.submissionState = "notSubmitting";
    if (error instanceof ValidationError || error instanceof OperationError) {
      states.errorCollection.mapFromApiErrorDetails(error.errors);
      props.onUpsertingFailed?.(error, true);
      return;
    }

    props.onUpsertingFailed?.(error as Error, false);
    throw error;
  }
}

// Context.
setFormContext(states);
</script>

<form class={className} onsubmit={handleUpsertingAsync} novalidate>
  {@render props.children()}
</form>