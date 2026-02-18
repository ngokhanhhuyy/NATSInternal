<script setup lang="ts">
import { useRoute } from "vue-router";

// Dependencies.
const route = useRoute();

// Props.
const props = defineProps<{
  description: string;
  isLoading?: boolean;
}>();
</script>

<template>
  <div id="main-container">
    <div id="page-title-container">
        <span class="text-2xl">{{ route.meta.pageTitle }}</span>
        <span class="text-md opacity-50">{{ props.description }}</span>
    </div>

    <slot></slot>
  </div>
</template>