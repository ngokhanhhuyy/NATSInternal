import { lazy } from "react";
import type { RouteObject } from "react-router";

const ProductCategoryListPage = lazy(() => {
  return import("@/pages/product/productCategory/productCategoryList/ProductCategoryListPage");
});

const ProductCategoryDetailPage = lazy(() => {
  return import("@/pages/product/productCategory/productCategoryDetail/ProductCategoryDetailPage");
});

const ProductCategoryUpdatePage = lazy(() => {
  return import("@/pages/product/productCategory/productCategoryUpsert/ProductCategoryUpdatePage");
});

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
    {
      path: ":id",
      children: [
        {
          index: true,
          Component: ProductCategoryDetailPage,
          loader: async ({ params }) => {
            const module = await import("@/pages/product/productCategory/productCategoryDetail/dataLoader");
            return module.loadDataAsync(params.id as string);
          },
          handle: {
            breadcrumbTitle: "Chi tiết",
            pageTitle: "Chi tiết phân loại thương hiệu",
            description: (
              "Thông tin chi tiết của phân loại thương hiệu."
            )
          }
        },
        {
          path: "chinh-sua",
          Component: ProductCategoryUpdatePage,
          loader: async ({ params }) => {
            const module = await import("@/pages/product/productCategory/productCategoryUpsert/dataLoader");
            return module.loadUpdateDataAsync(params.id as string);
          },
          handle: {
            breadcrumbTitle: "Chỉnh sửa",
            pageTitle: "Chỉnh sửa phân loại sản phẩm",
            description: (
              "Chỉnh sửa một phân loại sản phẩm đang tồn tại, dùng cho các " +
              "sản phẩm đang được kinh doanh tại cửa hàng."
            )
          }
        },
      ]
    },
  ],
  handle: {
    breadcrumbTitle: "Phân loại"
  }
};