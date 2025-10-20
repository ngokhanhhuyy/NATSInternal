const routeHelper = {
  getHomeRoutePath: () => "/",
  getSignInRoutePath: () => "/signIn",
  getUserListRoutePath: () => "/users",
  getUserProfileRoutePath: (id: number) => `/users/${id}`,
  getUserCreateRoutePath: () => `/users/create`,
  getUserUpdateRoutePath: (id: number) => `/users/${id}/update`,
  getUserPasswordChangeRoutePath: () => "/users/changePassword",
  getUserPasswordResetRoutePath: (id: number) => `/users/${id}/resetPassword`,

  getCustomerListRoutePath: () => "/customers",
  getCustomerDetailRoutePath: (id: number) => `/customers/${id}`,
  getCustomerCreateRoutePath: () => "/customers/create",
  getCustomerUpdateRoutePath: (id: number) => `/customers/${id}/update`,

  getProductListRoutePath: () => "/products",
  getProductDetailRoutePath: (id: number) => `/products/${id}`,
  getProductCreateRoutePath: () => `/products/create`,
  getProductUpdateRoutePath: (id: number) => `/products/${id}/update`,

  getProductCategoryCreateRoutePath: () => "/products/categories/create",
  getProductCategoryUpdateRoutePath: (id: number) => `/products/categories/${id}/update`,

  getBrandCreateRoutePath: () => "/products/brands/create",
  getBrandUpdateRoutePath: (id: number) => `/products/brands/${id}/update`,

  getSupplyListRoutePath: () => "/supplies",
  getSupplyDetailRoutePath: (id: number) => `/supplies/${id}`,
  getSupplyCreateRoutePath: () => "/supplies/create",
  getSupplyUpdateRoutePath: (id: number) => `/supplies/${id}/update`,

  getExpenseListRoutePath: () => "/expenses",
  getExpenseDetailRoutePath: (id: number) => `/expenses/${id}`,
  getExpenseCreateRoutePath: () => "/expenses/create",
  getExpenseUpdateRoutePath: (id: number) => `/expenses/${id}/update`,

  getConsultantListRoutePath: () => "/consultants",
  getConsultantDetailRoutePath: (id: number) => `/consultants/${id}`,
  getConsultantCreateRoutePath: () => "/consultants/create",
  getConsultantUpdateRoutePath: (id: number) => `/consultants/${id}/update`,

  getOrderListRoutePath: () => "/orders",
  getOrderDetailRoutePath: (id: number) => `/orders/${id}`,
  getOrderCreateRoutePath: () => "/orders/create",
  getOrderUpdateRoutePath: (id: number) => `/orders/${id}/update`,

  getTreatmentListRoutePath: () => "/treatments",
  getTreatmentDetailRoutePath: (id: number) => `/treatments/${id}`,
  getTreatmentCreateRoutePath: () => "/treatments/create",
  getTreatmentUpdateRoutePath: (id: number) => `/treatments/${id}/update`,

  getDebtOverviewRoutePath: () => "/debts/",

  getDebtIncurrenceListRoutePath: () => "/debts/incurrences",
  getDebtIncurrenceDetailRoutePath: (id: number) => `/debts/incurrences/${id}`,
  getDebtIncurrenceCreateRoutePath: () => "/debts/incurrences/create",
  getDebtIncurrenceUpdateRoutePath: (id: number) => `/debts/incurrences/${id}/update`,

  getDebtPaymentListRoutePath: () => "/debts/payments",
  getDebtPaymentDetailRoutePath: (id: number) => `/debts/payments/${id}`,
  getDebtPaymentCreateRoutePath: () => "/debts/payments/create",
  getDebtPaymentUpdateRoutePath: (id: number) => `/debts/payments/${id}/update`,

  getReportRoutePath: () => "/report"
};

export function useRouteHelper() {
  return routeHelper;
}