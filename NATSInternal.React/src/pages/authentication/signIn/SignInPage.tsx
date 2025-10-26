import React, { useState } from "react";
import { useNavigate } from "react-router";
import { useApi, ConnectionError, InternalServerError } from "@/api";
import { createSignInModel } from "@/models";
import { useTsxHelper, useRouteHelper } from "@/helpers";
import styles from "./SignInPage.module.css";

// Child components.
import { Form, FormField, TextInput } from "@/components/form";
import { Button } from "@/components/Button";

// Components.
export default function SignInPage() {
  // Dependencies.
  const navigate = useNavigate();
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
      await api.authentication.getAccessCookieAsync(model.toRequestDto());
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
    setTimeout(() => {
      navigate(getHomeRoutePath());
      console.log("navigated");
    }, 1000);
  }

  function handleLoginFailed(error: Error, errorHandled: boolean): void {
    if (errorHandled) {ư
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

    setState(state => ({ ...state, commonError }));
  }

  async function handleEnterKeyPressedAsync(): Promise<void> {
    if (areRequiredFieldsFilled) {
      await loginAsync();
    }
  }

  // Template.
  function renderSignInButton(): React.ReactNode {
    let buttonContent: string;
    if (state.isSubmitting) {
      buttonContent = "Đang kiểm tra";
    } else if (state.hasModelError) {
      buttonContent = "Đăng nhập lại";
    } else {
      buttonContent = "Đăng nhập";
    }

    return (
      <Button type="submit" variant={state.hasModelError ? "red" : "indigo"} showSpinner={state.isSubmitting}>
        {buttonContent}
      </Button>
    );
  }

  return (
    <div className={styles.container} onKeyUp={(event) => event.key === "Enter" && handleEnterKeyPressedAsync()}>
      <Form
        className={joinClassName(styles.form, "flex flex-col rounded-xl p-8 items-stretch")}
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
        <FormField className="mb-5" path="userName">
          <TextInput
            autoCapitalize="off"
            value={model.userName}
            onValueChanged={(userName) => setModel(model => ({ ...model, userName: userName.toLowerCase() }))}
          />
        </FormField>

        {/* Password */}
        <FormField className="mb-8" path="password">
          <TextInput
            password
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

      <div className="text-indigo-900/50 flex justify-end mt-7">
        © {new Date().getFullYear()} - Bản quyền thuộc về Ngô Khánh Huy
      </div>
    </div>
  );
}