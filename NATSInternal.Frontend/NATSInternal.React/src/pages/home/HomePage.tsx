import { useCallback } from "react";
import { useNavigate } from "react-router";
import { api } from "@/api";
import { useAuthenticationStore } from "@/stores";
import { getSignInRoutePath } from "@/helpers";

// Child components.
import MainContainer from "@/components/layouts/MainContainer";
import { Button } from "@/components/ui";

// Component.
export default function HomePage() {
  // Dependencies.
  const navigate = useNavigate();
  const setIsAuthenticated = useAuthenticationStore(store => store.setIsAuthenticated);

  // Callbacks.
  const signOut = useCallback(async () => {
    await api.authentication.clearAccessCookieAsync();
    setIsAuthenticated(false);
    navigate(getSignInRoutePath());
  }, [setIsAuthenticated]);

  // Template.
  return (
    <MainContainer>
      <Button onClick={signOut}>Đăng xuất</Button>
      {Array.from({ length: 10000 }, (_, index) => index).map((index) => index + "_abcxyz").join(" ")}
    </MainContainer>
  );
}
