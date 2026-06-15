import React from "react";
import { useLoaderData, Link } from "react-router";
import { api } from "@/api";
import { createProductCategoryBasicModel } from "@/models";
import { getProductCategoryCreateRoutePath } from "@/helpers";

// Child components.
import { MainContainer } from "@/components/layouts";
import ProductCategoryListResults from "@/pages/product/ProductCategoryListResults";
import { PlusIcon } from "@heroicons/react/24/outline";

// Data loader.
export async function loadDataAsync(): Promise<ProductCategoryBasicModel[]> {
  const responseDtos = await api.productCategory.getAllAsync();
  return responseDtos.map(createProductCategoryBasicModel);
}

// Components.
export default function ProductListPage(): React.ReactNode {
  // Dependencies.
  const model = useLoaderData<ProductCategoryBasicModel[]>();

  // Template.
  return (
    <MainContainer className="gap-3">
      <div className="panel">
        <div className="panel-header">
          <span className="panel-header-title">
            Danh sách phân loại sản phẩm
          </span>
        </div>

        <div className="panel-body p-3">
          <div className="panel-body-area">
            <ProductCategoryListResults model={model} />
          </div>
        </div>
      </div>

      <div className="flex justify-end">
        <Link className="btn" to={getProductCategoryCreateRoutePath()}>
          <PlusIcon/>
          <span>Tạo phân loại mới</span>
        </Link>
      </div>
    </MainContainer>
  );
}
