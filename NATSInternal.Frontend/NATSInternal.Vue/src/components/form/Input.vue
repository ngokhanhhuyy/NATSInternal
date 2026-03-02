<script setup lang="ts">
import { computed, inject } from "vue";

// Child component.
import { FormFieldProvidePayloadKey } from "./FormField.vue";

// Dependencies.
const formFieldPayload = inject(FormFieldProvidePayloadKey);

// Slot payload.
const slotPayload = computed(() => ({
  className: "form-control",
  path: formFieldPayload?.path,
  displayName: formFieldPayload?.displayName
}));
</script>

<template>
  <slot v-bind="slotPayload"></slot>
</template>