import { lazy } from "react";
import type { RouteObject } from "react-router";

const OrderListPage = lazy(() => import("@/pages/order/orderList/OrderListPage"));

export const orderRoutes: RouteObject = {
  path: "don-hang",
  children: [
    {
      index: true,
      Component: OrderListPage,
      loader: () => import("@/pages/order/orderList/dataLoader").then(m => m.loadDataAsync()),
      handle: {
        breadcrumbTitle: "Danh sách",
        pageTitle: "Danh sách đơn hàng",
        description: "Danh sách các đơn hàng đã giao dịch."
      }
    },
  ],
  handle: {
    breadcrumbTitle: "Đơn hàng",
  }
};
