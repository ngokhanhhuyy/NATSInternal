import type { RouteRecordRaw } from "vue-router";

// Pages.
const ReportOverviewPage = () => import("@/pages/report/reportOverview/ReportOverviewPage.vue");

// Routes.
export const reportRoutes: RouteRecordRaw = {
  path: "/bao-cao",
  name: "report",
  meta: {
    breadcrumbItem: {
      text: "Báo cáo",
      to: { name: "report" }
    }
  },
  children: [
    {
      path: "/",
      component: ReportOverviewPage,
      meta: {
        pageTitle: "Báo cáo tổng quan",
        breadcrumbItem: {
          text: "Tổng quan",
          to: { name: "report-overview" }
        }
      },
    }
  ]
};