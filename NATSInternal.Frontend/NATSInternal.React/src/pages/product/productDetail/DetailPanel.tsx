import React, { useMemo } from "react";
import { Link } from "react-router";

// Child components.
import Field from "@/pages/product/productDetail/Field";
import { ArchiveBoxIcon } from "@heroicons/react/24/outline";

// Props.
type Props = {
  model: ProductDetailModel;
};

// Components.
export default function DetailPanel(props: Props): React.ReactNode {
  // Computed.
  const thumbnailUrl = useMemo<string | null>(() => {
    return props.model.photos.filter(p => p.isThumbnail).map(p => p.url)[0] ?? null;
  }, [props.model.photos]);

  // Template.
  return (
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">
          Chi tiết sản phẩm
        </span>
      </div>

      <div className="panel-body flex flex-col gap-2 px-3 pt-3 pb-2">
        <div className="flex gap-3 justify-start items-start">
          {thumbnailUrl ? (
            <img className="img-thumbnail size-14" src={thumbnailUrl} alt={props.model.name} />
          ) : (
            <div className="img-thumbnail size-14 flex justify-center items-center">
              <ArchiveBoxIcon className="size-7 opacity-50" />
            </div>
          )}

          <div className="flex flex-col justify-start items-start">
            <div className="text-2xl text-blue-700 dark:text-blue-400">
              {props.model.name}
            </div>
            <div className="flex flex-wrap gap-2 mb-3">
              <TargetTransactionTypeAlert isForRetail={props.model.isForRetail}  />
              <StatusAlert isDiscontinued={props.model.isDiscontinued} stock={props.model.stock} />
            </div>
          </div>
        </div>

        <div className="flex flex-col gap-y-3">
          {/* Description */}
          {props.model.description && (
            <Field propertyName="description">
              {props.model.description}
            </Field>
          )}

          {/* DefaultAmountBeforeVatPerUnit */}
          <Field propertyName="defaultAmountBeforeVatPerUnit">
            {props.model.formattedDefaultAmountBeforeVatPerUnit}
          </Field>

          {/* DefaultVatPercentagePerUnit */}
          <Field propertyName="defaultVatPercentagePerUnit">
            {props.model.defaultVatPercentagePerUnit}%
          </Field>

          {/* Brand */}
          {props.model.brand && (
            <Field propertyName="brand">
              <Link to={props.model.brand.detailRoute}>
                {props.model.brand.name}
              </Link>
            </Field>
          )}

          {/* Category */}
          {props.model.category && (
            <Field propertyName="category">
              <Link to={props.model.category.detailRoutePath}>
                {props.model.category.name}
              </Link>
            </Field>
          )}
        </div>
      </div>
    </div>
  );
}

function TargetTransactionTypeAlert(props: { isForRetail: boolean }): React.ReactNode {
  if (props.isForRetail) {
    return (
      <div className="alert alert-emerald-outline dark:alert-emerald dark:font-bold alert-sm">
        Cả liệu trình và bán lẻ
      </div>
    );
  }

  return (
    <div className="alert alert-blue-outline dark:alert-blue dark:font-bold alert-sm">
      Chỉ liệu trình
    </div>
  );
};


 function StatusAlert(props: { isDiscontinued: boolean; stock: StockBasicModel | null }): React.ReactNode {
  if (props.isDiscontinued) {
    return (
      <div className="alert alert-neutral-outline dark:alert-neutral dark:font-bold alert-sm">
        Đã ngưng kinh doanh
      </div>
    );
  }
  
  if (props.stock != null) {
    if (props.stock.stockingQuantity === 0) {
      return (
        <div className="alert alert-red-outline dark:alert-red dark:font-bold alert-sm">
          Đã hết hàng
        </div>
      );
    }

    if (props.stock.stockingQuantity <= props.stock.resupplyThresholdQuantity) {
      return (
        <div className="alert alert-yellow-outline dark:alert-yellow dark:font-bold alert-sm">
          Sắp hết hàng
        </div>
      );
    }
  }
};