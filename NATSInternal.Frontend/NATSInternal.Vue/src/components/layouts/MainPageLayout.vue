<script lang="ts">
import type { RouteLocationRaw } from "vue-router";

type BreadcrumbItemData = { pageTitle: string | null; text: string; to: RouteLocationRaw | null };
</script>

<script setup lang="ts">
import { ref, computed, watch } from "vue";
import { useRoute, RouterView, RouterLink } from "vue-router";
import { useNavigationBarStore } from "@/stores";
import { createReusableTemplate, useTitle } from "@vueuse/core";

// Child components.
import { HomeIcon, ChevronRightIcon } from "@heroicons/vue/24/solid";

// Dependencies.
const route = useRoute();
const navigationBarStore = useNavigationBarStore();
const pageTitle = useTitle();

// States.
const shouldBlockPointerEvent = ref(false);

// Computed.
const breadcrumItemsData = computed<BreadcrumbItemData[]>(() => {
  return route.matched
    .filter((matchedRoute) => matchedRoute.meta.breadcrumbItem)
    .map<BreadcrumbItemData>((matchedRoute) => {
      const breadcrumbItem = typeof matchedRoute.meta.breadcrumbItem === "function"
        ? matchedRoute.meta.breadcrumbItem(route)
        : matchedRoute.meta.breadcrumbItem;

      return {
        pageTitle: matchedRoute.meta.pageTitle ?? null,
        text: breadcrumbItem.text,
        to: breadcrumbItem.to
      };
    });
});

const className = computed<string | undefined>(() => shouldBlockPointerEvent.value ? "pointer-events-none" : undefined);

const computeItemClassName = (to: RouteLocationRaw | null, isFirst: boolean, isLast: boolean): string | null => {
  const names: string[] = [];
  if (!isFirst && !isLast) {
    names.push("hidden sm:inline");
  } else {
    names.push("text-lg sm:text-base");
  }

  if (to == null || isLast) {
    names.push("pointer-events-none");
  }

  return names.length === 0 ? null : names.join(" ");
};

const computeSeparatorIconClassName = (index: number): string => {
  const names: string[] = ["size-4"];
  if (index) {
    names.push("hidden sm:inline");
  }

  return names.join(" ");
};

// Life cycle hooks.
watch(() => navigationBarStore.isExpanded, () => {
  setTimeout(() => shouldBlockPointerEvent.value = navigationBarStore.isExpanded, 200);
});

// Template.
const [BreadcrumbItemDefinition, BreadcrumbItem] = createReusableTemplate<{
  data: BreadcrumbItemData;
  isFirst: boolean;
  isLast: boolean;
}>();
</script>

<template>
  <div id="main-page-layout" v-bind:class="className">
    <div id="breadcrumb">
      <BreadcrumbItemDefinition v-slot="{ data: { to, text }, isFirst, isLast }">
        <RouterLink
          v-bind:class="computeItemClassName(to, isFirst, isLast)"
          v-bind:to="to ?? { name: `home` }"
        >
          <HomeIcon v-if="isFirst" class="size-5" />
          <template v-else-if="isLast">
            <span class="hidden sm:inline">{{ text }}</span>
            <span class="text-lg inline sm:hidden">{{ pageTitle }}</span>
          </template>
          <span v-else class="hidden sm:inline">{{ text }}</span>
        </RouterLink>
      </BreadcrumbItemDefinition>

      <div class="flex flex-wrap gap-3 items-center ms-2 lg:ms-3 translate-y-[7.5%] h-8">
        <template v-for="(data, index) of breadcrumItemsData" v-bind:key="index">
          <BreadcrumbItem
            v-bind:data="data"
            v-bind:is-first="index === 0"
            v-bind:is-last="index === breadcrumItemsData.length - 1"
          />
          <ChevronRightIcon
            v-if="index < breadcrumItemsData.length - 1"
            v-bind:class="computeSeparatorIconClassName(index)"
          />
        </template>
      </div>
    </div>

    <RouterView v-slot="{ Component }">
      <Transition name="fade" mode="out-in">
        <Suspense>
          <component v-bind:is="Component" />
        </Suspense>
      </Transition>
    </RouterView>
  </div>
</template>