import React, { useState, useEffect } from "react";
import { Link } from "react-router";
import { useApi } from "@/api";
import { createBrandListModel, createProductCategoryListModel } from "@/models";
import { getDisplayName } from "@/metadata";
import { useRouteHelper, useTsxHelper } from "@/helpers";
import styles from "./SecondaryPanels.module.css";

// Child components.
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
function SecondaryListPanel<
      TListModel extends
        ISearchableListModel<TItemModel> &
        ISortableListModel<TItemModel> &
        IPageableListModel<TItemModel> &
        IUpsertableListModel<TItemModel>,
      TItemModel extends BrandListBrandModel | ProductCategoryListProductCategoryModel>
  (props: Props<TListModel, TItemModel>): React.ReactNode
{
  // Dependencies.
  const { compute, joinClassName } = useTsxHelper();
  
  // Computed.
  const displayName = getDisplayName(props.resourceName) ?? props.resourceName;
    
  // Template.
  const list = compute<React.ReactNode>(() => {
    if (props.isInitialLoading) {
      return (
        <div className="flex justify-center items-center px-3 py-5">
          Đang tải..
        </div>
      );
    }

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
            <Link to={props.listRoutePath}>
              {`...và ${props.model.itemCount - props.model.items.length} ${displayName.toLowerCase()} khác`}
            </Link>
          </li>
        )}
      </ul>
    );
  });

  return (
    <div className={joinClassName("panel", styles.fadeInAnimation)}>
      <div className="panel-header">
        <div className="panel-header-title">
          {displayName}
        </div>

        <Link to={props.listRoutePath} className="btn btn-panel-header btn-sm gap-1">
          <Bars4Icon className="size-4"/>
          <span>Danh sách đầy đủ</span>
        </Link>
      </div>

      <div className="panel-body">
        {list}
      </div>
    </div>
  );
}

export function BrandListPanel(): React.ReactNode {
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
    <SecondaryListPanel
      model={model}
      resourceName="brand"
      isInitialLoading={isInitialLoading}
      listRoutePath={getBrandListRoutePath()}
      renderItem={(item: BrandListBrandModel) => (
        <Link to={item.detailRoute} className="text-blue-700 dark:text-blue-400 font-bold">
          {item.name}
        </Link>
      )}
    />
  );
}

export function ProductCategoryListPanel(): React.ReactNode {
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
    <SecondaryListPanel
      model={model}
      resourceName="productCategory"
      isInitialLoading={isInitialLoading}
      listRoutePath={getProductCategoryListRoutePath()}
      renderItem={(item) => <span className="text-blue-700 dark:text-blue-400">{item.name}</span>}
    />
  );
}