import type { RouteRecordRaw } from "vue-router";

// Pages.
const UserListPage = () => import("@/pages/user/userList/UserListPage.vue");

// Routes.
export const userRoutes: RouteRecordRaw = {
  path: "/tai-khoan",
  name: "user",
  meta: {
    breadcrumbItem: {
      text: "Tài khoản",
      to: { name: "user" }
    }
  },
  children: [
    {
      path: "/",
      component: UserListPage,
      meta: {
        pageTitle: "Danh sách tài khoản",
        breadcrumbItem: {
          text: "Danh sách",
          to: { name: "user-list" }
        }
      },
    }
  ]
};