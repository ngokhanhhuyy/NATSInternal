export const getSignInRoutePath = (returningPath?: string) => {
  let path = "/dang-nhap";
  if (returningPath && returningPath.length) {
    path += `?${returningPath}`;
  }

  return path;
};

export const getHomeRoutePath = () => "/";
export const getDashboardRoutePath = () => "/bang-dieu-khien";
export const getUserListRoutePath = () => "/nguoi-dung";
export const getUserDetailRoutePath = (id: number) => `/nguoi-dung/${id}`;
export const getUserCreateRoutePath = () => `/nguoi-dung/tao-moi`;
export const getUserPasswordChangeRoutePath = () => "/nguoi-dung/doi-mat-khau";
export const getUserPasswordResetRoutePath = (id: number) => `/nguoi-dung/${id}/reset-mat-khau`;

export const getCustomerListRoutePath = () => "/khach-hang";
export const getCustomerDetailRoutePath = (id: number) => `/khach-hang/${id}`;
export const getCustomerCreateRoutePath = () => "/khach-hang/tao-moi";
export const getCustomerUpdateRoutePath = (id: number) => `/khach-hang/${id}/chinh-sua`;

export const getProductListRoutePath = () => "/san-pham";
export const getProductDetailRoutePath = (id: number) => `/san-pham/${id}`;
export const getProductCreateRoutePath = () => `/san-pham/tao-moi`;
export const getProductUpdateRoutePath = (id: number) => `/san-pham/${id}/chinh-sua`;

export const getProductCategoryListRoutePath = () => "/san-pham/phan-loai";
export const getProductCategoryDetailRoutePath = (id: number) => `/san-pham/phan-loai/${id}`;
export const getProductCategoryCreateRoutePath = () => "/san-pham/phan-loai/tao-moi";
export const getProductCategoryUpdateRoutePath = (id: number) => `/san-pham/phan-loai/${id}/chinh-sua`;

export const getSupplyListRoutePath = () => "/nhap-hang";
export const getSupplyDetailRoutePath = (id: number) => `/nhap-hang/${id}`;
export const getSupplyCreateRoutePath = () => "/nhap-hang/tao-moi";
export const getSupplyUpdateRoutePath = (id: number) => `/nhap-hang/${id}/chinh-sua`;

export const getExpenseListRoutePath = () => "/chi-phi";
export const getExpenseDetailRoutePath = (id: number) => `/chi-phi/${id}`;
export const getExpenseCreateRoutePath = () => "/chi-phi/tao-moi";
export const getExpenseUpdateRoutePath = (id: number) => `/chi-phi/${id}/chinh-sua`;

export const getOrderListRoutePath = () => "/don-hang";
export const getOrderDetailRoutePath = (id: number) => `/don-hang/${id}`;
export const getOrderCreateRoutePath = () => "/don-hang/tao-moi";
export const getOrderUpdateRoutePath = (id: number) => `/don-hang/${id}/chinh-sua`;

export const getDebtOverviewRoutePath = () => "/khoan-no/";
export const getReportRoutePath = () => "/bao-cao";
