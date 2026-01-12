import React, { useEffect } from "react";
import { useMatches } from "react-router";
import { useTsxHelper } from "@/helpers";

// Props.
type MainContainerProps = {
  description: string;
  isLoading?: boolean;
} & React.ComponentPropsWithoutRef<"div">;

// Component.
export default function MainContainer(props: MainContainerProps): React.ReactNode {
  // Props.
  const { description, isLoading, ...domProps } = props;

  // Dependencies.
  const matchedRoutes = useMatches();
  const { joinClassName, compute } = useTsxHelper();

  // Computed.
  const matchedRouteHandle = compute<{ pageTitle?: string, description?: string } | null>(() => {
    if (!matchedRoutes.length) {
      return null;
    } 

    const route =  matchedRoutes[matchedRoutes.length - 1];
    if ("handle" !in Object.keys(route) || typeof route["handle"] !== "object") {
      return null;
    }

    return route["handle"];
  });

  const pageTitle = compute<string | null>(() => {
    if (!matchedRouteHandle || "pageTitle" !in Object.keys(matchedRouteHandle)) {
      return null;
    }

    return matchedRouteHandle.pageTitle ?? null;
  });

  // Effect.
  useEffect(() => {
    window.scrollTo({ top: 0, behavior: "smooth" });
  }, []);

  // Template.
  return (
    <div
      {...domProps}
      id="main-container"
      className={joinClassName(
        props.className,
        isLoading && "opacity-50 cursor-wait"
      )}
    >
      <div className="flex flex-col">
        <span className="text-2xl">{pageTitle}</span>
        <span className="text-md opacity-50">{props.description}</span>
      </div>
      {props.children}
    </div>
  );
}