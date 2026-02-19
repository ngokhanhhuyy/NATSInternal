const routeHelper = {
  getSignInRoutePath: (returningPath?: string) => {
    let path = "/sign-in";
    if (returningPath && returningPath.length) {
      path += `?${returningPath}`;
    }

    return path;
  },

  getHomeRoutePath: () => "/",
  getDashboardRoutePath: () => "/dashboard",
  getUserListRoutePath: () => "/users",
  getUserProfileRoutePath: (id: string) => `/users/${id}`,
  getUserCreateRoutePath: () => `/users/create`,
  getUserUpdateRoutePath: (id: string) => `/users/${id}/update`,
  getUserPasswordChangeRoutePath: () => "/users/change-password",
  getUserPasswordResetRoutePath: (id: string) => `/users/${id}/reset-password`,

  getCustomerListRoutePath: () => "/customers",
  getCustomerDetailRoutePath: (id: string) => `/customers/${id}`,
  getCustomerCreateRoutePath: () => "/customers/create",
  getCustomerUpdateRoutePath: (id: string) => `/customers/${id}/update`,

  getProductListRoutePath: () => "/products",
  getProductDetailRoutePath: (id: string) => `/products/${id}`,
  getProductCreateRoutePath: () => `/products/create`,
  getProductUpdateRoutePath: (id: string) => `/products/${id}/update`,

  getProductCategoryListRoutePath: () => "/products/categories",
  getProductCategoryCreateRoutePath: () => "/products/categories/create",
  getProductCategoryUpdateRoutePath: (id: string) => `/products/categories/${id}/update`,

  getBrandListRoutePath: () => "/products/brands",
  getBrandDetailRoutePath: (id: string) => `/products/brands/${id}`,
  getBrandCreateRoutePath: () => "/products/brands/create",
  getBrandUpdateRoutePath: (id: string) => `/products/brands/${id}/update`,

  getSupplyListRoutePath: () => "/supplies",
  getSupplyDetailRoutePath: (id: string) => `/supplies/${id}`,
  getSupplyCreateRoutePath: () => "/supplies/create",
  getSupplyUpdateRoutePath: (id: string) => `/supplies/${id}/update`,

  getExpenseListRoutePath: () => "/expenses",
  getExpenseDetailRoutePath: (id: string) => `/expenses/${id}`,
  getExpenseCreateRoutePath: () => "/expenses/create",
  getExpenseUpdateRoutePath: (id: string) => `/expenses/${id}/update`,

  getConsultantListRoutePath: () => "/consultants",
  getConsultantDetailRoutePath: (id: string) => `/consultants/${id}`,
  getConsultantCreateRoutePath: () => "/consultants/create",
  getConsultantUpdateRoutePath: (id: string) => `/consultants/${id}/update`,

  getOrderListRoutePath: () => "/orders",
  getOrderDetailRoutePath: (id: string) => `/orders/${id}`,
  getOrderCreateRoutePath: () => "/orders/create",
  getOrderUpdateRoutePath: (id: string) => `/orders/${id}/update`,

  getTreatmentListRoutePath: () => "/treatments",
  getTreatmentDetailRoutePath: (id: string) => `/treatments/${id}`,
  getTreatmentCreateRoutePath: () => "/treatments/create",
  getTreatmentUpdateRoutePath: (id: string) => `/treatments/${id}/update`,

  getDebtOverviewRoutePath: () => "/debts/",

  getDebtIncurrenceListRoutePath: () => "/debts/incurrences",
  getDebtIncurrenceDetailRoutePath: (id: string) => `/debts/incurrences/${id}`,
  getDebtIncurrenceCreateRoutePath: () => "/debts/incurrences/create",
  getDebtIncurrenceUpdateRoutePath: (id: string) => `/debts/incurrences/${id}/update`,

  getDebtPaymentListRoutePath: () => "/debts/payments",
  getDebtPaymentDetailRoutePath: (id: string) => `/debts/payments/${id}`,
  getDebtPaymentCreateRoutePath: () => "/debts/payments/create",
  getDebtPaymentUpdateRoutePath: (id: string) => `/debts/payments/${id}/update`,

  getReportRoutePath: () => "/reports"
};

export function useRouteHelper() {
  return routeHelper;
}