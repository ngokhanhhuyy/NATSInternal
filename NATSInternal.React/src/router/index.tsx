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

// Components.
function AuthenticationErrorBoundary(): React.ReactNode | null {
  // Dependencies.
  const error = useRouteError();
  const { getSignInRoutePath } = useRouteHelper();

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
          title: "Đăng nhập"
        }
      },
      {
        path: "",
        Component: MainPageLayout,
        children: [
          {
            index: true,
            Component: TestingPage,
            handle: {
              title: "Trang chủ",
            }
          },
          {
            path: "khach-hang",
            children: [
              {
                index: true,
                Component: TestingPage,
                handle: {
                  title: "Danh sách khách hàng"
                }
              },
              {
                path: "tao-moi",
                Component: TestingPage,
                handle: {
                  title: "Tạo khách hàng mới"
                }
              },

              {
                path: ":id",
                children: [
                  {
                    index: true,
                    Component: TestingPage,
                    handle: {
                      title: "Chi tiết khách hàng"
                    }
                  },
                  {
                    path: "chinh-sua",
                    Component: TestingPage,
                    handle: {
                      title: "Chỉnh sửa khách hàng"
                    }
                  },
                ]
              },
            ],
          },
        ]
      },
    ]
  },
]);

// Component.
export default function Router() {
  return <RouterProvider router={router} />;
}