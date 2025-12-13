import React, { useMemo } from "react";
import { Link } from "react-router";
import { getDisplayName } from "@/metadata";

// Child components.
import { Block } from "@/components/ui";

// Props.
type Props<TBasicModel extends BrandBasicModel | ProductCategoryBasicModel> = {
  model: TBasicModel[];
  resourceName: string;
  renderItem(item: TBasicModel): React.ReactNode;
};

// Component.
export default function SecondaryListBlock
      <TBasicModel extends BrandBasicModel | ProductCategoryBasicModel>
    (props: Props<TBasicModel>): React.ReactNode {
  // Computed.
  const slicedModel = useMemo<TBasicModel[]>(() => {
    return props.model.slice(0, 5);
  }, [props.model]);
    
  // Template.
  return (
    <Block title={getDisplayName(props.resourceName) ?? props.resourceName}>
      {slicedModel.length > 0 ? (
        <ul className="list-group list-group-flush">
          {slicedModel.map((item, index) => (
            <li className="px-3 py-2" key={index}>
              {props.renderItem(item)}
            </li>
          ))}

          {slicedModel.length < props.model.length && (
            <li className="px-3 py-2 text-center opacity-50">
              {`+${props.model.length - slicedModel.length} ${getDisplayName(props.resourceName)?.toLowerCase()} khác...`}
            </li>
          )}
        </ul>
      ) : (<></>)}
    </Block>
  );
}

export function BrandListBlock(props: { model: BrandBasicModel[] }): React.ReactNode {
  // Template.
  return (
    <SecondaryListBlock
      model={props.model}
      resourceName="brand"
      renderItem={(item) => (
        <Link to={item.detailRoute} className="font-bold">
          {item.name}
        </Link>
      )}
    />
  );
}

export function ProductCategoryListBlock(props: { model: ProductCategoryBasicModel[] }): React.ReactNode {
  // Template.
  return (
    <SecondaryListBlock
      model={props.model}
      resourceName="productCategory"
      renderItem={(item) => item.name}
    />
  );
}