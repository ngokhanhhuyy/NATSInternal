import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router";
import { useApi, ConnectionError, InternalServerError, AuthenticationError } from "@/api";
import { createSignInModel } from "@/models";
import { useAuthenticationStore } from "@/stores";
import { useTsxHelper, useRouteHelper } from "@/helpers";
import styles from "./SignInPage.module.css";

// Form components.
import { Form, FormField, TextInput } from "@/components/form";

// Components.
export default function SignInPage() {
  // Dependencies.
  const navigate = useNavigate();
  const params = useParams<{ returningPath?: string }>();
  const authenticationStore = useAuthenticationStore();
  const api = useApi();
  const { compute, joinClassName } = useTsxHelper();
  const { getHomeRoutePath } = useRouteHelper();

  // Model and state.
  const [model, setModel] = useState(createSignInModel);
  const [state, setState] = useState(() => ({
    commonError: null as string | null,
    isSignedIn: false,
    isInitiallyChecking: false,
    isSubmitting: false,
    hasModelError: false,
  }));

  // Effect.
  useEffect(() => {
    const checkAuthenticationStatusAsync = async () => {
      try {
        await api.authentication.checkAuthenticationStatusAsync();
      } catch (error) {
        if (error instanceof AuthenticationError) {
          navigate(params.returningPath ?? getHomeRoutePath(), { replace: true });
        }
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
    console.log(123);
    setState((state) => ({
      ...state,
      isSubmitting: true,
      commonError: null,
    }));

    try {
      await api.authentication.getAccessCookieAsync(model.toRequestDto());
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
      setState(s => ({ ...s, hasModelError: true }));
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

    console.log(error);

    setState(state => ({ ...state, commonError }));
  }

  async function handleEnterKeyPressedAsync(): Promise<void> {
    if (areRequiredFieldsFilled) {
      await loginAsync();
    }
  }

  // Template.
  function renderSignInButton(): React.ReactNode {
    let buttonContent = <span>{state.hasModelError ? "Đăng nhập lại" : "Đăng nhập"}</span>;
    if (state.isSubmitting) {
      buttonContent = <span className="spinner-border spinner-border-sm" aria-hidden="true" />;
    }

    return (
      <button type="submit" className={state.hasModelError ? "red" : "indigo"}>
        {buttonContent}
      </button>
    );
  }

  return (
    <div className={styles.container} onKeyUp={(event) => event.key === "Enter" && handleEnterKeyPressedAsync()}>
      <Form
        className={joinClassName(styles.form, "flex flex-col rounded-lg p-8 items-stretch")}
        submitAction={loginAsync}
        onSubmissionSucceeded={handleLoginSucceeded}
        onSubmissionFailed={handleLoginFailed}
        submissionSucceededText="Đăng nhập thành công!"
      >
        {/* Introduction */}
        <div className="flex flex-col mb-8 text-indigo-800/75">
          <span className="text-4xl font-bold">Đăng nhập</span>
          <span className="text-lg">
            Chào mừng bạn đã quay trở lại.
          </span>
        </div>

        {/* Username */}
        <FormField className="mb-5" path="userName" displayName="Tên tài khoản">
          <TextInput
            placeholder="Tên tài khoản"
            value={model.userName}
            onValueChanged={(userName) => setModel(model => ({ ...model, userName }))}
          />
        </FormField>

        {/* Password */}
        <FormField className="mb-8" path="password" displayName="Mật khẩu">
          <TextInput
            password
            placeholder="Mật khẩu"
            value={model.password}
            onValueChanged={(password) => setModel(model => ({ ...model, password }))}
          />
        </FormField>

        {/* Button */}
        {renderSignInButton()}

        {/* CommonError */}
        {state.commonError && (
          <span className="alert alert-danger d-flex justify-content-center mt-3 w-100">
            <i className="bi bi-exclamation-triangle-fill me-1" />
            {state.commonError}
          </span>
        )}
      </Form>
    </div>
  );
}