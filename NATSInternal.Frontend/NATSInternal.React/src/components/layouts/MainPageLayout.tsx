import React, { useMemo, useEffect, Fragment } from "react";
import { useMatches, Outlet, Link, useNavigation } from "react-router";
import { useNavigate } from "react-router";
import { useAuthenticationStore } from "@/stores";
import { useRouteHelper, useTsxHelper } from "@/helpers";

// Child components.
import RootLayout from "./RootLayout";
import { HomeIcon, ChevronRightIcon } from "@heroicons/react/24/solid";

// Props.
type BreadcrumbItem = { pageTitle: string | null; breadcrumbTitle: string; routePath: string | null };

// Components.
export default function MainPageLayout(): React.ReactNode {
  // Dependencies.
  const navigation = useNavigation();
  const navigate = useNavigate();
  const isAuthenticated = useAuthenticationStore(store => store.isAuthenticated);
  const { getSignInRoutePath } = useRouteHelper();
  const { joinClassName, compute } = useTsxHelper();

  // Computed.
  const isNavigating = compute<boolean>(() => Boolean(navigation.location));

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
      <div id="main-page-layout" className={joinClassName(
        "bg-white dark:bg-white/10 justify-self flex-1 border border-b-0 border-primary/10",
        "flex flex-col justify-start items-stretch rounded-t-2xl mx-3 mt-3 overflow-x-hidden",
        "shadow-lg transition-opacity",
        isNavigating && "opacity-50 dark:opacity-70 pointer-events-none"
      )}>
        {/* The bar on top the current page, containing breadcrumb */}
        <div id="breadcrumb" className="border-b border-primary/10 p-3 gap-3 flex">
          <Breadcrumb />
        </div>

        {/* Page */}
        <Outlet />
      </div>
    </RootLayout>
  );
}

function Breadcrumb(): React.ReactNode {
  // Dependencies.
  const matchedRoutes = useMatches();
  const { joinClassName } = useTsxHelper();

  // Computed.
  const breadcrumItems = useMemo<BreadcrumbItem[]>(() => {
    const items: { pageTitle: string | null; breadcrumbTitle: string; routePath: string | null }[] = [];
    for (const matchRoute of matchedRoutes) {
      const handle = matchRoute.handle;
      if (typeof handle === "object" && handle != null && "breadcrumbTitle" in handle) {
        const breadcrumbTitle = handle["breadcrumbTitle" as keyof typeof handle] as string;
        const pageTitle = (handle["pageTitle" as keyof typeof handle] as string) ?? null;
        items.push({ pageTitle, breadcrumbTitle, routePath: matchRoute.pathname });
      }
    }

    return items;
  }, [matchedRoutes]);

  // Template.
  return (
    <div className="flex flex-wrap gap-3 items-center ms-2 lg:ms-3 translate-y-[7.5%] h-8">
      {breadcrumItems.map((item, index) => (
        <Fragment key={index}>
          <BreadcrumbItem {...item} isFirst={index === 0} isLast={index === breadcrumItems.length - 1} />
          {index < breadcrumItems.length - 1 && (
            <ChevronRightIcon
              className={joinClassName("size-4", index !== 0 && "hidden sm:inline")}
              key={index}
            />
          )}
        </Fragment>
      ))}
    </div>
  );
}

function BreadcrumbItem(props: BreadcrumbItem & { isFirst: boolean, isLast: boolean; }): React.ReactNode {
  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // Template.
  function renderChild(): React.ReactNode {
    if (props.isFirst) {
      return <HomeIcon className="size-5" />;
    }
    
    if (props.isLast) {
      return (
        <>
          <span className="hidden sm:inline">{props.breadcrumbTitle}</span>
          <span className="text-lg inline sm:hidden">{props.pageTitle}</span>
        </>
      );
    }

    return <span className="hidden sm:inline">{props.breadcrumbTitle}</span>;
  };

  return (
    <Link
      to={props.routePath ?? ""}
      className={joinClassName(
        (!props.isFirst && !props.isLast) ? "hidden sm:inline" : "text-lg sm:text-base",
        (props.routePath == null || props.isLast) && "pointer-events-none"
      )}
    >
      {renderChild()}
    </Link>
  );
}