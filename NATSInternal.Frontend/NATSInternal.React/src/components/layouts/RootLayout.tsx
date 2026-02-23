import React, { useEffect } from "react";
import { useLocation, useMatches, Outlet } from "react-router";
import { useRouteHelper, useTsxHelper } from "@/helpers";

// Child components.
import TopBar from "./navigationBar/TopBar";
import NavigationBar from "./navigationBar/NavigationBar";
// import ProgressBar from "./progressBar/ProgressBar";

// Component.
export default function RootLayout(): React.ReactNode {
  // Dependencies.
  const location = useLocation();
  const matchedRoutes = useMatches();
  const { getSignInRoutePath } = useRouteHelper();
  const { compute } = useTsxHelper();

  // Computed.
  const shouldRenderNavigationBar = compute<boolean>(() => !location.pathname.startsWith(getSignInRoutePath()));

  // Effect.
  useEffect(() => {
    for (const matchRoute of matchedRoutes.reverse()) {
      const handle = matchRoute.handle;
      if (typeof handle === "object" && handle != null && "pageTitle" in handle) {
        const pageTitle = handle["pageTitle" as keyof typeof handle] as string;
        document.title = `${pageTitle} - NATSInternal`;
        break;
      }
    }
  }, [matchedRoutes]);

  // Template.
  return (
    <div id="root-layout">
      <TopBar shouldRenderNavigationBarToggleButton={shouldRenderNavigationBar} />
      <main>
        <Outlet />
        {shouldRenderNavigationBar && <NavigationBar />}
      </main>
    </div>
  );
}