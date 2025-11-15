import React from "react";
import { useLocation } from "react-router";
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
  const { getSignInRoutePath } = useRouteHelper();
  const { joinClassName, compute } = useTsxHelper();

  // Computed.
  const shouldRenderNavigationBar = compute<boolean>(() => !location.pathname.startsWith(getSignInRoutePath()));

  // Template.
  return (
    <div
      id="root-layout"
      className={joinClassName(
        "w-screen max-w-[96rem] h-auto min-h-screen flex justify-stretch items-stretch",
        "pt-(--topbar-height) md:pt-0 mx-auto"
      )}
    >
      {shouldRenderNavigationBar && (
        <>
          <TopBar />
          <NavigationBar />
        </>
      )}
      {props.children}
    </div>
  );
}