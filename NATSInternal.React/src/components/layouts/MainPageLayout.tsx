import React, { useState, useEffect } from "react";
import { Outlet } from "react-router";
import { useNavigate } from "react-router";
import { useAuthenticationStore } from "@/stores";
import { useRouteHelper, useTsxHelper } from "@/helpers";

// Child components.
import RootLayout from "./RootLayout";
import { ChevronUpIcon, ChevronRightIcon, HomeIcon as HomeOutlineIcon } from "@heroicons/react/24/outline";
import { HomeIcon as HomeSolidIcon } from "@heroicons/react/24/solid";

// Props.
export default function MainPageLayout(): React.ReactNode {
  // Dependencies.
  const navigate = useNavigate();
  const isAuthenticated = useAuthenticationStore(store => store.isAuthenticated);
  const { getSignInRoutePath } = useRouteHelper();
  const { joinClassName } = useTsxHelper();

  // States.
  const [isScrollToTopButtonVisible, setIsScrollToTopButtonVisible] = useState<boolean>(false);

  // Effect.
  useEffect(() => {
    if (!isAuthenticated) {
      navigate(getSignInRoutePath());
    }

    const handleScroll = () => {
      setIsScrollToTopButtonVisible(window.scrollY > window.innerHeight / 2);
    };

    window.addEventListener("scroll", handleScroll);

    return () => {
      window.removeEventListener("scroll", handleScroll);
    };
  }, []);

  // Template.
  if (!isAuthenticated) {
    return null;
  }

  return (
    <RootLayout>
      <div className={joinClassName(
        "bg-white justify-self flex-1 border border-primary/10",
        "flex flex-col justify-start items-stretch rounded-t-2xl shadow-xl mx-3 mt-3",
      )}>
        {/* The bar on top the current page, containing breadcrumb */}
        <div id="breadcrumb" className="border-b border-primary/10 p-3 gap-3 flex">
          {/* Page title */}
          <div className="text-xl flex items-center ms-2 lg:ms-3 translate-y-[7.5%]">Trang chủ</div>
        </div>

        {/* Page */}
        <Outlet />

        {/* To Top Button */}
        <button
          type="button"
          className={joinClassName(
            "bg-primary text-primary-foreground p-3 fixed bottom-3 right-6 hover:cursor-pointer rounded-[50%]",
            "transition-opacity",
            !isScrollToTopButtonVisible && "opacity-0 pointer-events-none"
          )}
          onClick={() => window.scrollTo({ top: 0, behavior: "smooth" })}
        >
          <ChevronUpIcon className="size-6" />
        </button>
      </div>
    </RootLayout>
  );
}