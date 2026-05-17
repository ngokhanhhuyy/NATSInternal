import React from "react";
import { useLoaderData, Link } from "react-router";
import { api } from "@/api";
import { createProductCategoryBasicModel } from "@/models";
import { getProductCategoryCreateRoutePath } from "@/helpers";

// Child components.
import { MainContainer } from "@/components/layouts";
import { TagIcon, PlusIcon } from "@heroicons/react/24/outline";

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

        <div className="panel-body">
          <ul className="list-group border-none">
            {model.map(category => (
              <li className="list-group-item p-2 flex align-center gap-2" key={category.id}>
                <div className="img-thumbnail rounded-sm size-6 flex justify-center items-center">
                  <TagIcon className="size-5 opacity-50" />
                </div>

                <Link className="text-blue-600 dark:text-blue-500 font-bold" to={category.detailRoutePath}>
                  {category.name}
                </Link>
              </li>
            ))}
          </ul>
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