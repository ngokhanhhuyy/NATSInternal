import React from "react";

// Child components.
import { Block } from "@/components/ui";

// Props.
type Props = {
  model: ProductDetailModel;
};

export default function PhotoBlock(props: Props): React.ReactNode {
  // Template.
  return (
    <Block title="Hình ảnh">
      {props.model.photos.length > 0 ? (
        <div className="flex flex-wrap gap-3 p-3">
          {props.model.photos.map((photo, index) => (
            <img
              className="img-thumbnail max-w-30 aspect-square object-cover"
              src={photo.url}
              alt={props.model.name}
              key={index}
            />
          ))}
        </div>
      ) : (
        <div className="flex justify-center items-center px-3 py-5 opacity-25">
          Không có ảnh
        </div>
      )}
    </Block>
  );
}