import React from "react";
import { Link, useLoaderData } from "react-router";
import { api } from "@/api";
import { createCustomerDetailModel } from "@/models/customer/customerDetailModel";

// Child components.
import { MainContainer } from "@/components/layouts";
import AvatarAndNamePanel from "./AvatarAndNamePanel";
import DetailPanel from "./DetailPanel";
import RecentOrdersPanel from "./RecentOrdersPanel";
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
        <DetailPanel model={model} />
        
        <RecentOrdersPanel model={model} />

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
