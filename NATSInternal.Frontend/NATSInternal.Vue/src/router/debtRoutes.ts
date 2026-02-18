import type { RouteRecordRaw } from "vue-router";

// Pages.
const DebtOverviewPage = () => import("@/pages/debt/debtOverview/DebtOverviewPage.vue");

// Routes.
export const debtRoutes: RouteRecordRaw = {
  path: "khoan-no",
  name: "debt",
  redirect: { name: "debt-overview" },
  meta: {
    breadcrumbItem: {
      text: "Nợ",
      to: { name: "debt" }
    }
  },
  children: [
    {
      path: "",
      component: DebtOverviewPage,
      name: "debt-overview",
      meta: {
        pageTitle: "Tổng quan nợ",
        breadcrumbItem: {
          text: "Tổng quan",
          to: { name: "debt-overview" }
        }
      },
    }
  ]
};