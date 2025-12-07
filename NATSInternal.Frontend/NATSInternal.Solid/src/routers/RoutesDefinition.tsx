import { lazy } from "solid-js";
import { Route, Router } from "@solidjs/router";

import SignInPage from "@/pages/authentication/signIn/SignInPage";
const HomePage = lazy(() => import("@/pages/home/HomePage"));

export default function RoutesDefinition() {
  return (
    <Router>
      <Route
      <Route
        path="/"
        component={() => <HomePage />}
        info={{ pageTitle: "Bảng điều khiển", breadcrumbTitle: "Bảng điều khiển" }}
      />
      <Route path="/signIn" component={() => <SignInPage />} />
    </Router>
  );
}