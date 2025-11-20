import React, { useEffect } from "react";
import { useLocation, useMatches } from "react-router";
import { useRouteHelper, useTsxHelper } from "@/helpers";

// Child components.
import TopBar from "./navigationBar/TopBar";
import NavigationBar from "./navigationBar/NavigationBar";

// Props.
type RootLayoutProps = Omit<React.ComponentPropsWithoutRef<"div">, "id">;

// Component.
export default function RootLayout(props: RootLayoutProps): React.ReactNode {
  // Dependencies.
  const location = useLocation();
  const matchedRoutes = useMatches();
  const { getSignInRoutePath } = useRouteHelper();
  const { joinClassName, compute } = useTsxHelper();

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
    <div
      id="root-layout"
      className={joinClassName(
        "w-screen h-auto min-h-screen grid-cols-[auto_1fr] justify-stretch items-stretch",
        shouldRenderNavigationBar && "pt-(--topbar-height)"
      )}
    >
      {shouldRenderNavigationBar && <TopBar />}
      <div className="max-w-384 flex justify-stretch items-stretch mx-auto transition-opacity">
        {shouldRenderNavigationBar && <NavigationBar />}
        {props.children}
      </div>
    </div>
  );
}