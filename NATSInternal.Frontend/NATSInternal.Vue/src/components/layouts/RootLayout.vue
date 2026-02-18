<script setup lang="ts">
import { watch } from "vue";
import { useRoute, RouterView } from "vue-router";
import { useTitle } from "@vueuse/core";
import { useNavigationBarStore } from "@/stores";

// Child components.
import TopBar from "./navigationBar/TopBar.vue";
import NavigationBar from "./navigationBar/NavigationBar.vue";

// Dependencies.
const route = useRoute();
const navigationBarStore = useNavigationBarStore();
const pageTitle = useTitle();

// Watch.
watch(() => route.matched, (matched) => {
  const lastWithTitle = [...matched].reverse().find(r => r.meta?.pageTitle);
  if (lastWithTitle?.meta?.pageTitle) {
    pageTitle.value = `${lastWithTitle.meta.pageTitle} - NATSInternal`;
  }
}, { immediate: true, deep: true });
</script>

<template>
  <div id="root-layout">
    <TopBar should-render-navigation-bar-toggle-button={shouldRenderNavigationBar} />
    
    <main>
      <RouterView />
      <NavigationBar v-if="navigationBarStore.shouldBeRendered" />
    </main>
  </div>

</template>