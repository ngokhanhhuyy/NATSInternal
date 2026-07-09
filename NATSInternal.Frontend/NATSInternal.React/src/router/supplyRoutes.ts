import { lazy } from "react";
import type { RouteObject } from "react-router";

const SupplyListPage = lazy(() => import("@/pages/supply/supplyList/SupplyListPage"));
// const SupplyDetailPage = lazy(() => import("@/pages/supply/supplyDetail/SupplyDetailPage"));
// const SupplyCreatePage = lazy(() => import("@/pages/supply/supplyUpsert/SupplyCreatePage"));
// const SupplyUpdatePage = lazy(() => import("@/pages/supply/supplyUpsert/SupplyUpdatePage"));

export const supplyRoutes: RouteObject = {
  path: "nhap-hang",
  children: [
    {
      index: true,
      Component: SupplyListPage,
      loader: () => import("@/pages/supply/supplyList/dataLoader").then(m => m.loadDataAsync()),
      handle: {
        breadcrumbTitle: "Danh sách",
        pageTitle: "Danh sách đơn nhập hàng",
        description: "Danh sách các đơn nhập hàng đã giao dịch."
      }
    },
    // {
    //   path: "tao-moi",
    //   Component: SupplyCreatePage,
    //   handle: {
    //     breadcrumbTitle: "Tạo mới",
    //     pageTitle: "Tạo đơn hàng mới",
    //     description: "Tạo một đơn hàng mới, chứa các thông tin như khách hàng, số tiền giao dịch, sản phẩm/dịch vụ, ..."
    //   }
    // },
    // {
    //   path: ":id",
    //   children: [
    //     {
    //       index: true,
    //       Component: SupplyDetailPage,
    //       loader: async ({ params }) => {
    //         const module = await import("@/pages/supply/supplyDetail/dataLoader");
    //         return module.loadDataAsync(parseInt(params.id as string));
    //       },
    //       handle: {
    //         breadcrumbTitle: "Chi tiết",
    //         pageTitle: "Chi tiết đơn hàng",
    //         description: (
    //           "Thông tin chi tiết về đơn đặt hàng, bao gồm giá sản phẩm/dịch vụ, " +
    //           "hồ sơ khách hàng và các thông tin quản lý."
    //         )
    //       }
    //     },
    //     {
    //       path: "chinh-sua",
    //       Component: SupplyUpdatePage,
    //       loader: async ({ params }) => {
    //         const module = await import("@/pages/supply/supplyUpsert/SupplyUpdatePage");
    //         return module.loadDataAsync(parseInt(params.id as string));
    //       },
    //       handle: {
    //         breadcrumbTitle: "Chỉnh sửa",
    //         pageTitle: "Chỉnh sửa nhập hàng",
    //         description: (
    //           "Chỉnh sửa một đơn nhập hàng đã tồn tại, chứa các thông tin như phí vận chuyển, " +
    //           "sản phẩm và số lượng, ...."
    //         )
    //       }
    //     },
    //   ]
    // },
  ],
  handle: {
    breadcrumbTitle: "Nhập hàng",
  }
};
