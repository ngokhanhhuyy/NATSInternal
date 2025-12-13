import React from "react";
import { Link } from "react-router";
import { useLoaderData } from "react-router";
import { useApi } from "@/api";
import { createCustomerListModel } from "@/models";
import { useTsxHelper } from "@/helpers";

// Child components.
import SearchablePageableListPage from "@/pages/shared/searchablePageableList/SearchablePageableListPage";

// Api.
const api = useApi();

// Loader
export async function loadDataAsync(model?: CustomerListModel): Promise<CustomerListModel> {
  if (model) {
    const responseDto = await api.customer.getListAsync(model.toRequestDto());
    return model.mapFromResponseDto(responseDto);
  }

  const responseDto = await api.customer.getListAsync();
  return createCustomerListModel(responseDto);
}

// Component.
export default function CustomerListPage(): React.ReactNode {
  // Dependencies.
  const initialModel = useLoaderData<CustomerListModel>();
  const { joinClassName } = useTsxHelper();

  // Template.
  return (
    <SearchablePageableListPage<CustomerListModel, CustomerListCustomerModel>
      description="Danh sách các khách hàng đã và đang giao dịch với cửa hàng."
      initialModel={initialModel}
      loadDataAsync={loadDataAsync}
      renderTableHeaderRowChildren={() => (
        <>
          <th>Họ và tên</th>
          <th>Biệt danh</th>
          <th>Giới tính</th>
          <th>Số điện thoại</th>
          <th>Ngày sinh</th>
          <th>Nợ còn lại</th>
        </>
      )}
      renderTableBodyRowChildren={(itemModel) => (
        <>
          <td className="px-3 py-2">
            <Link to={itemModel.detailRoute} className="font-bold">
              {itemModel.fullName}
            </Link>
          </td>

          <td className="px-3 py-2">
            {itemModel.nickName && (
              <span className="opacity-50">{itemModel.nickName}</span>
            )}
          </td>

          <td className={joinClassName(
            "px-3 py-2",
            itemModel.gender === "Male" ? "text-blue-700 dark:text-blue-400" : "text-red-700 dark:text-red-400"
          )}>
            {itemModel.gender === "Male" ? "Nam" : "Nữ"}
          </td>

          <td className="px-3 py-2">
            {itemModel.formattedPhoneNumber}
          </td>

          <td className="px-3 py-2">
            {itemModel.formattedBirthday}
          </td>

          <td className={joinClassName("px-3 py-2", !itemModel.debtRemainingAmount && "opacity-25")}>
            {itemModel.formattedDebtRemainingAmount}
          </td>
        </>
      )}
    />
  );
}