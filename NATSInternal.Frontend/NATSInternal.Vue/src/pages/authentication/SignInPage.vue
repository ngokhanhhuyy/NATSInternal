<script setup lang="ts">
import { reactive } from "vue";
import { useRouter } from "vue-router";
import { useApi, ConnectionError, InternalServerError } from "@/api";
import { createSignInModel } from "@/models";
import { useAuthenticationStore } from "@/stores";

// Child components.
import { Form, FormField } from "@/components/form";
import { ExclamationCircleIcon } from "@heroicons/vue/24/solid";

// Dependencies.
const router = useRouter();
const api = useApi();
const authenticationStore = useAuthenticationStore();

// States.
const states = reactive({
  model: createSignInModel(),
  commonError: null as string | null,
  isSignedIn: false,
  isInitiallyChecking: false,
  isSubmitting: false,
});

// Callbacks.
const signInAsync = async (): Promise<void> => {
  states.isSubmitting = true;
  states.commonError = null;
  
  try {
    await api.authentication.getAccessCookieAsync(states.model.toRequestDto());
  } finally {
    states.model.password = "";
    states.isSubmitting = false;
  }
};

const handleSignInSucceeded = (): void => {
  states.isSignedIn = true;
  authenticationStore.isAuthenticated = true;
  setTimeout(() => router.push({ name: "home" }), 0);
};

const handleSignInFailed = (error: Error, errorHandled: boolean): void => {
  if (errorHandled) {
    return;
  }

  states.isSignedIn = false;
  if (error instanceof InternalServerError) {
    states.commonError = "Đã xảy ra lỗi từ máy chủ.";
  } else if (error instanceof ConnectionError) {
    states.commonError = "Không thể kết nối đến máy chủ.";
  } else {
    states.commonError = "Đã xảy ra lỗi không xác định.";
  }
};
</script>

<template>
  <div class="form-container">
    <Transition name="fade" mode="out-in">
      <Form
        v-if="!states.isSignedIn"
        v-bind:upsert-action="signInAsync"
        v-on:upserting-succeeded="handleSignInSucceeded"
        v-on:upserting-failed="handleSignInFailed"
        v-slot="{ errorCollection: { isValid, isValidated } }"
        class="form"
      >
        <!-- Introduction -->
        <div class="flex flex-col mb-5">
          <span class="text-4xl font-bold">Đăng nhập</span>
          <span class="text-lg">
              Chào mừng bạn đã quay trở lại.
            </span>
        </div>

        <!-- UserName -->
        <FormField class="mb-3" path="userName">
          <input
            v-model="states.model.userName"
            type="text"
            class="form-control"
            placeholder="Tên tài khoản"
            autocapitalize="off"
          />
        </FormField>
        
        <!-- Password -->
        <FormField class="mb-3" path="password">
          <input
            v-model="states.model.password"
            type="password"
            class="form-control"
            placeholder="Mật khẩu"
          />
        </FormField>

        <!-- Button -->
        <button v-bind:class="[`btn`, isValidated && !isValid && `btn-danger`]" type="submit">
          <span v-if="states.isSubmitting">Đang kiểm tra</span>
          <span v-else-if="!isValid">Đăng nhập lại</span>
          <span v-else>Đăng nhập</span>
        </button>

        <!-- Common error -->
        <span v-if="states.commonError" class="alert alert-red-outline flex justify-center mt-3 w-full">
          <ExclamationCircleIcon class="size-5" />
          {{ states.commonError }}
        </span>
      </Form>

      <div v-else class="successful-notification">
        Đăng nhập thành công
      </div>
    </Transition>

    <div class="copyright">
      © {{ new Date().getFullYear() }} - Bản quyền thuộc về Ngô Khánh Huy
    </div>
  </div>
</template>

<style scoped>
@import "@/assets/css/style.css" reference;

.form-container {
  @apply 
    bg-white dark:bg-neutral-900 sm:bg-transparent dark:sm:bg-transparent
    flex flex-col justify-start items-center w-screen h-screen pt-[25vh];
}

.form {
  @apply 
    bg-white dark:bg-neutral-900
    border border-transparent sm:border-black/15 dark:sm:border-white/15
    shadow-none sm:shadow-xs
    flex flex-col rounded-xl p-8 items-stretch w-87.5 relative;
}

.successful-notification {
  @apply
    bg-emerald-500/10 border border-emerald-500/50
    flex justify-center items-center w-87.5 relative rounded-xl p-8 shadow-none sm:shadow-xs
    text-emerald-700 dark:text-emerald-400 uppercase font-bold;
}

.copyright {
  @apply text-primary/50 absolute bottom-1 sm:relative sm:bottom-[unset] flex justify-end mb-3 sm:mt-10 sm:mb-0;
}
</style>