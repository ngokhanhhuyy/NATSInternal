import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router";
import { useApi, ConnectionError, InternalServerError } from "@/api";
import { createSignInModel } from "@/models";
import { useAuthenticationStore } from "@/stores";
import { useTsxHelper, useRouteHelper } from "@/helpers";
// import { useUpsertViewStates } from "@/hooks/upsertViewStatesHook";

// Form components.
import { Form, FormField, TextInput } from "@/components/form";

// Components.
export default function SignInPage() {
  // Dependencies.
  const navigate = useNavigate();
  const params = useParams<{ returningPath?: string }>();
  const authenticationStore = useAuthenticationStore();
  const api = useApi();
  const { compute } = useTsxHelper();
  const { getHomeRoutePath } = useRouteHelper();

  // Model and state.
  const [model, setModel] = useState(createSignInModel);
  const [state, setState] = useState(() => ({
    commonError: null as string | null,
    isSignedIn: false,
    isInitiallyChecking: false,
    isSubmitting: false
  }));

  // Effect.
  useEffect(() => {
    const checkAuthenticationStatusAsync = async () => {
      if (!await authenticationStore.isAuthenticatedAsync()) {
        navigate(params.returningPath ?? getHomeRoutePath(), { replace: true });
      }

      setState((state) => ({ ...state, isInitiallyChecking: true }));
    };

    checkAuthenticationStatusAsync();
  }, []);

  // Computed.
  const areRequiredFieldsFilled = compute<boolean>(() => {
    return model.userName.length > 0 && model.password.length > 0;
  });

  // Callbacks.
  async function loginAsync(): Promise<void> {
    setState((state) => ({
      ...state,
      isSubmitting: true,
      commonError: null,
    }));

    try {
      await api.authentication.signInAsync(model.toRequestDto());
      authenticationStore.isAuthenticated = true;
      setState(state => ({ ...state, isSubmitting: false, isSignedIn: true }));
    } finally {
        setModel(model => ({ ...model, password: "" }));
        setState(state => ({
          ...state,
          isSubmitting: false,
          isSignedIn: false
        }));
    }
  }

  function handleLoginSucceeded(): void {
    const returningPath = params.returningPath ?? getHomeRoutePath();
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

    setState(state => ({ ...state, commonError }));
  }

  async function handleEnterKeyPressedAsync(): Promise<void> {
    if (areRequiredFieldsFilled) {
      await loginAsync();
    }
  }

  // Template.
  function renderSignInButton(): React.ReactNode {
    let buttonContent = <span>Đăng nhập</span>;
    if (state.isSubmitting) {
      buttonContent = <span className="spinner-border spinner-border-sm" aria-hidden="true" />;
    }

    return (
      <button type="submit" className="btn btn-primary w-100" disabled={!areRequiredFieldsFilled || state.isSignedIn}>
        {buttonContent}
      </button>
    );
  }

  return (
    <div
      className="container-fluid d-flex flex-column flex-fill justify-content-center"
      style={{ width: "100vw", maxWidth: "100%", minHeight: "100%" }}
      onKeyUp={(event) => event.key === "Enter" && handleEnterKeyPressedAsync()}
    >
      <div className="row py-3 g-3 justify-content-center">
        <div className="col col-xxl-4 col-xl-4 col-lg-5 col-md-6 col-sm-8 col-12 d-flex align-items-center">
          <Form
            className="block bg-white border border-primary-subtle rounded-3 shadow-sm w-100 p-3"
            submitAction={loginAsync}
            onSubmissionSucceeded={handleLoginSucceeded}
            onSubmissionFailed={handleLoginFailed}
            submissionSucceededText="Đăng nhập thành công!"
          >
            {/* Username */}
            <FormField className="mb-3" propertyPath="userName">
              <div className="form-floating">
                <TextInput
                  value={model.userName}
                  onValueChanged={(userName) => setModel(model => ({ ...model, userName }))}
                />
                <label className="form-label bg-transparent fw-normal">
                  Tên tài khoản
                </label>
              </div>
            </FormField>

            {/* Password */}
            <FormField className="mb-3" propertyPath="password">
              <div className="form-floating">
                <TextInput
                  password
                  value={model.password}
                  onValueChanged={(password) => setModel(model => ({ ...model, password }))}
                />
                <label className="form-label bg-transparent fw-normal">
                  Mật khẩu
                </label>
              </div>
            </FormField>

            <div className="form-group">
              {/* Button */}
              {renderSignInButton()}

              {/* CommonError */}
              {state.commonError && (
                <span className="alert alert-danger d-flex justify-content-center mt-3 w-100">
                  <i className="bi bi-exclamation-triangle-fill me-1" />
                  {state.commonError}
                </span>
              )}
            </div>
          </Form>
        </div>
      </div>
    </div>
  );
}