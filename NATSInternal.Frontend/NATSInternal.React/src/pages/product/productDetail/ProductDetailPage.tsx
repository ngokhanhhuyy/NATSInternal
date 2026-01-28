import React from "react";
import { Link, useLoaderData } from "react-router";
import { useApi } from "@/api";
import { createProductDetailModel } from "@/models";

// Child components.
import { MainContainer } from "@/components/layouts";
import DetailPanel from "./DetailPanel";
import ManagementPanel from "./ManagementPanel";
import StockPanel from "./StockPanel";
import PhotoPanel from "./PhotoPanel";
import LatestTransactionPanel from "./LatestTransactionPanel";
import { PencilSquareIcon } from "@heroicons/react/24/outline";

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
      <div className="flex flex-col gap-3 w-full">
        <div className="flex justify-start">
          <Link className="btn gap-1.5" to={model.updateRoutePath}>
            <PencilSquareIcon className="size-4" />
            <span>Chỉnh sửa</span>
          </Link>
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-2 gap-3 items-stretch">
          <div className="flex flex-col gap-y-3">
            <DetailPanel model={model} />
            <StockPanel model={model} />
            <ManagementPanel model={model} />
          </div>
          
          <PhotoPanel model={model} />
        </div>

        <LatestTransactionPanel />

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

const description = "Thông tin chi tiết của sản phẩm, tình trạng lưu kho và các giao dịch liên quan gần nhất";