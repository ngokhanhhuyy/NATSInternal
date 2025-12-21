import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router";
import { useApi, ConnectionError, InternalServerError } from "@/api";
import { createSignInModel } from "@/models";
import { useAuthenticationStore } from "@/stores";
import { useTsxHelper, useRouteHelper } from "@/helpers";

// Child components.
import RootLayout from "@/components/layouts/RootLayout";
import { Form, FormField, TextInput } from "@/components/form";
import { Button } from "@/components/ui";

// Components.
export default function SignInPage(): React.ReactNode {
  // Dependencies.
  const navigate = useNavigate();
  const api = useApi();
  const authenticationStore = useAuthenticationStore();
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
    if (authenticationStore.isAuthenticated) {
      navigate(getHomeRoutePath());
    }
  }, []);

  // Computed.
  const areRequiredFieldsFilled = compute<boolean>(() => {
    return model.userName.length > 0 && model.password.length > 0;
  });

  const buttonText = compute<string>(() => {
    if (state.isSubmitting) {
      return "Đang kiểm tra";
    } else if (state.hasModelError) {
      return "Đăng nhập lại";
    } else {
      return "Đăng nhập";
    }
  });

  const buttonVariant = compute<ColorVariant>(() => state.hasModelError ? "danger" : "primary");

  // Callbacks.
  async function loginAsync(): Promise<void> {
    setState((state) => ({
      ...state,
      isSubmitting: true,
      commonError: null,
      hasModelError: false,
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
    authenticationStore.setIsAuthenticated(true);
    setTimeout(() => {
      navigate(getHomeRoutePath());
    }, 1000);
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

    setState(state => ({ ...state, commonError }));
  }

  async function handleEnterKeyPressedAsync(): Promise<void> {
    if (areRequiredFieldsFilled) {
      await loginAsync();
    }
  }

  // Template.
  return (
    <RootLayout>
      <div
        className={joinClassName(
          "bg-white dark:bg-neutral-900 sm:bg-black/2 dark:sm:bg-black",
          "flex flex-col justify-center items-center w-screen h-screen"
        )}
        onKeyUp={(event) => event.key === "Enter" && handleEnterKeyPressedAsync()}>
        <Form
          className={joinClassName(
            "bg-white dark:bg-neutral-900",
            "border border-transparent sm:border-black/10 dark:sm:border-white/10",
            "shadow-none sm:shadow-xs",
            "flex flex-col rounded-xl p-8 items-stretch w-[350px] relative"
          )}
          upsertAction={loginAsync}
          onUpsertingSucceeded={handleLoginSucceeded}
          onUpsertingFailed={handleLoginFailed}
        >
          {/* Introduction */}
          <div className="flex flex-col mb-5">
            <span className="text-4xl font-bold">Đăng nhập</span>
            <span className="text-lg">
              Chào mừng bạn đã quay trở lại.
            </span>
          </div>

          {/* Username */}
          <FormField className="mb-3" path="userName">
            <TextInput
              autoCapitalize="off"
              value={model.userName}
              onValueChanged={(userName) => setModel(model => ({ ...model, userName: userName.toLowerCase() }))}
            />
          </FormField>

          {/* Password */}
          <FormField className="mb-5" path="password">
            <TextInput
              type="password"
              value={model.password}
              onValueChanged={(password) => setModel(model => ({ ...model, password }))}
            />
          </FormField>

          {/* Button */}
          <Button type="submit" className={buttonVariant} showSpinner={state.isSubmitting}>
            {buttonText}
          </Button>

          {/* CommonError */}
          {state.commonError && (
            <span className="alert alert-danger d-flex justify-content-center mt-3 w-100">
              <i className="bi bi-exclamation-triangle-fill me-1" />
              {state.commonError}
            </span>
          )}
        </Form>

        <div className={joinClassName(
          "text-primary/50 absolute bottom-1 sm:relative sm:bottom-unset",
          "flex justify-end mb-3 sm:mt-7 sm:mb-0")}
        >
          © {new Date().getFullYear()} - Bản quyền thuộc về Ngô Khánh Huy
        </div>
      </div>
    </RootLayout>
  );
}