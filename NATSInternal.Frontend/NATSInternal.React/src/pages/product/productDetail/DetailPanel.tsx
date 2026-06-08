import React from "react";
import { Link } from "react-router";
import { compute } from "@/helpers";

// Child components.
import { Field } from "@/pages/shared/detail";
import { TagIcon, ExclamationTriangleIcon } from "@heroicons/react/24/outline";

// Props.
type DetailPanelProps = {
  model: ProductDetailModel;
};

// Components.
export default function DetailPanel(props: DetailPanelProps): React.ReactNode {
  // Computed.
  const isResupplyNeeded = compute<boolean>(() => {
    return !props.model.isDiscontinued && props.model.stockingQuantity <= (props.model.resupplyThresholdQuantity ?? 0);
  });
  
  // Template.
  function renderUser(user: UserBasicModel): React.ReactNode {
    if (user.isDeleted) {
      return (
        <span className="line-through">
          Đã xoá
        </span>
      );
    }

    return (
      <Link to={props.model.createdUser.detailRoute}>
        @{props.model.createdUser.userName}
      </Link>
    );
  }

  return (
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">
          Chi tiết sản phẩm
        </span>
      </div>

      <div className="panel-body grid grid-cols-1 lg:grid-cols-2 gap-3 p-3 items-start">
        <div className="panel-body-area flex flex-col gap-y-3 p-3 h-full">
          {/* Description */}
          {props.model.description && (
            <Field name="description">
              {props.model.description}
            </Field>
          )}

          {/* DefaultAmountBeforeVatPerUnit */}
          <Field name="defaultAmountBeforeVatPerUnit">
            {props.model.formattedDefaultAmountBeforeVatPerUnit}
          </Field>

          {/* DefaultVatPercentagePerUnit */}
          <Field name="defaultVatPercentagePerUnit">
            {props.model.defaultVatPercentagePerUnit}%
          </Field>

          {/* Categories */}
          {props.model.categories.length > 0 && (
            <Field name="category">
              <div className="flex flex-wrap gap-1">
                {props.model.categories.map(category => (
                  <Link
                    className="alert alert-neutral-outline dark:alert-neutral alert-sm gap-1"
                    to={category.detailRoutePath}
                    key={category.id}
                  >
                    <TagIcon className="size-3.5" />
                    <span>{category.name}</span>
                  </Link>
                ))}
              </div>
            </Field>
          )}
        </div>

        <div className="flex flex-col gap-3">
          <div className="panel-body-area flex flex-col gap-y-3 p-3 pb-2">
            {/* CreatedUser */}
            <Field name="createdUser">
              {renderUser(props.model.createdUser)}
            </Field>

            {/* CreatedDateTime */}
            <Field name="createdDateTime">
              {props.model.createdDateTime}
            </Field>

            {/* LastUpdatedUser */}
            {props.model.lastUpdatedUser && (
              <Field name="lastUpdatedUser">
                {renderUser(props.model.lastUpdatedUser)}
              </Field>
            )}

            {/* LastUpdatedDateTime */}
            {props.model.lastUpdatedDateTime && (
              <Field name="lastUpdatedDateTime">
                {props.model.lastUpdatedDateTime}
              </Field>
            )}

            {/* DeletedUser */}
            {props.model.deletedUser && (
              <Field name="deletedUser">
                {renderUser(props.model.deletedUser)}
              </Field>
            )}

            {/* DeletedDateTime */}
            {props.model.deletedDateTime && (
              <Field name="deletedDateTime">
                {props.model.deletedDateTime}
              </Field>
            )}
          </div>

          <div className="panel-body-area flex flex-col gap-y-3 p-3 pb-2 h-full">
            {/* StockingQuantity */}
            <Field name="stockingQuantity" className="flex gap-3">
              {props.model.stockingQuantity}
              {isResupplyNeeded && (
                <div className="flex items-center gap-1 text-yellow-600 dark:text-yellow-400 text-sm">
                  <ExclamationTriangleIcon className="size-4.5" />
                  <span>Cần nhập hàng</span>
                </div>
              )}
            </Field>

            {/* ResupplyStockingQuantity */}
            <Field name="resupplyThresholdQuantity">
              {props.model.resupplyThresholdQuantity ?? 0}
            </Field>
          </div>
        </div>
      </div>
    </div>
  );
}
