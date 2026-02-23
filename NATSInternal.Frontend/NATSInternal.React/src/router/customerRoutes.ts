import { lazy } from "react";
import type { RouteObject } from "react-router";

const CustomerListPage = lazy(() => import("@/pages/customer/customerList/CustomerListPage"));
const CustomerDetailPage = lazy(() => import("@/pages/customer/customerDetail/CustomerDetailPage"));
const CustomerCreatePage = lazy(() => import("@/pages/customer/customerUpsert/CustomerCreatePage"));
const CustomerUpdatePage = lazy(() => import("@/pages/customer/customerUpsert/CustomerUpdatePage"));

export const customerRoutes: RouteObject = {
  path: "khach-hang",
  children: [
    {
      index: true,
      Component: CustomerListPage,
      loader: () => import("@/pages/customer/customerList/CustomerListPage").then(m => m.loadDataAsync()),
      handle: {
        breadcrumbTitle: "Danh sách",
        pageTitle: "Danh sách khách hàng",
        description: "Danh sách các khách hàng đã đăng kí và đã từng giao dịch với cửa hàng."
      }
    },
    {
      path: "tao-moi",
      Component: CustomerCreatePage,
      handle: {
        breadcrumbTitle: "Tạo mới",
        pageTitle: "Tạo khách hàng mới",
        desciption: (
          "Tạo bản ghi dữ liệu cho một khách hàng mới, bao gồm thông tin cá nhân và người giới thiệu (nếu có)."
        )
      }
    },
    {
      path: ":id",
      children: [
        {
          index: true,
          Component: CustomerDetailPage,
          loader: async ({ params }) => {
            const module = await import("@/pages/customer/customerDetail/CustomerDetailPage");
            return module.loadDataAsync(params.id as string);
          },
          handle: {
            breadcrumbTitle: "Chi tiết",
            pageTitle: "Chi tiết khách hàng",
            description: (
              "Thông tin chi tiết về khách hàng, bao gồm thông tin cá nhân " +
              "và thông tin về nợ các giao dịch gần nhất"
            )
          }
        },
        {
          path: "chinh-sua",
          Component: CustomerUpdatePage,
          loader: async ({ params }) => {
            const module = await import("@/pages/customer/customerUpsert/CustomerUpdatePage");
            return module.loadDataAsync(params.id as string);
          },
          handle: {
            breadcrumbTitle: "Chỉnh sửa",
            pageTitle: "Chỉnh sửa khách hàng",
            description: (
              "Chỉnh sửa một bản ghi dữ liệu của một khách hàng đang tồn tại, " +
              "bao gồm thông tin cá nhân và người giới thiệu (nếu có)."
            )
          }
        },
      ]
    },
  ],
  handle: {
    breadcrumbTitle: "Khách hàng",
  }
};