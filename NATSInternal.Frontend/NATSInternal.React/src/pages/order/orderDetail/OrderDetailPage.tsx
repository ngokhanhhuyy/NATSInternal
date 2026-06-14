import React from "react";
import { useLoaderData, Link } from "react-router";
import { joinClassName, compute, getTextClassNameBasedOnDebtAmount } from "@/helpers";

// Child components.
import DetailPanel from "./DetailPanel";
import ItemListPanel from "./itemListPanel";
import DebtAlert from "@/pages/shared/alerts/DebtAlert";
import { MainContainer } from "@/components/layouts";
import { ShoppingCartIcon, PencilSquareIcon } from "@heroicons/react/24/outline";

// Components.
export default function OrderDetailPage(): React.ReactNode {
  // Dependencies.
  const model = useLoaderData<OrderDetailModel>();
    // Computed.
    const thumbnailUrl = compute<string | null>(() => {
      const thumbnails = model.photos.filter(p => p.isThumbnail);
      if (thumbnails.length > 0) {
        return thumbnails[0].url;
      }
  
      return null;
    });
  

  // Template.
  return (
    <MainContainer>
      <div className="panel">
        <div className="panel-body grid grid-cols-[auto_1fr] gap-3 p-3">
          {thumbnailUrl ? (
            <img
              src={thumbnailUrl}
              className="img-thumbnail size-15"
              alt={model.displayName}
            />
          ) : (
            <div className="img-thumbnail size-15 flex justify-center items-center">
              <ShoppingCartIcon className="size-8 opacity-50" />
            </div>
          )}

          <div className="flex flex-col flex-1 justify-center align-start">
            <div className="flex flex-wrap gap-x-2 items-center">
              <span className={joinClassName(
                "font-bold text-xl",
                getTextClassNameBasedOnDebtAmount(model.debtAmount)
              )}>
                {model.displayName}
              </span>

              <DebtAlert debtAmount={model.debtAmount} />
            </div>

            <span className="opacity-50">
              {model.displayStatsDate}
            </span>
          </div>
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-3">
        <DetailPanel model={model} />
        <ItemListPanel model={model} />
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
