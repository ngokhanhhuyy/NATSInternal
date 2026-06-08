import React from "react";

// Props.
type Props = {
  model: ProductDetailModel;
};

export default function PhotoPanel(props: Props): React.ReactNode {
  // Template.
  return (
    <div className="panel flex-1">
      <div className="panel-header">
        <span className="panel-header-title">
          Hình ảnh
        </span>
      </div>

      <div className="panel-body">
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
      </div>
    </div>
  );
}
