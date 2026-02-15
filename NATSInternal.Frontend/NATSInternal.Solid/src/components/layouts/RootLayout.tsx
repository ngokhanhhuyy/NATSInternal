import { createEffect } from "solid-js";
import { useLocation, useCurrentMatches } from "@solidjs/router";
import { useRouteHelper, useTsxHelper } from "@/helpers";

// Child components.
import TopBar from "@/components/layouts/navigationBar/TopBar";
import NavigationBar from "@/components/layouts/navigationBar/navigationBar";

// Props.
type RootLayoutProps = Omit<JSX.HTMLD, "id">;

// Component.
export default function RootLayout(props: RootLayoutProps): React.ReactNode {
  // Dependencies.
  const location = useLocation();
  const matchedRoutes = useMatches();
  const { getSignInRoutePath } = useRouteHelper();
  const { joinClassName: joinClass, compute } = useTsxHelper();

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
      className={joinClass(
        "w-screen h-auto min-h-screen grid-cols-[auto_1fr] justify-stretch items-stretch",
        shouldRenderNavigationBar && "pt-(--topbar-height)"
      )}
    >
      {shouldRenderNavigationBar && <TopBar />}
      <main className={joinClass(
        "max-w-384 flex justify-stretch items-stretch mx-auto",
        "transition-opacity min-h-[calc(100vh-var(--topbar-height))]"
      )}>
        {shouldRenderNavigationBar && <NavigationBar />}
        {props.children}
      </main>
    </div>
  );
}