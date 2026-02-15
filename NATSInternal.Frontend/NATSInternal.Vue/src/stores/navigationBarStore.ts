import { ref, computed, watch, readonly } from "vue";
import { useRoute, type RouteRecordNameGeneric } from "vue-router";
import { defineStore } from "pinia";
import { breakpointsTailwind, useBreakpoints } from "@vueuse/core";

export const useNavigationBarStore = defineStore("navigationBarStore", () => {
  const route = useRoute();
  const breakpoints = useBreakpoints(breakpointsTailwind);

  // States.
  const isExpanded = ref(breakpoints.md.value);

  // Computed
  const shouldBeRendered = computed<boolean>(() => {
    return route.matched[0]?.name !== "sign-in";
  });

  const activeItemName = computed<RouteRecordNameGeneric | undefined>(() => {
    return route.matched[0]?.name;
  });

  // Watch.
  watch(() => breakpoints.md, () => {
    if (breakpoints.md.value) {
      isExpanded.value = false;
    }
  });

  watch([route], () => console.log(route.matched), { immediate: true });

  return {
    isExpanded: readonly(isExpanded),
    shouldBeRendered: readonly(shouldBeRendered),
    activeItemName,
    expand: () => isExpanded.value = true,
    collapse: () => isExpanded.value = false,
    toggle: () => isExpanded.value = !isExpanded.value
  };
});