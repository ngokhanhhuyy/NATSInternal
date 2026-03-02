<script lang="ts">
import type { InjectionKey, Ref } from "vue";
import type { PaginationRange } from "@/helpers";

type PaginationRanges = {
  smScreen: PaginationRange,
  xsScreen: PaginationRange,
};

type PaginatorProvidePayload = {
  currentPage: Ref<number>;
  paginationRanges: Ref<PaginationRanges>;
  getClassName?(currentPage: number, isActive: boolean): string | undefined;
};
export const PaginatorProvidePayloadKey: InjectionKey<PaginatorProvidePayload> = Symbol("paginator-provide-payload");
</script>

<script setup lang="ts">
import { computed, provide } from "vue";
import { usePaginationHelper } from "@/helpers";

// Dependencies.
const { getPaginationRange } = usePaginationHelper();

// Props, emits and model.
const props = defineProps<{
  pageCount: number,
  getPageButtonClassName?(currentPage: number, isActive: boolean): string | undefined;
}>();

const model = defineModel<number>({ required: true });

// Computed.
const paginationRanges = computed<PaginationRanges>(() => ({
  smScreen: getPaginationRange({
    currentPage: model.value,
    pageCount: props.pageCount,
    visibleButtons: 5
  }),
  xsScreen: getPaginationRange({
    currentPage: model.value,
    pageCount: props.pageCount,
    visibleButtons: 3
  }),
}));

const pages = computed<number[]>(() => {
  const startingPage = paginationRanges.value.smScreen.startingPage;
  const endingPage = paginationRanges.value.smScreen.endingPage;
  const arrayLength = endingPage - (startingPage - 1);

  return Array.from({ length: arrayLength }, (_, index) => index + startingPage);
});

const isPageExceedingXsScreenRange = (page: number) => {
  return page < paginationRanges.value.xsScreen.startingPage || page > paginationRanges.value.xsScreen.endingPage;
};

const isPageExceedingSmScreenRange = (page: number) => {
  return page < paginationRanges.value.smScreen.startingPage || page > paginationRanges.value.smScreen.endingPage;
};

const computeButtonClassName = (page: number, isSeparatedButton: boolean = false): string => {
  const names: (string | false | undefined)[] = [
    "btn min-w-8.5",
    (isSeparatedButton && isPageExceedingXsScreenRange(page)) && "hidden sm:flex",
    props.getPageButtonClassName?.(page, model.value === page)
  ];

  return names.filter(n => n).join(" ");
};

// Callbacks.
const handlePageButtonClick = (page: number): void => {
  model.value = page;
};

// Provide.
provide(PaginatorProvidePayloadKey, {
  currentPage: model,
  paginationRanges,
  getClassName: props.getPageButtonClassName
});
</script>

<template>
  <div class="flex flex-row justify-center gap-2">
    <!-- Separated first page button -->
    <template v-if="isPageExceedingSmScreenRange(1)">
      <div :class="[`flex gap-2`, !isPageExceedingXsScreenRange(1) && `hidden`]">
        <button type="button" :class="computeButtonClassName(1)" @click="handlePageButtonClick(1)">
          {{ 1 }}
        </button>
        <span>...</span>
      </div>
    </template>

    <template v-for="page in pages" :key="page">
      <button type="button" :class="computeButtonClassName" @click="handlePageButtonClick(page)">
        {{ page }}
      </button>
    </template>

    <!-- Separated last page button -->
    <template v-if="isPageExceedingSmScreenRange(pageCount)">
      <div :class="[`flex gap-2`, !isPageExceedingXsScreenRange(pageCount) && `hidden`]">
        <span>...</span>
        <button
          type="button"
          :class="computeButtonClassName(pageCount)"
          @click="handlePageButtonClick(pageCount)"
        >
          {{ pageCount }}
        </button>
      </div>
    </template>
  </div>
</template>