import { createBrowserRouter, redirect, type MiddlewareFunction } from "react-router";
import { RouterProvider } from "react-router";
import { useRouteHelper } from "@/helpers";

// Pages.
import SignInPage from "@/pages/authentication/signIn/SignInPage";
import HomePage from "@/pages/home/HomePage";
import { useAuthenticationStore } from "@/stores";

const authenticationMiddleware: MiddlewareFunction = async ({ request }) => {
  const { isAuthenticatedAsync } = useAuthenticationStore();
  const { getSignInRoutePath } = useRouteHelper();
  if (!await isAuthenticatedAsync()) {
    const url = new URL(request.url);
    const pathWithSearchParams = url.pathname + url.search;
    throw redirect(getSignInRoutePath(pathWithSearchParams));
  }
};

const router = createBrowserRouter([
  {
    path: "/dang-nhap",
    Component: SignInPage
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
]);

export default function Router() {
  return <RouterProvider router={router} />;
}