import { redirect, type MiddlewareFunction } from "react-router";
import { useApi } from "@/api";
import { useRouteHelper } from "@/helpers";

let isAuthenticated: boolean | null = null;

export const authenticationMiddleware: MiddlewareFunction = async ({ request }) => {
  const api = useApi();
  const { getSignInRoutePath } = useRouteHelper();

  if (isAuthenticated == null) {
    try {
      api.authentication.checkAuthenticationStatusAsync();
      isAuthenticated = true;
    } catch {
      isAuthenticated = false;
    }
  }

  if (!isAuthenticated) {
    const url = new URL(request.url);
    const pathWithSearchParams = url.pathname + url.search;
    throw redirect(getSignInRoutePath(pathWithSearchParams));
  }
};