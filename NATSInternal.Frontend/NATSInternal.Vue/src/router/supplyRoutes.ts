import type { RouteRecordRaw } from "vue-router";

// Pages.
const SupplyListPage = () => import("@/pages/supply/supplyList/SupplyListPage.vue");

// Routes.
export const supplyRoutes: RouteRecordRaw = {
  path: "/nhap-hang",
  name: "supply",
  meta: {
    breadcrumbItem: {
      text: "Đơn nhập hàng",
      to: { name: "supply" }
    }
  },
  children: [
    {
      path: "/",
      component: SupplyListPage,
      meta: {
        pageTitle: "Danh sách đơn nhập hàng",
        breadcrumbItem: {
          text: "Danh sách",
          to: { name: "supply-list" }
        }
      },
    }
  ]
};