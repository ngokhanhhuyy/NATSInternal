import React, { useState, useMemo, useEffect } from "react";
import { Link } from "react-router";
import { useApi } from "@/api";
import { createBrandListModel, createProductCategoryListModel } from "@/models";
import { getDisplayName } from "@/metadata";
import { useRouteHelper, useTsxHelper } from "@/helpers";

// Child components.
import { Block } from "@/components/ui";
import { Bars4Icon } from "@heroicons/react/24/outline";

// Props.
type Props<
    TListModel extends 
      ISearchableListModel<TItemModel> &
      ISortableListModel<TItemModel> &
      IPageableListModel<TItemModel> &
      IUpsertableListModel<TItemModel>,
    TItemModel extends BrandListBrandModel | ProductCategoryListProductCategoryModel> = {
  model: TListModel;
  resourceName: string;
  isInitialLoading: boolean;
  renderItem(item: TItemModel): React.ReactNode;
  listRoutePath: string;
};

// Component.
export default function SecondaryListBlock<
      TListModel extends
        ISearchableListModel<TItemModel> &
        ISortableListModel<TItemModel> &
        IPageableListModel<TItemModel> &
        IUpsertableListModel<TItemModel>,
      TItemModel extends BrandListBrandModel | ProductCategoryListProductCategoryModel>
    (props: Props<TListModel, TItemModel>): React.ReactNode {
  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // Computed.
  const displayName = getDisplayName(props.resourceName) ?? props.resourceName;
    
  // Template.
  const headerChildren = (
    <Link to={props.listRoutePath} className="button block-header-button small gap-1">
      <Bars4Icon className="size-4"/>
      <span>Danh sách đầy đủ</span>
    </Link>
  );

  const loadingChildren = (
    <div className="flex justify-center items-center px-3 py-5">
      Đang tải..
    </div>
  );

  const renderListChildren = (): React.ReactNode => {
    if (props.model.items.length === 0) {
      return (
        <div className="flex justify-center items-center opacity-50 px-3 py-5">
          Không tìm thấy kết quả
        </div>
      );
    }

    return (
      <ul className="list-group list-group-flush">
        {props.model.items.map((item, index) => (
          <li className="px-3 py-2" key={index}>
            {props.renderItem(item)}
          </li>
        ))}

        {props.model.items.length < props.model.itemCount && (
          <li className="px-3 py-2 text-center opacity-50">
            {`...và ${props.model.itemCount - props.model.items.length} ${displayName.toLowerCase()} khác`}
          </li>
        )}
      </ul>
    );
  };

  return (
    <Block
      className={joinClassName(props.isInitialLoading && "opacity-50")}
      title={displayName}
      headerChildren={headerChildren}
    >
      {props.isInitialLoading ? loadingChildren : renderListChildren()}
    </Block>
  );
}

export function BrandListBlock(): React.ReactNode {
  // Dependencies.
  const api = useApi();
  const { getBrandListRoutePath } = useRouteHelper();

  // States.
  const [isInitialLoading, setIsInitialLoading] = useState(true);
  const [model, setModel] = useState<BrandListModel>(() => {
    const initialModel = createBrandListModel();
    initialModel.resultsPerPage = 7;
    return initialModel;
  });

  // Effect.
  useEffect(() => {
    api.brand
      .getListAsync(model.toRequestDto())
      .then(responseDto => {
        setModel(m => m.mapFromResponseDto(responseDto));
        setIsInitialLoading(false);
      });
  }, []);

  // Template.
  return (
    <SecondaryListBlock
      model={model}
      resourceName="brand"
      isInitialLoading={isInitialLoading}
      listRoutePath={getBrandListRoutePath()}
      renderItem={(item: BrandListBrandModel) => (
        <Link to={item.detailRoute} className="font-bold">
          {item.name}
        </Link>
      )}
    />
  );
}

export function ProductCategoryListBlock(): React.ReactNode {
  // Dependencies.
  const api = useApi();
  const { getProductCategoryListRoutePath } = useRouteHelper();

  // States.
  const [isInitialLoading, setIsInitialLoading] = useState(true);
  const [model, setModel] = useState<ProductCategoryListModel>(() => {
    const initialModel = createProductCategoryListModel();
    initialModel.resultsPerPage = 7;
    return initialModel;
  });

  // Effect.
  useEffect(() => {
    api.productCategory
      .getListAsync(model.toRequestDto())
      .then(responseDto => {
        setModel(m => m.mapFromResponseDto(responseDto));
        setIsInitialLoading(false);
      });
  }, []);

  // Template.
  return (
    <SecondaryListBlock
      model={model}
      resourceName="productCategory"
      isInitialLoading={isInitialLoading}
      listRoutePath={getProductCategoryListRoutePath()}
      renderItem={(item) => item.name}
    />
  );
}