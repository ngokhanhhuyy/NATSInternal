<script setup lang="ts">
import { computed } from "vue";
import { RouterLink } from "vue-router";
import { useNavigationBarStore } from "@/stores";
import { getDisplayName } from "@/metadata";

// Props and emits.
const props = defineProps<{
  name: string;
  fallbackDisplayName?: string;
}>();

const emit = defineEmits<{
  (event: "clicked"): void;
}>();

// Dependencies.
const navigationBarStore = useNavigationBarStore();

// Computed.
const displayName = computed<string | undefined>(() => getDisplayName(props.name) ?? props.fallbackDisplayName);
const isActive = computed<boolean>(() => {
  return navigationBarStore.activeItemName === props.name;
});
</script>

<template>
  <RouterLink v-bind:class="{ 'active': isActive }" v-bind:to="{ name }" v-on:click="emit('clicked')">
    <slot class="size-5" v-bind="{ isActive }"></slot>
    <span v-if="navigationBarStore.isExpanded" class="inline-block md:hidden lg:inline-block">
      {{ displayName }}
    </span>

    <div class="tooltip">
      {{ displayName }}
    </div>
  </RouterLink>
</template>