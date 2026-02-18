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
    return route.matched.filter(m => m.name === "main-page").length === 1;
  });

  const activeItemName = computed<RouteRecordNameGeneric | undefined>(() => {
    if (shouldBeRendered) {
      const mainPageRouteIndex = route.matched.map(matchedRoute => matchedRoute.name).indexOf("main-page");
      return route.matched[mainPageRouteIndex + 1]?.name;
    }
  });

  // Watch.
  watch(breakpoints.md, () => {
    if (breakpoints.md.value) {
      isExpanded.value = false;
    }

    console.log(breakpoints.md.value);
  });

  return {
    isExpanded: readonly(isExpanded),
    shouldBeRendered: readonly(shouldBeRendered),
    activeItemName,
    expand: () => isExpanded.value = true,
    collapse: () => isExpanded.value = false,
    toggle: () => isExpanded.value = !isExpanded.value
  };
});