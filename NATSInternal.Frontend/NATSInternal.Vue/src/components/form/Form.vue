<script lang="ts">
import { InjectionKey } from "vue";
// Types.
export type SubmissionState = "notSubmitting" | "submitting" | "submissionSucceeded";
export type FormProvidePayload = {
  errorCollection: ErrorCollectionModel;
  submissionState: SubmissionState;
  handleDeletionAsync?(): Promise<void>;
  isModelDirty?: boolean;
};

export const FormProvidePayloadKey: InjectionKey<FormProvidePayload> = Symbol("FormProvidePayload");
</script>

<script setup lang="ts" generic="TUpsertResult">
import { reactive, computed, provide } from "vue";
import { ValidationError, OperationError } from "@/api";
import { createErrorCollectionModel } from "@/models";


// Props and emits.
const props = defineProps<{
  upsertAction: () => Promise<TUpsertResult>;
  onUpsertingSucceeded?: (result: TUpsertResult) => any;
  onUpsertingFailed?: (error: Error, errorHandled: boolean) => any;
  isModelDirty?: boolean;
}>();

const emit = defineEmits<{
  (event: "upsertingSucceeded", result: TUpsertResult): void;
  (event: "upsertingFailed", error: Error, wasErrorHandled: boolean): void;
}>();

// States.
const states = reactive<FormProvidePayload>({
  errorCollection: createErrorCollectionModel(),
  submissionState: "notSubmitting"
});

// Provide.
provide<FormProvidePayload>(FormProvidePayloadKey, states);

// Computed.
const submittingClassName = computed<string | null>(() => {
  if (states.submissionState === "submitting") {
    return "opacity-50 pointer-events-none";
  }

  return null;
});

const className = computed<any[]>(() => {
  return [submittingClassName, `transition transition-500`, states.submissionState === "submitting" && "cursor-wait"];
});

// Callbacks.
async function handleUpsertingAsync(event: Event): Promise<void> {
  event.preventDefault();
  states.errorCollection.clear();
  states.submissionState = "submitting";

  try {
    const result = await props.upsertAction();
    emit("upsertingSucceeded", result);
    states.submissionState = "submissionSucceeded";
  } catch (error) {
    states.submissionState = "notSubmitting";
    if (error instanceof ValidationError || error instanceof OperationError) {
      states.errorCollection.mapFromApiErrorDetails(error.errors);
      emit("upsertingFailed", error, true);
      return;
    }

    emit("upsertingFailed", error as Error, false);
    throw error;
  }
}
</script>

<template>
  <form v-bind:class="className" v-on:submit="handleUpsertingAsync" novalidate>
    <slot></slot>
  </form>
</template>