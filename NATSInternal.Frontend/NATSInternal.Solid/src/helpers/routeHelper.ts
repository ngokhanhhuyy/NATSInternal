const routeHelper = {
  getSignInRoutePath: (returningPath?: string) => {
    let path = "/dang-nhap";
    if (returningPath && returningPath.length) {
      path += `?${returningPath}`;
    }

    return path;
  },

  getHomeRoutePath: () => "/",
  getDashboardRoutePath: () => "/bang-dieu-khien",
  getUserListRoutePath: () => "/nguoi-dung",
  getUserProfileRoutePath: (id: string) => `/nguoi-dung/${id}`,
  getUserCreateRoutePath: () => `/nguoi-dung/tao-moi`,
  getUserUpdateRoutePath: (id: string) => `/nguoi-dung/${id}/chinh-sua`,
  getUserPasswordChangeRoutePath: () => "/nguoi-dung/doi-mat-khau",
  getUserPasswordResetRoutePath: (id: string) => `/nguoi-dung/${id}/reset-mat-khau`,

  getCustomerListRoutePath: () => "/khach-hang",
  getCustomerDetailRoutePath: (id: string) => `/khach-hang/${id}`,
  getCustomerCreateRoutePath: () => "/khach-hang/tao-moi",
  getCustomerUpdateRoutePath: (id: string) => `/khach-hang/${id}/chinh-sua`,

  getProductListRoutePath: () => "/san-pham",
  getProductDetailRoutePath: (id: string) => `/san-pham/${id}`,
  getProductCreateRoutePath: () => `/san-pham/tao-moi`,
  getProductUpdateRoutePath: (id: string) => `/san-pham/${id}/chinh-sua`,

  getProductCategoryListRoutePath: () => "/san-pham/phan-loai",
  getProductCategoryCreateRoutePath: () => "/san-pham/phan-loai/tao-moi",
  getProductCategoryUpdateRoutePath: (id: string) => `/san-pham/phan-loai/${id}/chinh-sua`,

  getBrandListRoutePath: () => "/san-pham/thuong-hieu",
  getBrandDetailRoutePath: (id: string) => `/san-pham/thuong-hieu/${id}`,
  getBrandCreateRoutePath: () => "/san-pham/thuong-hieu/tao-moi",
  getBrandUpdateRoutePath: (id: string) => `/san-pham/thuong-hieu/${id}/chinh-sua`,

  getSupplyListRoutePath: () => "/nhap-hang",
  getSupplyDetailRoutePath: (id: string) => `/nhap-hang/${id}`,
  getSupplyCreateRoutePath: () => "/nhap-hang/tao-moi",
  getSupplyUpdateRoutePath: (id: string) => `/nhap-hang/${id}/chinh-sua`,

  getExpenseListRoutePath: () => "/chi-phi",
  getExpenseDetailRoutePath: (id: string) => `/chi-phi/${id}`,
  getExpenseCreateRoutePath: () => "/chi-phi/tao-moi",
  getExpenseUpdateRoutePath: (id: string) => `/chi-phi/${id}/chinh-sua`,

  getConsultantListRoutePath: () => "/tu-van",
  getConsultantDetailRoutePath: (id: string) => `/tu-van/${id}`,
  getConsultantCreateRoutePath: () => "/tu-van/tao-moi",
  getConsultantUpdateRoutePath: (id: string) => `/tu-van/${id}/chinh-sua`,

  getOrderListRoutePath: () => "/don-hang",
  getOrderDetailRoutePath: (id: string) => `/don-hang/${id}`,
  getOrderCreateRoutePath: () => "/don-hang/tao-moi",
  getOrderUpdateRoutePath: (id: string) => `/don-hang/${id}/chinh-sua`,

  getTreatmentListRoutePath: () => "/lieu-trinh",
  getTreatmentDetailRoutePath: (id: string) => `/lieu-trinh/${id}`,
  getTreatmentCreateRoutePath: () => "/lieu-trinh/tao-moi",
  getTreatmentUpdateRoutePath: (id: string) => `/lieu-trinh/${id}/chinh-sua`,

  getDebtOverviewRoutePath: () => "/khoan-no/",

  getDebtIncurrenceListRoutePath: () => "/khoan-no/phat-sinh",
  getDebtIncurrenceDetailRoutePath: (id: string) => `/khoan-no/phat-sinh/${id}`,
  getDebtIncurrenceCreateRoutePath: () => "/khoan-no/phat-sinh/tao-moi",
  getDebtIncurrenceUpdateRoutePath: (id: string) => `/khoan-no/phat-sinh/${id}/chinh-sua`,

  getDebtPaymentListRoutePath: () => "/khoan-no/thanh-toan",
  getDebtPaymentDetailRoutePath: (id: string) => `/khoan-no/thanh-toan/${id}`,
  getDebtPaymentCreateRoutePath: () => "/khoan-no/thanh-toan/tao-moi",
  getDebtPaymentUpdateRoutePath: (id: string) => `/khoan-no/thanh-toan/${id}/chinh-sua`,

  getReportRoutePath: () => "/bao-cao"
};

export function useRouteHelper() {
  return routeHelper;
}