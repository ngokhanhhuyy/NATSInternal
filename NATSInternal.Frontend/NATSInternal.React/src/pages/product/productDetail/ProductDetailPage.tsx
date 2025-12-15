import React from "react";
import { useLoaderData } from "react-router";
import { useApi } from "@/api";
import { createProductDetailModel } from "@/models";

// Child components.
import { MainContainer } from "@/components/layouts";
import DetailBlock from "./DetailBlock.tsx";
import ManagementBlock from "./ManagementBlock";
import TransactionBlock from "./TransactionBlock";

// Api.
const api = useApi();

// Data loader.
export async function loadDataAsync(id: string): Promise<ProductDetailModel> {
  const responseDto = await api.product.getDetailAsync(id as string);
  return createProductDetailModel(responseDto);
}

// Components.
export default function ProductDetailPage(): React.ReactNode {
  // Dependencies.
  const model = useLoaderData<ProductDetailModel>();

  // Templates.
  return (
    <MainContainer description={description}>
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-5 items-start">
        <div className="flex flex-col gap-y-5">
          <DetailBlock model={model} />
          <ManagementBlock model={model} />
        </div>

        <TransactionBlock />
      </div>
    </MainContainer>
  );
}

const description = "Thông tin chi tiết của sản phẩm, tình trạng lưu kho và các giao dịch liên quan gần nhất";