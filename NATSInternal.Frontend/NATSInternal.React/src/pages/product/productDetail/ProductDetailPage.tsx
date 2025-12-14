import React from "react";
import { useLoaderData, Link } from "react-router";

// Child components.
import { MainContainer } from "@/components/layouts";
import { Block } from "@/components/ui";

// Components.
export default function ProductDetailPage(): React.ReactNode {
  // Dependencies.
  const model = useLoaderData<ProductDetailModel>

  // Templates.
  return (
    <MainContainer description={description}>
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-3">

      </div>
    </MainContainer>
  );
}

const description = "Thông tin chi tiết của sản phẩm, tình trạng lưu kho và các giao dịch liên quan gần nhất";