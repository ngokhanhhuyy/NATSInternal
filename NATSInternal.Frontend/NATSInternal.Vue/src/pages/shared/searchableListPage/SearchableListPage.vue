<script lang="ts">
export type ListModel<TItemModel extends object> =
  ISearchableListModel<TItemModel> &
  ISortableListModel<TItemModel> &
  IPageableListModel<TItemModel> &
  IUpsertableListModel<TItemModel>;
</script>

<script setup lang="ts" generic="TListModel extends ListModel<TItemModel>, TItemModel extends object">
import { getDisplayName } from "@/metadata";

// Child components.
import FilterPanel from "./FilterPanel.vue";
import { MainContainer } from "@/components/layouts";
import { Paginator } from "@/components/ui";
import { PlusIcon } from "@heroicons/vue/24/outline";

// Props, emits and model.
const props = defineProps<{
  resourceName: string;
  isReloading: boolean;
}>();

const model = defineModel<TListModel>({ required: true });
const emit = defineEmits<{
  (event: "paginator-page-changed", page: number): void;
  (event: "filter-panel-reload-button-clicked"): void;
}>();

// Computed.
const displayName = getDisplayName(props.resourceName);

const getPageButtonClassName = (_: number, isActive: boolean) => isActive ? "btn-primary" : undefined;

// Callbacks.
const handleFilterPanelReloadButtonClicked = () => emit("filter-panel-reload-button-clicked");
</script>

<template>
  <MainContainer class="gap-3" v-bind:is-loading="isReloading">
    <div class="flex flex-col items-stretch gap-3">
      <slot></slot>

      <div class="flex justify-end gap-3">
        <Paginator
          v-model="model.page"
          :pageCount="model.pageCount"
          :get-page-button-class-name="getPageButtonClassName"
        />

        <div v-if="model.pageCount > 1" class="border-r border-black/25 dark:border-white/25 w-px"></div>
        
        <Link class="btn gap-1 shrink-0" to="props.model.createRoutePath">
          <PlusIcon class="size-4.5" />
          <span>Tạo {{ displayName?.toLowerCase() }} mới</span>
        </Link>
      </div>
      
      <slot name="link-buttons"></slot>

      <FilterPanel v-model="model" :reload-button-click="handleFilterPanelReloadButtonClicked">
        <slot name="filter-panel-content"></slot>
      </FilterPanel>
    </div>

    <slot name="additional-panels"></slot>
  </MainContainer>
</template>