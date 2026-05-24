import React from "react";
import { useLoaderData, Link } from "react-router";
import { compute } from "@/helpers";

// Child components.
import CustomerPanel from "./CustomerPanel";
import ManagementPanel from "./ManagementPanel";
import NotePanel from "./NotePanel";
import ItemListPanel from "./ItemListPanel";
import PhotoPanel from "./PhotoPanel";
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
      {/* Name panel */}
      <div className="panel">
        <div className="panel-body border-t rounded-xl p-3">
          <div className="grid grid-cols-[auto_1fr] gap-3">
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
              <span className="text-blue-600 dark:text-blue-400 font-bold text-xl">
                {model.displayName}
              </span>

              <span className="opacity-50">
                {model.displayStatsDate}
              </span>
            </div>
          </div>
        </div>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
        <div className="flex flex-col gap-3">
          <CustomerPanel model={model.customer} />
          <ManagementPanel model={model} />
        </div>

        <NotePanel model={model.note} />
      </div>
      
      <ItemListPanel model={model} />
      <PhotoPanel model={model.photos} />
      
      <div className="flex justify-end">
        <Link className="btn gap-1.5" to={model.updateRoutePath}>
          <PencilSquareIcon className="size-4" />
          <span>Chỉnh sửa</span>
        </Link>
      </div>
    </MainContainer>
  );
}
