import { createMemoryHistory, createRouter, type RouteRecordRaw } from "vue-router";
import { useAuthenticationStore } from "@/stores";

// Layouts.
import RootLayout from "@/components/layouts/RootLayout.vue";
import MainPageLayout from "@/components/layouts/MainPageLayout.vue";

// Routes.
import { authenticationRoutes } from "./authenticationRoutes";
import { homeRoutes } from "./homeRoutes";
import { customerRoutes } from "./customerRoutes";
import { productRoutes } from "./productRoutes";
import { expenseRoutes } from "./expenseRoutes";
import { supplyRoutes } from "./supplyRoutes";
import { orderRoutes } from "./orderRoutes";
import { debtRoutes } from "./debtRoutes";
import { reportRoutes } from "./reportRoutes";
import { userRoutes } from "./userRoutes";

// Router.
const routes: RouteRecordRaw[] = [
  {
    path: "/",
    component: RootLayout,
    children: [
      authenticationRoutes,
      {
        path: "/",
        component: MainPageLayout,
        children: [
          homeRoutes,
          customerRoutes,
          productRoutes,
          supplyRoutes,
          expenseRoutes,
          orderRoutes,
          debtRoutes,
          reportRoutes,
          userRoutes
        ]
      }
    ]
  }
];

const router = createRouter({
  history: createMemoryHistory(),
  routes
});

router.beforeEach((to) => {
  const { isAuthenticated } = useAuthenticationStore();
  if (to.name !== "sign-in" && !isAuthenticated) {
    return { name: "sign-in" };
  }
  
  if (to.name === "sign-in" && isAuthenticated) {
    return { name: "home" };
  }
});

export { router };