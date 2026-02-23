import { lazy } from "react";
import type { RouteObject } from "react-router";

const SignInPage = lazy(() => import("@/pages/authentication/signIn/SignInPage"));

export const authenticationRoutes: RouteObject = {
  path: "dang-nhap",
  Component: SignInPage,
  handle: {
    pageTitle: "Đăng nhập"
  }
};