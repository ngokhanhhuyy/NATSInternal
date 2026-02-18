import { createSignal, createEffect, onMount, batch, Show } from "solid-js";
import { useParams, useNavigate } from "@solidjs/router";
import { useApi, ValidationError, ConnectionError, InternalServerError, OperationError } from "@/api";
import { createSignInModel } from "@/models";
import { useAuthenticationStore } from "@/stores";
import { useRouteHelper } from "@/helpers";
// import { useUpsertViewStates } from "@/hooks/upsertViewStatesHook";

// Form components.
import { Form, FormField, TextInput } from "@/components/form";

export default function SignInPage() {
  // Dependencies.
  const navigate = useNavigate();
  const params = useParams<{ returningPath?: string }>();
  const authenticationStore = useAuthenticationStore();
  const api = useApi();
  const routeHelper = useRouteHelper();

  // Model and state.
  const [model, setModel] = createSignal(createSignInModel());
  const [states, getStates] = createSignal({
    commonError: null as string | null,
    isSignedIn: false,
    isInitiallyChecking: false,
    isSubmitting: false
  });

  // Effect.
  onMount(() => {
    checkAuthenticationStatusAsync();
  });

  // Computed.
  function computeAreRequiredFieldsFilled(): boolean {
    return true;
    // return getModel().userName.length > 0 && getModel().password.length > 0;
  }

  // Callbacks.
  async function checkAuthenticationStatusAsync(): Promise<void> {
    if (!await authenticationStore.isAuthenticatedAsync()) {
      navigate(params.returningPath ?? routeHelper.getHomeRoutePath(), { replace: true });
    }

    getStates((state) => ({ ...state, isInitiallyChecking: true }));
  }

  async function loginAsync(): Promise<void> {
    getStates((state) => ({
      ...state,
      isSubmitting: true,
      commonError: null,
    }));

    try {
      await api.authentication.getAccessCookieAsync(model().toRequestDto());
      authenticationStore.isAuthenticated = true;
      getStates(state => ({ ...state, isSubmitting: false, isSignedIn: true }));
    } finally {
      batch(() => {
        setModel(model => ({ ...model, password: "" }));
        getStates(state => ({
          ...state,
          isSubmitting: false,
          isSignedIn: false
        }));
      });
    }
  }

  function handleLoginSucceeded(): void {
    const returningPath = params.returningPath ?? routeHelper.getHomeRoutePath();
    setTimeout(() => navigate(returningPath), 1000);
  }

  function handleLoginFailed(error: Error, errorHandled: boolean): void {
    if (errorHandled) {
      return;
    }

    let commonError: string;
    if (error instanceof InternalServerError) {
      commonError = "Đã xảy ra lỗi từ máy chủ.";
    } else if (error instanceof ConnectionError) {
      commonError = "Không thể kết nối đến máy chủ.";
    } else {
      commonError = "Đã xảy ra lỗi không xác định.";
    }

    getStates(state => ({ ...state, commonError }));
  }

  async function handleEnterKeyPressedAsync(): Promise<void> {
    if (computeAreRequiredFieldsFilled()) {
      await loginAsync();
    }
  }

  // Template.
  function renderSignInButton(): JSX.Element {
    return (
      <Show when={!states().isSignedIn}>
        <button type="submit" class="btn btn-primary w-100" disabled={!computeAreRequiredFieldsFilled()}>
          <Show when={states().isSubmitting} fallback={<span>Đăng nhập</span>}>
            <span class="spinner-border spinner-border-sm" aria-hidden="true" />
          </Show>
        </button>
      </Show>
    );
  }

  return (
    <div
      class="container-fluid d-flex flex-column flex-fill justify-content-center"
      style={{ width: "100vw", "max-width": "100%", "min-height": "100%" }}
      onKeyUp={(event) => event.key === "Enter" && handleEnterKeyPressedAsync()}
    >
      <div class="row py-3 g-3 justify-content-center">
        <div class="col col-xxl-4 col-xl-4 col-lg-5 col-md-6 col-sm-8 col-12 d-flex align-items-center">
          <Form
            class="block bg-white border border-primary-subtle rounded-3 shadow-sm w-100 p-3"
            submitAction={loginAsync}
            onSubmissionSucceeded={handleLoginSucceeded}
            onSubmissionFailed={handleLoginFailed}
            submissionSucceededText="Đăng nhập thành công!"
          >
            {/* Username */}
            <FormField class="mb-3" propertyPath="userName">
              <div class="form-floating">
                <TextInput
                  value={model().userName}
                  onValueChanged={(userName) => setModel(model => ({ ...model, userName }))}
                />
                <label class="form-label bg-transparent fw-normal">
                  Tên tài khoản
                </label>
              </div>
            </FormField>

            {/* Password */}
            <FormField class="mb-3" propertyPath="password">
              <div class="form-floating">
                <TextInput
                  password
                  value={model().password}
                  onValueChanged={(password) => setModel(model => ({ ...model, password }))}
                />
                <label class="form-label bg-transparent fw-normal">
                  Mật khẩu
                </label>
              </div>
            </FormField>

            <div class="form-group">
              {/* Button */}
              {renderSignInButton()}

              {/* CommonError */}
              <Show when={states().commonError}>
                <span class="alert alert-danger d-flex justify-content-center mt-3 w-100">
                  <i class="bi bi-exclamation-triangle-fill me-1" />
                  {states().commonError}
                </span>
              </Show>
            </div>
          </Form>
        </div>
      </div>
    </div>
  );
}