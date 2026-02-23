import { useMatches, type RouteHandle } from "react-router";

export function useMatchedRouteHandles(): (RouteHandle | null)[] {
  // Dependencies.
  const matchedRoutes = useMatches();

  return matchedRoutes.map(route => (route.handle as RouteHandle | undefined) ?? null);
}