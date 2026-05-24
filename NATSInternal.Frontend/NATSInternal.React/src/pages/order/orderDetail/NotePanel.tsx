import React from "react";

// Props.
type NotePanelProps = {
  model: string | null;
};

// Components.
export default function NotePanel(props: NotePanelProps): React.ReactNode {
  // Template.
  return (
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">
          Ghi chú
        </span>
      </div>

      <div className="panel-body p-3 pt-2">
        {props.model ? (
          <div className="mx-2">
            {props.model}
          </div>
        ) : (
          <div className="flex flex-1 justify-center items-center opacity-50 h-full">
            Không có ghi chú
          </div>
        )}
      </div>
    </div>
  );
}
