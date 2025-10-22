import { createBrowserRouter, RouterProvider } from "react-router";
import { authenticationMiddleware } from "./middlewares";

// Pages.
import SignInPage from "@/pages/authentication/signIn/SignInPage";
import HomePage from "@/pages/home/HomePage";

// Router.
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

// Component.
export default function Router() {
  return <RouterProvider router={router} />;
}