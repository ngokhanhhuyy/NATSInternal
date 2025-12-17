import React from "react";
import { Link } from "react-router";

// Child components.
import { Block } from "@/components/ui";

// Components.
export default function LatestTransactionBlock(): React.ReactNode {
  // Template.
  return (
    <Block title="Giao dịch gần nhất">
      <table className="table">
        <thead>
          <tr>
            <th>Loại</th>
            <th className="w-25">Số lượng</th>
            <th>Thời gian</th>
          </tr>
        </thead>
        <tbody>
          {model.map((transaction, index) => (
            <tr key={index}>
              <td>
                <Link to="#" className="font-bold">
                  {transaction.type}
                </Link>
              </td>
              <td className="text-center">{transaction.quantity}</td>
              <td className="text-end">{transaction.dateTime}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </Block>
  );
}

const model = Array.from({ length: 10 }).map((_, index) => ({
  type: getTransactionType(),
  quantity: Math.round(Math.random() * 100) + 20,
  dateTime: getDateTime(index)
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

function getDateTime(index: number): string {
  const dateTime = new Date();
  dateTime.setDate(new Date().getDate() - index);
  return dateTime.toLocaleString("vi-VN");
}