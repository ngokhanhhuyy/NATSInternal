import { lazy } from "react";
import type { RouteObject } from "react-router";

const BrandListPage = lazy(() => import("@/pages/product/brand/brandList/BrandListPage"));

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
  ],
  handle: {
    breadcrumbTitle: "Thương hiệu"
  }
};