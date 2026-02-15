import type { RouteRecordRaw } from "vue-router";

// Pages.
const SignInPage = () => import("@/pages/authentication/SignInPage.vue");

// Routes.
export const authenticationRoutes: RouteRecordRaw = {
  path: "dang-nhap",
  component: SignInPage,
  name: "sign-in",
  meta: {
    pageTitle: "Đăng nhập",
    breadcrumbItem: {
      text: "Đăng nhập",
      to: { name: "sign-in" }
    }
  }
};