import { lazy } from "react";
import type { RouteObject } from "react-router";
import { brandRoutes } from "./brandRoutes";
import { productCategoryRoutes } from "./productCategoryRoutes";

const ProductListPage = lazy(() => import("@/pages/product/productList/ProductListPage"));
const ProductDetailPage = lazy(() => import("@/pages/product/productDetail/ProductDetailPage"));
const ProductCreatePage = lazy(() => import("@/pages/product/productUpsert/ProductCreatePage"));
const ProductUpdatePage = lazy(() => import("@/pages/product/productUpsert/ProductUpdatePage"));

export const productRoutes: RouteObject = {
  path: "san-pham",
  children: [
    {
      index: true,
      Component: ProductListPage,
      loader: () => import("@/pages/product/productList/dataLoader").then(m => m.loadDataAsync()),
      handle: {
        breadcrumbTitle: "Danh sách",
        pageTitle: "Danh sách sản phẩm",
        description: "Danh sách các sản phẩm trong kho, kể cả các sản phẩm đã ngừng kinh doanh."
      }
    },
    {
      path: "tao-moi",
      Component: ProductCreatePage,
      loader: async () => {
        const module = await import("@/pages/product/productUpsert/ProductCreatePage");
        return module.loadDataAsync();
      },
      handle: {
        breadcrumbTitle: "Tạo mới",
        pageTitle: "Tạo sản phẩm mới",
        description: "Tạo một sản phẩm mới, dùng cho các giao dịch về bán lẻ và liệu trình."
      }
    },
    {
      path: ":id",
      children: [
        {
          index: true,
          Component: ProductDetailPage,
          loader: ({ params }) => import("@/pages/product/productDetail/ProductDetailPage")
            .then((module) => module.loadDataAsync(params.id as string)),
          handle: {
            breadcrumbTitle: "Chi tiết",
            pageTitle: "Chi tiết sản phẩm",
            description: "Thông tin chi tiết của sản phẩm, tình trạng lưu kho và các giao dịch liên quan gần nhất"
          }
        },
        {
          path: "chinh-sua",
          Component: ProductUpdatePage,
          loader: async ({ params }) => {
            const module = await import("@/pages/product/productUpsert/ProductUpdatePage");
            return module.loadDataAsync(params.id as string);
          },
          handle: {
            breadcrumbTitle: "Chỉnh sửa",
            pageTitle: "Chỉnh sửa sản phẩm",
            description: "Chỉnh sửa một sản phẩm đang tồn tại, dùng cho các giao dịch về bán lẻ và liệu trình."
          }
        },
      ]
    },
    brandRoutes,
    productCategoryRoutes
  ],
  handle: {
    breadcrumbTitle: "Sản phẩm",
  }
};