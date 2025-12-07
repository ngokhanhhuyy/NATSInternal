import React from "react";

// Child components.
import { Block } from "@/components/ui";

// Props.
type RecentTransactionsBlockProps = { model: CustomerDetailModel };

// Component.
export default function RecentTransactionsBlock(_: RecentTransactionsBlockProps): React.ReactNode {
  // Template.
  return (
    <Block title="Giao dịch gần nhất" className="h-full" bodyClassName="flex justify-center items-center p-5">
      <span className="opacity-50">Không có giao dịch nào</span>
    </Block>
  );
}