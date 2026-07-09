import React from "react";
import { createBrowserRouter, RouterProvider, useRouteError, Navigate } from "react-router";
import { useAuthenticationStore } from "@/stores";
import { AuthenticationError } from "@/api";
import { getSignInRoutePath, getDashboardRoutePath } from "@/helpers";

// Layouts.
import { RootLayout, MainPageLayout } from "@/components/layouts";

// Routes.
import { authenticationRoutes } from "./authenticatinoRoutes";
import { homeRoutes } from "./homeRoutes";
import { customerRoutes } from "./customerRoutes";
import { productRoutes } from "./productRoutes";
import { supplyRoutes } from "./supplyRoutes";
import { orderRoutes } from "./orderRoutes";

// Components.
function AuthenticationErrorBoundary(): React.ReactNode | null {
  // Dependencies.
  const error = useRouteError();
  const setIsAutheticated = useAuthenticationStore(store => store.setIsAuthenticated);

  // Template.
  if (error instanceof AuthenticationError) {
    console.log(true);
    setIsAutheticated(false);
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
          productRoutes,
          supplyRoutes,
          orderRoutes
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
