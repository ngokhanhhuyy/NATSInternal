import { createEffect, createMemos, useLocation, useCurrentMatches } from "@/solid";
import { useRouteHelper, useTsxHelper } from "@/helpers";

// Child components.
import TopBar from "@/components/layouts/navigationBar/TopBar";
import NavigationBar from "@/components/layouts/navigationBar/navigationBar";

// Props.
type RootLayoutProps = Omit<JSX.HTMLD, "id">;

// Component.
export default function RootLayout(props: RootLayoutProps): React.ReactNode {
  // Dependencies.
  const router = {
    location: useLocation(),
    get matchedRoutes() { return useCurrentMatches(); }
  };
  const { getSignInRoutePath } = useRouteHelper();
  const { joinClassName, compute } = useTsxHelper();

  // Computed.
  const shouldRenderNavigationBar = compute<boolean>();
  const memos = createMemos({
    "shouldRenderNavigationBar": () => !router.location.pathname.startsWith(getSignInRoutePath())
  });

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
      class={joinClassName(
        "w-screen h-auto min-h-screen grid-cols-[auto_1fr] justify-stretch items-stretch",
        shouldRenderNavigationBar && "pt-(--topbar-height)"
      )}
    >
      <solid:suspense>
        
      </solid:suspense>

      {shouldRenderNavigationBar && <TopBar />}
      <main class={joinClassName(
        "max-w-384 flex justify-stretch items-stretch mx-auto",
        "transition-opacity min-h-[calc(100vh-var(--topbar-height))]"
      )}>
        {shouldRenderNavigationBar && <NavigationBar />}
        {props.children}
      </main>
    </div>
  );
}