<script lang="ts">
import type { Snippet } from "svelte";
import type { HTMLAttributes } from "svelte/elements";
import { getDisplayName } from "@/metadata";
import { getFormContext } from "./Form.svelte";
import { setFormFieldContext } from "./FormField";

// Props.
const props: {
  path?: string;
  displayName?:
  string; children: Snippet
} & HTMLAttributes<HTMLDivElement> = $props();
const formContextPayload = getFormContext();

// Computed.
const errorMessage = $derived.by(() => {
  if (!formContextPayload || !formContextPayload.errorCollection.isValidated || !props.path) {
    return;
  }

  const messages = formContextPayload.errorCollection.details
    .filter(d => d.propertyPath === props.path)
    .map(d => d.message);

  if (messages.length === 0) {
    return;
  }

  return messages[0];
});

const displayName = $derived.by(() => {
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
  return getDisplayName(lastIndexerOmittedPathElement) ?? undefined;
});

const validationMessageClassName = $derived.by<string | undefined>(() => {
  if (formContextPayload?.errorCollection.isValidated) {
    if (errorMessage) {
      return "field-validation-error";
    }

    return "field-validation-valid";
  }
});

setFormFieldContext({
  get isValidated(): boolean {
    return formContextPayload?.errorCollection.isValidated;
  },
  get hasError(): boolean {
    return !!errorMessage;
  },
  get path(): string | undefined {
    return props.path;
  },
  get displayName(): string | undefined {
    return props.displayName;
  }
});
</script>

<div class="form-field flex flex-col justify-stretched">
  {!!errorMessage}

  {#if displayName}
    <label for={props.path}>
      {displayName}
    </label>
  {/if}

  {@render props.children?.()}
</div>