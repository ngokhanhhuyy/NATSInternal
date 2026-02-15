<script setup lang="ts" vapor>
import { defineSlots as defineVueSlots } from "vue";
export type Model = {
  userName: string;
  roleId: number;
};
import { JSX } from "vue/jsx-runtime";

const model = defineModel<Model>({ required: true });

const emit = defineEmits<{
  (event: "modelChanged", newModel: Model): void;
}>();
</script>

<template>
  <input type="text" v-model="model.userName" />
  <slot name="header" v-bind="{ model }"></slot>
</template>`>`