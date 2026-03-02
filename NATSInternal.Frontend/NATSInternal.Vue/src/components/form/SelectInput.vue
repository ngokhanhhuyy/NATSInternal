<script lang="ts">
export type SelectInputOption = {
  value: string;
  displayName?: string;
};
</script>

<script setup lang="ts">
// Child components.
import Input from "./Input.vue";
import { Menu, MenuButton, MenuItem, MenuItems } from "@headlessui/vue";
import { CheckIcon } from "@heroicons/vue/24/outline";

// Props and models.
const props = defineProps<{
  options: SelectInputOption[];
  disabled?: boolean;
}>();

const model = defineModel<string>({ required: true });

// Computed.
const computeInputId = (path?: string) => path?.replaceAll(".", "__");
</script>

<template>
  <Input v-slot="{ path }">
    <Menu>
      <input type="hidden" v-bind:name="path" v-bind:id="computeInputId(path)" />

      <MenuButton class="menu-button">
        {{ props.options.find(option => option.value)?.displayName }}
      </MenuButton>

      <MenuItems class="menu-items" :modal="false" anchor="bottom start" transition>
        <MenuItem
          v-for="option, index in options"
          v-bindclass="[`menu-item`, option.value !== model && `ps-9`]"
          v-bindkey="index"
          v-on:click="model = option.value"
          as="div"
        >
          <CheckIcon v-if="option.value === model" class="size-4" />
          <span>{{ option.displayName ?? option.value }}</span>
        </MenuItem>
      </MenuItems>
      
    </Menu>
  </Input>
</template>

<style scoped>
.menu-button {
  @apply 
    form-control text-start hover:cursor-pointer
    data-open:border-blue-500 data-open:outline-blue-500
}

.menu-items {
  @apply
    bg-white/50 dark:bg-neutral-800/65 border border-black/25 dark:border-white/25 outline-none
    rounded-lg shadow-lg p-1.5 backdrop-blur-md
    w-(--button-width) [--anchor-gap:--spacing(1.5)] max-h-100 overflow-y-auto
    origin-center transition duration-200 ease-out data-closed:scale-95 data-closed:opacity-0
}

.menu-item {
  @apply
    flex items-center gap-2 cursor-pointer px-3 py-1 rounded-lg
    data-focus:bg-blue-500 data-focus:text-white;
}
</style>