import React from "react";

// Child components.
import MainBlock from "./MainBlock";

// Props.
type RecentTransactionsBlockProps = { model: CustomerDetailModel };

// Component.
export default function RecentTransactionsBlock(_: RecentTransactionsBlockProps): React.ReactNode {
  // Template.
  return (
    <MainBlock title="Giao dịch gần nhất" className="h-full">
      <div className="h-full flex justify-center items-center opacity-50">
        Không có giao dịch nào
      </div>
    </MainBlock>
  );
}