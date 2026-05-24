import React from "react";

// Props.
type RecentTransactionPanelProps = { model: CustomerDetailModel };

// Component.
export default function RecentTransactionPanel(_: RecentTransactionPanelProps): React.ReactNode {
  // Template.
  return (
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">
          Giao dịch gần nhất
        </span>
      </div>

      <div className="panel-body flex justify-center items-center p-5">
      <span className="opacity-50">Không có giao dịch nào</span>
      </div>
    </div>
  );
}
