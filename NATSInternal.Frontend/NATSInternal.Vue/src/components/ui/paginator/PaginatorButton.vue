<script setup lang="ts">
import { computed, inject } from "vue";
import { PaginatorProvidePayloadKey } from "./Paginator.vue";
import { createReusableTemplate } from "@vueuse/core";

// Dependencies.
const paginatorPayload = inject(PaginatorProvidePayloadKey)!;

// Props and emits.
const props = defineProps<{
  page: number;
  isSeparatedFirst: boolean;
  isSeparatedLast: boolean;
}>();


// Computed.
const smScreen = computed(() => paginatorPayload.paginationRanges.value.smScreen);
const xsScreen = computed(() => paginatorPayload.paginationRanges.value.xsScreen);

const isActive = computed<boolean>(() => props.page === paginatorPayload.currentPage.value);

const isExceedingXsScreenRange = computed(() => {
  return props.page < xsScreen.value.startingPage || props.page > xsScreen.value.endingPage;
});

const isExceedingSmScreenRange = computed(() => {
  return props.page < smScreen.value.startingPage || props.page > smScreen.value.endingPage;
});

const className = computed<(string | undefined)[]>(() => ([
  "btn min-w-8.5",
  paginatorPayload.getClassName?.(1, paginatorPayload.currentPage.value === 1)
]));

// Callbacks.
const handleClicked = (): void => {
  emit("clicked");
};

// Template.
const [DefinePageButton, PageButton] = createReusableTemplate();
</script>

<template>
  <DefinePageButton>
    <button type="button" v-bind:class="[`btn min-w-8.5`, className]" v-on:click="handleClicked">
      {{ 1 }}
    </button>
  </DefinePageButton>

  <div v-if="isExceedingSmScreenRange" v-bind:class="[`flex gap-2`, isExceedingSmScreenRange && `hidden`]">
    <PageButton />
  </div>

  <
</template>