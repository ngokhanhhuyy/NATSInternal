import { lazy } from "react";
import type { RouteObject } from "react-router";

const BrandListPage = lazy(() => import("@/pages/product/brand/brandList/BrandListPage"));
const BrandDetailPage = lazy(() => import("@/pages/product/brand/brandDetail/BrandDetailPage"));

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
        }
      ]
    }
  ],
  handle: {
    breadcrumbTitle: "Thương hiệu"
  }
};