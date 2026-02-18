<script setup lang="ts">
import { useAuthenticationStore } from "@/stores";

// Child components.
import MainContainer from "@/components/layouts/MainContainer.vue";

// Dependencies.
const authenticationStore = useAuthenticationStore();

// States.
const paragraphs = Array.from({ length: 100 }, (_, index) => index).map((index) => index + "_abcxyz").join(" ");

// Callbacks.
const handleSignOutButtonClicked = () => {
  authenticationStore.isAuthenticated = false;
};
</script>

<template>
  <MainContainer description="Tổng quan các thông tin vận hành của cửa hàng.">
    <div class="panel">
      <div class="panel-header">
        <span class="panel-header-title">
          Testing
        </span>

        <button type="button" class="btn btn-panel-header btn-sm" v-on:click="handleSignOutButtonClicked">
          Đăng xuất
        </button>
      </div>

      <div class="panel-body flex flex-col gap-3 p-3">
        <div class="flex gap-3">
          <button type="button" class="btn btn-primary">Primary</button>
          <button type="button" class="btn btn-primary-outline">Primary Outline</button>
        </div>
        
        <div class="flex gap-3">
          <button type="button" class="btn btn-danger">Danger</button>
          <button type="button" class="btn btn-danger-outline">Danger Outline</button>
        </div>
        
        <div class="flex gap-3">
          <button type="button" class="btn">Normal</button>
        </div>
      </div>
    </div>

    <div class="panel">
      <div class="panel-header">
        <span class="panel-header-title">
          Paragraph
        </span>
      </div>

      <div class="panel-body flex flex-col gap-3 p-3">
        {{ paragraphs }}
      </div>
    </div>
  </MainContainer>
</template>