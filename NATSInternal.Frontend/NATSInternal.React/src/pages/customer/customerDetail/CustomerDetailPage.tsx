import React from "react";
import { Link, useLoaderData } from "react-router";
import { api } from "@/api";
import { createCustomerDetailModel } from "@/models/customer/customerDetailModel";

// Child components.
import { MainContainer } from "@/components/layouts";
import AvatarAndNamePanel from "./AvatarAndNamePanel";
import PersonalInformationBlock from "./PersonalInformationPanel";
import ManagementPanel from "./ManagementPanel";
import DebtPanel from "./DebtPanel";
import RecentTransactionPanel from "./RecentTransactionsPanel";
import { PencilSquareIcon } from "@heroicons/react/24/outline";

// Data loder.
export async function loadDataAsync(id: number): Promise<CustomerDetailModel> {
  const responseDto = await api.customer.getDetailAsync(id);
  return createCustomerDetailModel(responseDto);
}

// Components.
export default function CustomerDetailPage(): React.ReactNode {
  // Dependencies.
  const model = useLoaderData<CustomerDetailModel>();

  // Template.
  return (
    <MainContainer>
      <div className="flex flex-col gap-3">
        <AvatarAndNamePanel model={model} />
        <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
          <div className="flex flex-col gap-3">
            <PersonalInformationBlock model={model} />
            <DebtPanel model={model} />
            <ManagementPanel model={model} />
          </div>
          <RecentTransactionPanel model={model} />
        </div>

        <div className="flex justify-end">
          <Link className="btn gap-1.5" to={model.updateRoutePath}>
            <PencilSquareIcon className="size-4" />
            <span>Chỉnh sửa</span>
          </Link>
        </div>
      </div>
    </MainContainer>
  );
}
