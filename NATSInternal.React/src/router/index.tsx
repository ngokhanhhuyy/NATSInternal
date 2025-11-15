import React from "react";
import { createBrowserRouter, RouterProvider, useRouteError, Navigate } from "react-router";
import { useRouteHelper } from "@/helpers";

// Layouts.
import MainPageLayout from "@/components/layouts/MainPageLayout";

// Pages.
import SignInPage from "@/pages/authentication/signIn/SignInPage";
// import HomePage from "@/pages/home/HomePage";
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
                Component: TestingPage,
                handle: {
                  breadcrumbTitle: "Danh sách",
                  pageTitle: "Danh sách khách hàng"
                }
              },
              {
                path: "tao-moi",
                Component: TestingPage,
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
                    Component: TestingPage,
                    handle: {
                      breadcrumbTitle: "Chi tiết",
                      pageTitle: "Chi tiết khách hàng"
                    }
                  },
                  {
                    path: "chinh-sua",
                    Component: TestingPage,
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