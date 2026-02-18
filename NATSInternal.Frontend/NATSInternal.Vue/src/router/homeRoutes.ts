import type { RouteRecordRaw } from "vue-router";

// Pages.
const HomePage = () => import("@/pages/home/HomePage.vue");

// Routes.
export const homeRoutes: RouteRecordRaw = {
  path: "",
  component: HomePage,
  name: "home",
  meta: {
    pageTitle: "Trang chủ",
    breadcrumbItem: {
      text: "Trang chủ",
      to: { name: "home" }
    }
  }
};