import React from "react";
import { Link, useLoaderData } from "react-router";

// Child components.
import DetailPanel from "./DetailPanel";
import ManagementPanel from "./ManagementPanel";
import ProductListPanel from "./ProductListPanel";
import { MainContainer } from "@/components/layouts";
import { PencilSquareIcon } from "@heroicons/react/24/outline";

// Components.
export default function BrandDetailPage(): React.ReactNode {
  // Dependencies.
  const model = useLoaderData<BrandDetailModel>();

  // Template.
  return (
    <MainContainer className="gap-3">
      <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
        <div className="flex flex-col gap-3">
          <DetailPanel model={model} />
          <ManagementPanel model={model} />
        </div>

        <ProductListPanel brandModel={model} />
      </div>

      <div className="flex justify-end">
        <Link className="btn gap-1.5" to={model.updateRoutePath}>
          <PencilSquareIcon className="size-4" />
          <span>Chỉnh sửa</span>
        </Link>
      </div>
    </MainContainer>
  );
}