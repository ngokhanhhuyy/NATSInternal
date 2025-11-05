const routeHelper = {
  getSignInRoutePath: (returningPath?: string) => {
    let path = "/dang-nhap";
    if (returningPath && returningPath.length) {
      path += `?${returningPath}`;
    }

    return path;
  },

  getHomeRoutePath: () => "/",
  getUserListRoutePath: () => "/nguoi-dung",
  getUserProfileRoutePath: (id: number) => `/nguoi-dung/${id}`,
  getUserCreateRoutePath: () => `/nguoi-dung/tao-moi`,
  getUserUpdateRoutePath: (id: number) => `/nguoi-dung/${id}/chinh-sua`,
  getUserPasswordChangeRoutePath: () => "/nguoi-dung/doi-mat-khau",
  getUserPasswordResetRoutePath: (id: number) => `/nguoi-dung/${id}/reset-mat-khau`,

  getCustomerListRoutePath: () => "/khach-hang",
  getCustomerDetailRoutePath: (id: number) => `/khach-hang/${id}`,
  getCustomerCreateRoutePath: () => "/khach-hang/tao-moi",
  getCustomerUpdateRoutePath: (id: number) => `/khach-hang/${id}/chinh-sua`,

  getProductListRoutePath: () => "/san-pham",
  getProductDetailRoutePath: (id: number) => `/san-pham/${id}`,
  getProductCreateRoutePath: () => `/san-pham/tao-moi`,
  getProductUpdateRoutePath: (id: number) => `/san-pham/${id}/chinh-sua`,

  getProductCategoryCreateRoutePath: () => "/san-pham/phan-loai/tao-moi",
  getProductCategoryUpdateRoutePath: (id: number) => `/san-pham/phan-loai/${id}/chinh-sua`,

  getBrandCreateRoutePath: () => "/san-pham/thuong-hieu/tao-moi",
  getBrandUpdateRoutePath: (id: number) => `/san-pham/thuong-hieu/${id}/chinh-sua`,

  getSupplyListRoutePath: () => "/nhap-hang",
  getSupplyDetailRoutePath: (id: number) => `/nhap-hang/${id}`,
  getSupplyCreateRoutePath: () => "/nhap-hang/tao-moi",
  getSupplyUpdateRoutePath: (id: number) => `/nhap-hang/${id}/chinh-sua`,

  getExpenseListRoutePath: () => "/chi-phis",
  getExpenseDetailRoutePath: (id: number) => `/chi-phis/${id}`,
  getExpenseCreateRoutePath: () => "/chi-phis/tao-moi",
  getExpenseUpdateRoutePath: (id: number) => `/chi-phis/${id}/chinh-sua`,

  getConsultantListRoutePath: () => "/tu-van",
  getConsultantDetailRoutePath: (id: number) => `/tu-van/${id}`,
  getConsultantCreateRoutePath: () => "/tu-van/tao-moi",
  getConsultantUpdateRoutePath: (id: number) => `/tu-van/${id}/chinh-sua`,

  getOrderListRoutePath: () => "/don-hang",
  getOrderDetailRoutePath: (id: number) => `/don-hang/${id}`,
  getOrderCreateRoutePath: () => "/don-hang/tao-moi",
  getOrderUpdateRoutePath: (id: number) => `/don-hang/${id}/chinh-sua`,

  getTreatmentListRoutePath: () => "/lieu-trinh",
  getTreatmentDetailRoutePath: (id: number) => `/lieu-trinh/${id}`,
  getTreatmentCreateRoutePath: () => "/lieu-trinh/tao-moi",
  getTreatmentUpdateRoutePath: (id: number) => `/lieu-trinh/${id}/chinh-sua`,

  getDebtOverviewRoutePath: () => "/no/",

  getDebtIncurrenceListRoutePath: () => "/no/phat-sinh",
  getDebtIncurrenceDetailRoutePath: (id: number) => `/no/phat-sinh/${id}`,
  getDebtIncurrenceCreateRoutePath: () => "/no/phat-sinh/tao-moi",
  getDebtIncurrenceUpdateRoutePath: (id: number) => `/no/phat-sinh/${id}/chinh-sua`,

  getDebtPaymentListRoutePath: () => "/no/thanh-toan",
  getDebtPaymentDetailRoutePath: (id: number) => `/no/thanh-toan/${id}`,
  getDebtPaymentCreateRoutePath: () => "/no/thanh-toan/tao-moi",
  getDebtPaymentUpdateRoutePath: (id: number) => `/no/thanh-toan/${id}/chinh-sua`,

  getReportRoutePath: () => "/bao-cao"
};

export function useRouteHelper() {
  return routeHelper;
}