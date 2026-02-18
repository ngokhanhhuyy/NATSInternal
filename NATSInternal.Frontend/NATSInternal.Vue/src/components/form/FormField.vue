<script lang="ts">
import type { InjectionKey } from "vue";

export type FormFieldProvidePayload = {
  isValidated: boolean;
  hasError: boolean;
  path?: string;
  displayName?: string
};

export const FormFieldProvidePayloadKey: InjectionKey<FormFieldProvidePayload> = Symbol("FormFieldProvidePayload");
</script>

<script setup lang="ts">
import { computed, provide, inject } from "vue";
import { getDisplayName } from "@/metadata";

// Shared components.
import { FormProvidePayloadKey } from "./Form.vue";

// Props.
const props = defineProps<{
  path?: string;
  displayName?: string;
}>();

// Inject.
const formProvidePayload = inject(FormProvidePayloadKey);

// Computed.
const errorMessage = computed(() => {
  if (!formProvidePayload || !formProvidePayload.errorCollection.isValidated || !props.path) {
    return;
  }

  const messages = formProvidePayload.errorCollection.details
    .filter(d => d.propertyPath === props.path)
    .map(d => d.message);

  if (messages.length === 0) {
    return;
  }

  return messages[0];
});

const displayName = computed(() => {
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
});

const validationMessageClassName = computed<string | undefined>(() => {
  if (formProvidePayload?.errorCollection.isValidated) {
    if (errorMessage) {
      return "field-validation-error";
    }

    return "field-validation-valid";
  }
});

// Provide.
provide(FormFieldProvidePayloadKey, {
    get isValidated(): boolean {
      return computed(() => !!formProvidePayload?.errorCollection.isValidated).value;
    },
    get hasError(): boolean {
      return computed(() => !!errorMessage.value).value;
    },
    get path(): string | undefined {
      return computed(() => props.path).value;
    },
    get displayName(): string | undefined {
      return computed(() => props.displayName ?? undefined).value;
    }
});
</script>

<template>
  <div class="form-field flex flex-col justify-stretched">
    {{ !!errorMessage }}
    <label v-if="displayName" v-bind:for="props.path">
      {{ displayName }}
    </label>

    <slot></slot>

    <span v-if="!!errorMessage" v-bind:class="validationMessageClassName">
      {{ errorMessage }}
    </span>
  </div>
</template>