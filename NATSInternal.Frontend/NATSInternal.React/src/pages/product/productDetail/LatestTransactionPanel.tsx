import React from "react";
import { Link } from "react-router";
import { useDateTimeHelper, useCurrencyHelper } from "@/helpers";

// Components.
export default function LatestTransactionPanel(): React.ReactNode {
  // Dependencies.
  const { getAmountDisplayText } = useCurrencyHelper();

  // Template.
  return (
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">
          Giao dịch gần nhất
        </span>
      </div>

      <div className="panel-body relative">
        <table className="data-table">
          <thead>
            <tr>
              <th>Loại</th>
              <th>Số lượng</th>
              <th>Tổng giá trị sản phẩm</th>
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
                <td className="text-center">{getAmountDisplayText(transaction.totalAmount)}</td>
                <td className="text-end">{transaction.dateTime}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

type Model = {
  type: string;
  quantity: number;
  totalAmount: number;
  dateTime: string;
};

const { getDeltaTextRelativeToNow } = useDateTimeHelper();
const model: Model[] = Array.from({ length: 2 }).map((_, index) => ({
  type: getTransactionType(),
  quantity: Math.round(Math.random() * 100) + 20,
  totalAmount: Math.round(Math.random() * 1000) * 1000,
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
  dateTime.setHours(dateTime.getHours() + 6);
  return getDeltaTextRelativeToNow(dateTime.toISOString());
}