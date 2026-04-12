import React, { useState, useEffect, startTransition } from "react";
import { Link } from "react-router";
import { useApi } from "@/api";
import { createProductListModel } from "@/models";
import { useInitialRendering } from "@/hooks";

// Child components.
import { ArchiveBoxIcon, TagIcon } from "@heroicons/react/24/outline";

// Props.
type Props = {
  productCategoryModel: ProductCategoryDetailModel;
};

// Component.
export default function ProductListPanel(props: Props): React.ReactNode {
  // Dependencies.
  const api = useApi();

  // States.
  const isInitialRendering = useInitialRendering();
  const [model, setModel] = useState<ProductListModel>(() => {
    const m = createProductListModel();
    m.resultsPerPage = 9;
    m.category = props.productCategoryModel.toBasicModel();
    return m;
  });

  // Effect.
  useEffect(() => {
    startTransition(async () => {
      const responseDto = await api.product.getListAsync(model.toRequestDto());
      setModel(m => m.mapFromResponseDto(responseDto));
    });
  }, [model.page]);

  // Template.
  let listElement: React.ReactNode;
  if (isInitialRendering) {
    listElement = (
      <div className="flex justify-center items-center p-5 opacity-50">
        Đang tải...
      </div>
    );
  } else if (model.items.length) {
    listElement = (
      <ul className="list-group list-group-flush">
        {model.items.map((item, index) => (
          <Product model={item} key={index} />
        ))}
      </ul>
    );
  } else {
    listElement = (
      <div className="flex justify-center items-center p-5 opacity-50">
        Không có sản phẩm nào
      </div>
    );
  }

  return (
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">Sản phẩm</span>
      </div>

      <div className="panel-body grid grid-cols-1">
        {listElement}
      </div>
    </div>
  );
}

function Product(props: { model: ProductListProductModel }): React.ReactNode {
  // Template.
  return (
    <li className="list-group-item flex gap-3 justify-start items-start px-3 py-2">
      {props.model.thumbnailUrl ? (
        <img src={props.model.thumbnailUrl} className="img-thumbnail size-11" alt={props.model.name} />
      ) : (
        <div className="img-thumbnail size-11 flex justify-center items-center">
          <ArchiveBoxIcon className="size-6 opacity-50" />
        </div>
      )}

      <div className="flex flex-col">
        <Link className="font-bold text-blue-600 dark:text-blue-400" to={props.model.detailRoutePath}>
          {props.model.name}
        </Link>

        {props.model.category && (
          <div className="flex justify-start items-center gap-1 text-sm">
            <TagIcon className="size-3.5" />
            <span>{props.model.category.name}</span>
          </div>
        )}
      </div>
    </li>
  );
}