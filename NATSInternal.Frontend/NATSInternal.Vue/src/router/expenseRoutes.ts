import type { RouteRecordRaw } from "vue-router";

// Pages.
const ExpenseListPage = () => import("@/pages/expense/expenseList/ExpenseListPage.vue");

// Routes.
export const expenseRoutes: RouteRecordRaw = {
  path: "/chi-phi",
  name: "expense",
  meta: {
    breadcrumbItem: {
      text: "Chi phí",
      to: { name: "expense" }
    }
  },
  children: [
    {
      path: "/",
      component: ExpenseListPage,
      meta: {
        pageTitle: "Danh sách chi phí",
        breadcrumbItem: {
          text: "Danh sách",
          to: { name: "expense-list" }
        }
      },
    }
  ]
};