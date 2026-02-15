import type { RouteRecordRaw } from "vue-router";

// Pages.
const ProductListPage = () => import("@/pages/product/productList/ProductListPage.vue");

// Routes.
export const productRoutes: RouteRecordRaw = {
  path: "/san-pham",
  name: "product",
  meta: {
    breadcrumbItem: {
      text: "Sản phẩm",
      to: { name: "product" }
    }
  },
  children: [
    {
      path: "/",
      component: ProductListPage,
      meta: {
        pageTitle: "Danh sách sản phẩm",
        breadcrumbItem: {
          text: "Danh sách",
          to: { name: "product-list" }
        }
      },
    }
  ]
};