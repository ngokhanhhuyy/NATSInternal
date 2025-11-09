import React from "react";
import { useLocation } from "react-router";
import { useRouteHelper, useTsxHelper } from "@/helpers";

// Child components.
import NavigationBar from "./navigationBar/NavigationBar";

// Props.
type RootLayoutProps = Omit<React.ComponentPropsWithoutRef<"div">, "id">;

// Component.
export default function RootLayout(props: RootLayoutProps): React.ReactNode {
  // Dependencies.
  const location = useLocation();
  const { getSignInRoutePath } = useRouteHelper();
  const { compute } = useTsxHelper();

  // Computed.
  const shouldRenderNavigationBar = compute<boolean>(() => !location.pathname.startsWith(getSignInRoutePath()));

  // Template.
  return (
    <div
      id="root-layout"
      className="bg-primary/3 w-screen h-auto min-h-screen flex justify-stretch items-stretch pt-3 pe-3"
    >
      {shouldRenderNavigationBar && <NavigationBar />}
      {props.children}
    </div>
  );
}