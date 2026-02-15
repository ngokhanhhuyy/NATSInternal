import type { RouteRecordRaw } from "vue-router";

// Pages.
const CustomerListPage = () => import("@/pages/customer/customerList/CustomerListPage.vue");

// Routes.
export const customerRoutes: RouteRecordRaw = {
  path: "/khach-hang",
  name: "customer",
  meta: {
    breadcrumbItem: {
      text: "Khách hàng",
      to: { name: "customer" }
    }
  },
  children: [
    {
      path: "/",
      component: CustomerListPage,
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