import React, { lazy } from "react";
import { createBrowserRouter, RouterProvider, useRouteError, Navigate } from "react-router";
import { useRouteHelper } from "@/helpers";

// Layouts.
import { MainPageLayout } from "@/components/layouts";

// Pages.
import SignInPage from "@/pages/authentication/signIn/SignInPage";
// import HomePage from "@/pages/home/HomePage";
const CustomerListPage = lazy(() => import("@/pages/customer/customerList/CustomerListPage"));
const CustomerDetailPage = lazy(() => import("@/pages/customer/customerDetail/CustomerDetailPage"));
const CustomerCreatePage = lazy(() => import("@/pages/customer/customerUpsert/CustomerCreatePage"));
const CustomerUpdatePage = lazy(() => import("@/pages/customer/customerUpsert/CustomerUpdatePage"));
const ProductListPage = lazy(() => import("@/pages/product/productList/ProductListPage"));
import TestingPage from "@/pages/TestingPage";
import { AuthenticationError } from "@/api";

// Route helper.
const { getSignInRoutePath, getDashboardRoutePath } = useRouteHelper();

// Components.
function AuthenticationErrorBoundary(): React.ReactNode | null {
  // Dependencies.
  const error = useRouteError();

  // Template.
  if (error instanceof AuthenticationError) {
    return <Navigate to={getSignInRoutePath()} />;
  }
}

// Router.
const router = createBrowserRouter([
  {
    path: "/",
    errorElement: <AuthenticationErrorBoundary />,
    children: [
      {
        path: "dang-nhap",
        Component: SignInPage,
        handle: {
          pageTitle: "Đăng nhập"
        }
      },
      {
        path: "",
        Component: MainPageLayout,
        children: [
          {
            index: true,
            element: <Navigate to={getDashboardRoutePath()} />
          },
          {
            path: "bang-dieu-khien",
            Component: TestingPage,
            handle: {
              breadcrumbTitle: "Bảng điều khiển",
              pageTitle: "Bảng điều khiển"
            }
          },
          {
            path: "khach-hang",
            children: [
              {
                index: true,
                Component: CustomerListPage,
                loader: () => import("@/pages/customer/customerList/CustomerListPage").then(m => m.loadDataAsync()),
                handle: {
                  breadcrumbTitle: "Danh sách",
                  pageTitle: "Danh sách khách hàng"
                }
              },
              {
                path: "tao-moi",
                Component: CustomerCreatePage,
                handle: {
                  breadcrumbTitle: "Tạo mới",
                  pageTitle: "Tạo khách hàng mới"
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
                      pageTitle: "Chi tiết khách hàng"
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
                      pageTitle: "Chỉnh sửa khách hàng"
                    }
                  },
                ]
              },
            ],
            handle: {
              breadcrumbTitle: "Khách hàng",
            }
          },
          {
            path: "san-pham",
            children: [
              {
                index: true,
                Component: ProductListPage,
                loader: () => import("@/pages/product/productList/ProductListPage").then(m => m.loadDataAsync()),
                handle: {
                  breadcrumbTitle: "Danh sách",
                  pageTitle: "Danh sách sản phẩm"
                }
              }
            ]
          }
        ],
        handle: {
          breadcrumbTitle: "Trang chủ"
        }
      },
    ]
  },
]);

// Component.
export default function Router() {
  return <RouterProvider router={router} />;
}