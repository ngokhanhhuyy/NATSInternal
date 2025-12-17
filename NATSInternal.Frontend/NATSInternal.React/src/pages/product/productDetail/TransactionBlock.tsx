import React from "react";
import { Link } from "react-router";
import { useDateTimeHelper } from "@/helpers";

// Child components.
import { Block } from "@/components/ui";

// Components.
export default function TransactionBlock(): React.ReactNode {
  // Template.
  return (
    <Block title="Giao dịch" className="flex-1">
      <table className="table-fixed w-full m-0 p-0">
        <thead>
          <tr className="border-b border-black/15 dark:border-white/15 text-black/50 dark:text-white/50 text-sm">
            <th className="border-black/15 dark:border-white/15 px-3 py-0.5">Loại</th>
            <th className="border-black/15 dark:border-white/15 px-3 py-0.5 w-25">Số lượng</th>
            <th className="px-3 py-0.5">Thời gian</th>
          </tr>
        </thead>
        <tbody>
          {model.map((transaction, index) => (
            <tr
              className="odd:bg-black/3 odd:dark:bg-white/3 not-last:border-b border-black/15 dark:border-white/15"
              key={index}
            >
              <td className="border-black/15 dark:border-white/15 px-3 py-1.5">
                <Link to="#" className="font-bold">{transaction.type}</Link>
              </td>
              <td className="border-black/15 dark:border-white/15 px-3 py-1.5">{transaction.quantity}</td>
              <td className="px-3 py-1.5">{transaction.dateTime}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </Block>
  );
}

const { getDeltaTextRelativeToNow } = useDateTimeHelper();
const model = Array.from({ length: 3 }).map((_, index) => ({
  type: getTransactionType(),
  quantity: Math.round(Math.random() * 100) + 20,
  dateTime: getDeltaTextRelativeToNow(getDateTimeISOString(index))
}));

function getTransactionType(): string {
  const random = Math.random();
  if (random < (1 / 4)) {
    return "Nhập hàng";
  }

  if (random < (2 / 4)) {
    return "Bán lẻ";
  }

  return "Liệu trình";
}

function getDateTimeISOString(index: number): string {
  const dateTime = new Date();
  dateTime.setHours(dateTime.getHours() + 6);
  dateTime.setDate(new Date().getDate() - index);
  return dateTime.toISOString();
}