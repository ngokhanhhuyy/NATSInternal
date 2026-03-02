<script setup lang="ts">
import { onMounted } from "vue";

// Life-cycle hooks.
onMounted(() => window.scrollTo({ top: 0, behavior: "smooth" }));
</script>

<template>
  <div id="main-container">
    <slot></slot>
  </div>
</template>