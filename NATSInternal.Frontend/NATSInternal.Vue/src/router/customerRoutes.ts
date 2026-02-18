import type { RouteRecordRaw } from "vue-router";

// Pages.
const CustomerListPage = () => import("@/pages/customer/customerList/CustomerListPage.vue");

// Routes.
export const customerRoutes: RouteRecordRaw = {
  path: "khach-hang",
  name: "customer",
  redirect: { name: "customer-list" },
  meta: {
    breadcrumbItem: {
      text: "Khách hàng",
      to: { name: "customer" }
    }
  },
  children: [
    {
      path: "",
      component: CustomerListPage,
      name: "customer-list",
      meta: {
        pageTitle: "Danh sách khách hàng",
        breadcrumbItem: {
          text: "Danh sách",
          to: { name: "customer-list" }
        }
      },
    }
  ]
};