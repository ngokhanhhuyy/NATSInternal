import { createBrowserRouter, RouterProvider, useRouteError, Navigate } from "react-router";
import { authenticationMiddleware, notAuthenticationMiddleware } from "./middlewares";
import { useRouteHelper } from "@/helpers";

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
        middleware: [notAuthenticationMiddleware]
      },
      {
        path: "/",
        middleware: [authenticationMiddleware],
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