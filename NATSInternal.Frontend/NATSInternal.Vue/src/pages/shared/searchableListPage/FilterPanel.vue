<script setup lang="ts" generic="TListModel extends ListModel<TItemModel>, TItemModel extends object">
import { computed } from "vue";
import { getDisplayName } from "@/metadata";
import { useDirtyModelChecker } from "@/composables";

// Child components.
import type { ListModel } from "./SearchableListPage.vue";
import { FormField, TextInput, SelectInput, type SelectInputOption } from "@/components/form";
import { ArrowPathIcon } from "@heroicons/vue/24/outline";
import { BarsArrowUpIcon, BarsArrowDownIcon } from "@heroicons/vue/24/outline";

// Emits and model.
const emit = defineEmits<{
  (event: "reload-button-clicked"): void;
}>();

const model = defineModel<TListModel>({ required: true });

// States.
const { isDirty, setOriginalModel } = useDirtyModelChecker(model.value);

// Computed.
const sortByFieldNameOptions = computed<SelectInputOption[]>(() => {
  return model.value.sortByFieldNameOptions.map((fieldName) => ({
    value: fieldName,
    displayName: getDisplayName(fieldName) ?? undefined
  }));
});

const resultsPerPageOptions = [15, 20, 30, 40, 50].map(resultsPagePage => ({
  value: resultsPagePage.toString(),
  displayName: `${resultsPagePage.toString()} kết quả`
}));

// Callbacks.
const handleReloadButtonClicked = () => {
  emit("reload-button-clicked");
  setOriginalModel(model.value);
};
</script>

<template>
  <div class="panel mt-3 md:mt-5">
    <div class="panel-header">
      <div class="panel-header-title">
        Tuỳ chọn lọc và sắp xếp
      </div>
    </div>

    <div class="panel-body p-3">
      <div class="flex flex-col gap-3">
        <!-- Search content and advanced filter toggle button -->
        <FormField path="searchContent" display-name="Tìm kiếm">
          <TextInput
            v-model="model.searchContent"
            placeholder="Tìm kiếm"
            autoComplete="off"
          />
        </FormField>

        <div class="grid grid-cols-1 sm:grid-cols-3 lg:grid-cols-1 xl:grid-cols-3 gap-3">
          <FormField path="sortByFieldName">
            <SelectInput
              v-model="model.sortByFieldName"
              v-bind:options="sortByFieldNameOptions"
            />
          </FormField>

          <FormField path="sortByAscending">
            <button
              type="button"
              class="form-control justify-start gap-2"
              v-on:click="model.sortByAscending = !model.sortByAscending"
            >
              <template v-if="model.sortByAscending">
                <BarsArrowDownIcon />
                <span>Từ nhỏ đến lớn</span>
              </template>
              <template v-else>
                <BarsArrowUpIcon />
                <span>Từ lớn đến nhỏ</span>
              </template>
            </Button>
          </FormField>

          <FormField path="resultsPerPage">
            <SelectInput
              v-bind:options=resultsPerPageOptions
              v-bind:model-value="model.resultsPerPage.toString()"
              v-on:update:model-value="(resultsAsString) => model.resultsPerPage = parseInt(resultsAsString)"
            />
          </FormField>
        </div>

        <slot></slot>

        <div class="flex justify-end">
          <Button v-bind:class="[`gap-1`, isDirty && `btn-primary`]" v-on:click="handleReloadButtonClicked">
            <ArrowPathIcon />
            <span>Tải lại kết quả</span>
          </Button>
        </div>
      </div>
    </div>
  </div>
</template>