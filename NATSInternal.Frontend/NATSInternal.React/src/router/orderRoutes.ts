import { lazy } from "react";
import type { RouteObject } from "react-router";

const OrderListPage = lazy(() => import("@/pages/order/orderList/OrderListPage"));
const OrderDetailPage = lazy(() => import("@/pages/order/orderDetail/OrderDetailPage"));

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
    {
      path: ":id",
      children: [
        {
          index: true,
          Component: OrderDetailPage,
          loader: async ({ params }) => {
            const module = await import("@/pages/order/orderDetail/dataLoader");
            return module.loadDataAsync(parseInt(params.id as string));
          },
          handle: {
            breadcrumbTitle: "Chi tiết",
            pageTitle: "Chi tiết đơn hàng",
            description: (
              "Thông tin chi tiết về đơn đặt hàng, bao gồm giá sản phẩm/dịch vụ, " +
              "hồ sơ khách hàng và các thông tin quản lý."
            )
          }
        },
      ]
    },
  ],
  handle: {
    breadcrumbTitle: "Đơn hàng",
  }
};
