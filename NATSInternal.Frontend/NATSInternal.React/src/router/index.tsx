import React from "react";
import { createBrowserRouter, RouterProvider, useRouteError, Navigate } from "react-router";
import { AuthenticationError } from "@/api";
import { useRouteHelper } from "@/helpers";

// Layouts.
import { RootLayout, MainPageLayout } from "@/components/layouts";

// Routes.
import { authenticationRoutes } from "./authenticatinoRoutes";
import { homeRoutes } from "./homeRoutes";
import { customerRoutes } from "./customerRoutes";
import { productRoutes } from "./productRoutes";

// Pages.

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

  throw error;
}

// Router.
const router = createBrowserRouter([
  {
    path: "/",
    Component: RootLayout,
    errorElement: <AuthenticationErrorBoundary />,
    children: [
      authenticationRoutes,
      {
        path: "",
        Component: MainPageLayout,
        children: [
          {
            index: true,
            element: <Navigate to={getDashboardRoutePath()} replace />
          },
          homeRoutes,
          customerRoutes,
          productRoutes
        ],
        handle: {
          breadcrumbTitle: "Trang chủ"
        }
      },
    ]
  },
  {
    path: "*",
    element: <Navigate to={getDashboardRoutePath()} replace />
  }
]);

// Component.
export default function Router() {
  return <RouterProvider router={router} />;
}