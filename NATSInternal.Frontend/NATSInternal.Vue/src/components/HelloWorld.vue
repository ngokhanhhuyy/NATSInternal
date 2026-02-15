<script setup lang="tsx">
import { ref } from "vue";
import UserList from "./testing/User.vue";

defineProps<{ msg: string }>();

const count = ref<({ model: string })[]>(null!);
</script>

<template>
</template>

<style scoped>
  .read-the-docs {
    color: #888;
  }
</style>