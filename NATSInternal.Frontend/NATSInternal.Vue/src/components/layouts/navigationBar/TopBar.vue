<script setup lang="ts">
import { useNavigationBarStore, useThemeStore } from "@/stores";
import { useWindowScroll } from "@vueuse/core";

// Child component.
import MainLogo from "./MainLogo.vue";
import { Bars4Icon, MoonIcon, SunIcon, Cog6ToothIcon } from "@heroicons/vue/24/solid";

// Dependencies.
const navigationBarStore = useNavigationBarStore();
const themeStore = useThemeStore();
const { y } = useWindowScroll();
</script>

<template>
  <div id="topbar" v-bind:class="{ 'shadow-xs': y > 0 }">
    <div id="topbar-container">
      <!-- Main logo -->
      <MainLogo />

      <div></div>

      <!-- Theme toggle button -->
      <button type="button" class="btn h-full aspect-[1.2]" v-on:click="themeStore.toggle()">
        <Cog6ToothIcon v-if="themeStore.auto" class="size-4.5" />
        <SunIcon v-else-if="themeStore.theme === `light`" class="size-5" />
        <MoonIcon v-else class="size-4" />
      </button>

      <!-- Navigation bar toggle button -->
      <template v-if="navigationBarStore.shouldBeRendered">
        <button type="button" class="btn h-full aspect-[1.2] md:hidden shrink-0" v-on:click="navigationBarStore.toggle">
          <Bars4Icon class="size-6" />
        </button>
      </template>
    </div>
  </div>
</template>