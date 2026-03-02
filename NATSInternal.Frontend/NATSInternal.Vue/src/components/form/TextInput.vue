<script setup lang="ts">
// Child components.
import Input from "./Input.vue";

// Props and model.
const props = withDefaults(defineProps<{
  type?: "text" | "password" | "tel" | "email";
}>(), {
  type: "text"
});

const model = defineModel<string>({ required: true });
</script>

<template>
  <Input v-slot="{ className, path, displayName }">
    <input
      v-bind:type="props.type"
      v-bind:class="className"
      v-bind:path="path"
      v-bind:placeholder="displayName"
      v-model="model"
    />
  </Input>
</template>