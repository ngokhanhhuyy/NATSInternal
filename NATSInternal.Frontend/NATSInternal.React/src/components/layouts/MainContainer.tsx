import React from "react";
import { useMatches } from "react-router";
import { useTsxHelper } from "@/helpers";

// Props.
type MainContainerProps = {
  description: string;
} & React.ComponentPropsWithoutRef<"div">;

// Component.
export default function MainContainer({ className, ...props }: MainContainerProps): React.ReactNode {
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

  // Template.
  return (
    <div id="main-container" className={joinClassName("m-5", className)} {...props}>
      <div className="flex flex-col mb-5">
        <span className="text-2xl">{pageTitle}</span>
        <span className="text-md opacity-50">{props.description}</span>
      </div>
      {props.children}
    </div>
  );
}