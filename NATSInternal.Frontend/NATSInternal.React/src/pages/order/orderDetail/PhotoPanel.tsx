import React from "react";

// Child components.
import { NewTabLink } from "@/components/ui";

// Props.
type Props = {
  model: PhotoBasicModel[];
};

// Components.
export default function PhotoPanel(props: Props): React.ReactNode {
  // Template.
  return (
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">
          Hình ảnh
        </span>
      </div>

      <div className="panel-body p-3 flex flex-wrap gap-2">
        {props.model.length ? props.model.map(photo => (
          <NewTabLink url={photo.url} key={photo.id}>
            <img className="img-thumbnail size-16" src={photo.url}/>
          </NewTabLink>
        )) : (
          <div className="flex p-10 justify-center items-center opacity-50 flex-1 h-full">
            Không có hình ảnh
          </div>
        )}
      </div>
    </div>
  );
}
