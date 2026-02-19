<script lang="ts">
import { page, navigating } from "$app/state";
import { goto } from "$app/navigation";
import { useApi, ConnectionError, InternalServerError } from "@/api";
import { createSignInModel } from "@/models";
import { useAuthenticationStore } from "@/stores";
import { useRouteHelper } from "@/helpers";

// Child components.
import { Form, FormField } from "@/components/form";

// Dependencies.
const api = useApi();
const authenticationStore = useAuthenticationStore();
const { getHomeRoutePath } = useRouteHelper();

// States.
const states= $state({
  model: createSignInModel(),
  commonError: null as string | null,
  isSignedIn: false,
  isInitialChecking: false,
  isSubmitting: false
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
  setTimeout(() => goto(getHomeRoutePath(), { replaceState: true }));
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

<div class="form-container">
  <Form
    class="form"
    upsertAction={signInAsync}
    onUpsertingSucceeded={handleSignInSucceeded}
    onUpsertingFailed={handleSignInFailed}
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
        bind:value={states.model.userName}
        type="text"
        class="form-control"
        placeholder="Tên tài khoản"
        autocapitalize="off"
      />
    </FormField>
  </Form>
</div>

<style lang="postcss">
@reference "@/assets/css/style.css";

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