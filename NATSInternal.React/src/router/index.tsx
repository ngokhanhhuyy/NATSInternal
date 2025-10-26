import React from "react";
import { createBrowserRouter, RouterProvider, useRouteError, Navigate } from "react-router";
import { useRouteHelper } from "@/helpers";

// Layouts.
import MainPageLayout from "@/components/layouts/MainPageLayout";

// Pages.
import SignInPage from "@/pages/authentication/signIn/SignInPage";
import HomePage from "@/pages/home/HomePage";
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
        path: "/dang-nhap",
        Component: SignInPage,
      },
      {
        path: "/",
        Component: MainPageLayout,
        children: [
          {
            index: true,
            Component: HomePage
          }
        ]
      }
    ]
  },
]);

// Component.
export default function Router() {
  return <RouterProvider router={router} />;
}