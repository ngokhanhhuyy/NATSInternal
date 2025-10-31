import { useCallback } from "react";
import { useNavigate } from "react-router";
import { useApi } from "@/api";
import { useAuthenticationStore } from "@/stores";
import { useRouteHelper } from "@/helpers";

// Component.
export default function HomePage() {
  // Dependencies.
  const navigate = useNavigate();
  const setIsAuthenticated = useAuthenticationStore(store => store.setIsAuthenticated);
  const api = useApi();
  const { getSignInRoutePath } = useRouteHelper();

  // Callbacks.
  const signOut = useCallback(async () => {
    await api.authentication.clearAccessCookieAsync();
    setIsAuthenticated(false);
    navigate(getSignInRoutePath());
  }, [setIsAuthenticated]);

  // Template.
  return <button onClick={signOut}>Đăng xuất</button>;
}