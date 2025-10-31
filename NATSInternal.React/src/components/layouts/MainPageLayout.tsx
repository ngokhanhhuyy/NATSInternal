import { useEffect } from "react";
import { Outlet } from "react-router";
import { useNavigate } from "react-router";
import { useAuthenticationStore } from "@/stores";
import { useRouteHelper } from "@/helpers";
import styles from "./MainPageLayout.module.css";

// Layout components.
import RootLayout from "./RootLayout";

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
    <RootLayout>
      <div className={styles.mainPageLayout}>
        <Outlet />
      </div>
    </RootLayout>
  );
}