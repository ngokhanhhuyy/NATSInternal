import type { RouteObject } from "react-router";

// import HomePage from "@/pages/home/HomePage";
import TestingPage from "@/pages/TestingPage";

export const homeRoutes: RouteObject = {
  path: "bang-dieu-khien",
  Component: TestingPage,
  handle: {
    breadcrumbTitle: "Bảng điều khiển",
    pageTitle: "Bảng điều khiển",
    description: "Trang kiểm tra dành để kiểm tra các component."
  }
};