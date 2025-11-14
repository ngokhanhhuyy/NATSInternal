import React, { useEffect } from "react";
import { Outlet } from "react-router";
import { useNavigate } from "react-router";
import { useAuthenticationStore, useNavigationBarStore } from "@/stores";
import { useRouteHelper, useTsxHelper } from "@/helpers";

// Child components.
import RootLayout from "./RootLayout";
import { Button } from "../ui";
import { Bars4Icon as NavigationBarToggleIcon } from "@heroicons/react/24/outline";

// Props.
export default function MainPageLayout(): React.ReactNode {
  // Dependencies.
  const navigate = useNavigate();
  const isAuthenticated = useAuthenticationStore(store => store.isAuthenticated);
  const navigationBarStore = useNavigationBarStore();
  const { getSignInRoutePath } = useRouteHelper();
  const { joinClassName } = useTsxHelper();

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
      <div className={joinClassName(
        "bg-white justify-self flex-1 border border-primary/10",
        "flex flex-col justify-start items-stretch sm:rounded-t-2xl shadow-xl sm:mt-3",
        navigationBarStore.isExpanded && "translate-x-[190px] sm:translate-x-0"
      )}>
        {/* The bar on top the current page, containing the navigation bar toggle button and breadcrumb */}
        <div className="border-b border-primary/10 p-3 gap-3 flex">
          {/* Navigation bar toggle button */}
          <Button onClick={() => navigationBarStore.toggle()}>
            <NavigationBarToggleIcon className="size-6" />
          </Button>

          {/* Separator */}
          <div className="border-l border-primary/10 self-stretch" />

          {/* Page title */}
          <div className="text-xl flex items-center ms-2 lg:ms-3 translate-y-[7.5%]">Trang chủ</div>
        </div>

        {/* Page */}
        <Outlet />
      </div>
    </RootLayout>
  );
}