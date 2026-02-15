<script setup lang="ts">
import { RouterLink } from "vue-router";

// Child components.
import { StarIcon } from "@heroicons/vue/24/solid";
</script>

<template>
  <RouterLink id="main-logo" v-bind:to="{ name: 'home' }">
    <StarIcon />
    <span>natsinternal</span>
  </RouterLink>
</template>