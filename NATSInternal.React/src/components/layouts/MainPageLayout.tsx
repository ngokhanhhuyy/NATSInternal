import { useEffect } from "react";
import { Outlet } from "react-router";
import { useNavigate } from "react-router";
import { useAuthenticationStore } from "@/stores";
import { useRouteHelper } from "@/helpers";

// Props.
export default function MainPageLayout() {
  // Dependencies.
  const navigate = useNavigate();
  const isAuthenticated = useAuthenticationStore(store => store.isAuthenticated);
  const { getSignInRoutePath } = useRouteHelper();

  // Effect.
  useEffect(() => {
    if (!isAuthenticated) {
      navigate(getSignInRoutePath());
    }
  }, []);

  // Template.
  if (!isAuthenticated) {
    return null;
  }

  return (
    <div className="bg-white" style={{ width: "100vw", height: "100vh" }}>
      <Outlet />
    </div>
  );
}