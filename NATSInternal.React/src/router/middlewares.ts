import { redirect, type MiddlewareFunction } from "react-router";
import { useApi, AuthenticationError } from "@/api";
import { useRouteHelper } from "@/helpers";

const api = useApi();
const { getSignInRoutePath, getHomeRoutePath } = useRouteHelper();

let isAuthenticated: boolean | null = null;

export const authenticationMiddleware: MiddlewareFunction = async (_, next) => {
  if (isAuthenticated == null) {
    try {
      await api.authentication.checkAuthenticationStatusAsync();
      isAuthenticated = true;
    } catch (error) {
      if (error instanceof AuthenticationError) {
        isAuthenticated = false;
      } else {
        throw error;
      }
    }
  }

  if (!isAuthenticated) {
    throw redirect(getSignInRoutePath());
  }
  

  await next();
};

export const notAuthenticationMiddleware: MiddlewareFunction = async (_, next) => {
  if (isAuthenticated == null) {
    try {
      await api.authentication.checkAuthenticationStatusAsync();
      isAuthenticated = true;
    } catch (error) {
      if (error instanceof AuthenticationError) {
        isAuthenticated = false;
      } else {
        throw error;
      }
    }
  }

  if (isAuthenticated) {
    throw redirect(getHomeRoutePath());
  }

  await next();
};