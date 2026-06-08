import React from "react";
import { Link, useLoaderData } from "react-router";
import { api } from "@/api";
import { createProductDetailModel } from "@/models";

// Child components.
import { MainContainer } from "@/components/layouts";
import DetailPanel from "./DetailPanel";
import ThumbnailAndNamePanel from "./ThumbnailAndNamePanel";
import PhotoPanel from "./PhotoPanel";
import RecentOrdersPanel from "@/pages/shared/detail/recentOrdersPanel";
import { PencilSquareIcon } from "@heroicons/react/24/outline";

// Data loader.
export async function loadDataAsync(id: number): Promise<ProductDetailModel> {
  const responseDto = await api.product.getDetailAsync(id);
  return createProductDetailModel(responseDto);
}

// Components.
export default function ProductDetailPage(): React.ReactNode {
  // Dependencies.
  const model = useLoaderData<ProductDetailModel>();

  // Templates.
  return (
    <MainContainer>
      <div className="flex flex-col gap-3 w-full">
        <ThumbnailAndNamePanel model={model} />
        <DetailPanel model={model} />

        {model.photos.length > 0 && (
          <PhotoPanel model={model} />
        )}

        <RecentOrdersPanel productModel={model} />

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
