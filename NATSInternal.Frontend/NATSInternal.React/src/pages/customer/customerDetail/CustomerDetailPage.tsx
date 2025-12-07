import React from "react";
import { Link, useLoaderData } from "react-router";
import { useApi } from "@/api";
import { createCustomerDetailModel } from "@/models/customers/customerDetailModel";

// Child components.
import MainContainer from "@/components/layouts/MainContainer";
import PersonalInformationBlock from "./PersonalInformationBlock";
import ManagementBlock from "./ManagementBlock";
import DebtBlock from "./DebtBlock";
import RecentTransactionsBlock from "./RecentTransactions";
import { PencilSquareIcon } from "@heroicons/react/24/outline";

// Data loder.
export async function loadDataAsync(id: string): Promise<CustomerDetailModel> {
  const api = useApi();
  const responseDto = await api.customer.getDetailAsync(id);
  return createCustomerDetailModel(responseDto);
}

// Components.
export default function CustomerDetailPage(): React.ReactNode {
  // Dependencies.
  const model = useLoaderData<CustomerDetailModel>();

  // Template.
  return (
    <MainContainer description={
      "Thông tin chi tiết về khách hàng, bao gồm thông tin cá nhân " +
      "và thông tin về nợ các giao dịch gần nhất"
    }>
      <div className="flex flex-col gap-3">
        <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
          <div className="flex flex-col gap-3">
            <PersonalInformationBlock model={model} />
            <DebtBlock model={model} />
            <ManagementBlock model={model} />
          </div>
          <RecentTransactionsBlock model={model} />
        </div>

        <div className="flex justify-end">
          <Link className="button gap-1.5" to={model.updateRoute}>
            <PencilSquareIcon className="size-4" />
            <span>Chỉnh sửa</span>
          </Link>
        </div>
      </div>
    </MainContainer>
  );
}