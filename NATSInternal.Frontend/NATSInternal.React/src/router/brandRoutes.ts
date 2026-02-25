import { lazy } from "react";
import type { RouteObject } from "react-router";

const BrandListPage = lazy(() => import("@/pages/product/brand/brandList/BrandListPage"));
const BrandDetailPage = lazy(() => import("@/pages/product/brand/brandDetail/BrandDetailPage"));
const BrandCreatePage = lazy(() => import("@/pages/product/brand/brandUpsert/BrandCreatePage"));
const BrandUpdatePage = lazy(() => import("@/pages/product/brand/brandUpsert/BrandUpdatePage"));

export const brandRoutes: RouteObject = {
  path: "thuong-hieu",
  children: [
    {
      index: true,
      Component: BrandListPage,
      loader: () => import("@/pages/product/brand/brandList/BrandListPage").then(m => m.loadDataAsync()),
      handle: {
        breadcrumbTitle: "Danh sách",
        pageTitle: "Danh sách thương hiệu",
        description: "Danh sách các thương hiệu của tất cả sản phẩm trong kho."
      }
    },
    {
      path: "tao-moi",
      Component: BrandCreatePage,
      handle: {
        breadcrumbTitle: "Tạo mới",
        pageTitle: "Tạo thương hiệu mới",
        description: "Tạo một thương hiệu mới, dùng cho các sản phẩm đang được kinh doanh bởi cửa hàng."
      }
    },
    {
      path: ":id",
      children: [
        {
          index: true,
          Component: BrandDetailPage,
          loader: async ({ params }) => {
            const module = await import("@/pages/product/brand/brandDetail/dataLoader");
            return module.loadDataAsync(params.id as string);
          },
          handle: {
            breadcrumbTitle: "Chi tiết",
            pageTitle: "Chi tiết thương hiệu",
            description: (
              "Thông tin chi tiết kèm thông tin liên lạc và " +
              "các sản phẩm hiện đang kinh doanh của thương hiệu."
            )
          }
        },
        {
          path: "chinh-sua",
          Component: BrandUpdatePage,
          loader: async ({ params }) => {
            const module = await import("@/pages/product/brand/brandUpsert/dataLoader");
            return module.loadUpdateDataAsync(params.id as string);
          },
          handle: {
            breadcrumbTitle: "Chỉnh sửa",
            pageTitle: "Chỉnh sửa thương hiệu",
            description: (
              "Chỉnh sửa một thương hiệu đang tồn tại, dùng cho các " +
              "sản phẩm đang được kinh doanh tại cửa hàng."
            )
          }
        },
      ]
    },
  ],
  handle: {
    breadcrumbTitle: "Thương hiệu"
  }
};