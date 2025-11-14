import React from "react";
import { useLocation } from "react-router";
import { useNavigationBarStore } from "@/stores";
import { useRouteHelper, useTsxHelper } from "@/helpers";

// Child components.
import NavigationBar from "./navigationBar/NavigationBar";

// Props.
type RootLayoutProps = Omit<React.ComponentPropsWithoutRef<"div">, "id">;

// Component.
export default function RootLayout(props: RootLayoutProps): React.ReactNode {
  // Dependencies.
  const location = useLocation();
  const isNavigationBarExpanded = useNavigationBarStore();
  const { getSignInRoutePath } = useRouteHelper();
  const { joinClassName, compute } = useTsxHelper();

  // Computed.
  const shouldRenderNavigationBar = compute<boolean>(() => !location.pathname.startsWith(getSignInRoutePath()));

  // Template.
  return (
    <div
      id="root-layout"
      className={joinClassName(
        "bg-primary/3 w-screen h-auto min-h-screen flex justify-stretch items-stretch sm:px-3",
        isNavigationBarExpanded ? "gap-3" : "lg-gap-3"
      )}
    >
      {shouldRenderNavigationBar && <NavigationBar />}
      {props.children}
    </div>
  );
}