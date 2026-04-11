import { lazy } from "react";
import type { RouteObject } from "react-router";

const ProductCategoryListPage = lazy(() =>
  import("@/pages/product/productCategory/productCategoryList/ProductCategoryListPage"));

export const productCategoryRoutes: RouteObject = {
  path: "phan-loai",
  children: [
    {
      index: true,
      Component: ProductCategoryListPage,
      loader: () => import("@/pages/product/productCategory/productCategoryList/ProductCategoryListPage")
        .then(m => m.loadDataAsync()),
      handle: {
        breadcrumbTitle: "Danh sách",
        pageTitle: "Danh sách phân loại sản phẩm",
        description: "Danh sách các phân loại sản phẩm trong kho."
      }
    },
  ],
  handle: {
    breadcrumbTitle: "Phân loại"
  }
};