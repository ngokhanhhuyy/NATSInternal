import type { RouteRecordRaw } from "vue-router";

// Pages.
const OrderListPage = () => import("@/pages/order/orderList/OrderListPage.vue");

// Routes.
export const orderRoutes: RouteRecordRaw = {
  path: "/don-hang",
  name: "order",
  meta: {
    breadcrumbItem: {
      text: "Đơn hàng",
      to: { name: "order" }
    }
  },
  children: [
    {
      path: "/",
      component: OrderListPage,
      meta: {
        pageTitle: "Danh sách đơn hàng",
        breadcrumbItem: {
          text: "Danh sách",
          to: { name: "order-list" }
        }
      },
    }
  ]
};