import React from "react";
import { Link } from "react-router";
import { useDateTimeHelper } from "@/helpers";

// Child components.
import { Block } from "@/components/ui";

// Components.
export default function LatestTransactionBlock(): React.ReactNode {
  // Computed.
  // const { compute } = useTsxHelper();

  // Computed.
  // const computedModel = compute<(Model | null)[]>(() => {
  //   return Array.from({ length: 30 }).map((_, index) => index < model.length ? model[index] : null);
  // });

  // Template.
  return (
    <Block title="Giao dịch gần nhất" bodyClassName="relative">
      <table className="table table-row-height-30 table-extra-empty-rows-as-background relative lg:absolute top-0">
        <thead>
          <tr>
            <th>Loại</th>
            <th>Số lượng</th>
            <th>Thời gian</th>
          </tr>
        </thead>
        <tbody>
          {model.map((transaction, index) => (
            <tr key={index}>
              <td>
                <Link to="#" className="font-bold">
                  {transaction?.type}
                </Link>
              </td>
              <td className="text-center">{transaction?.quantity}</td>
              <td className="text-end">{transaction?.dateTime}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </Block>
  );
}

type Model = {
  type: string,
  quantity: number,
  dateTime: string
};

const { getDeltaTextRelativeToNow } = useDateTimeHelper();
const model: Model[] = Array.from({ length: 2 }).map((_, index) => ({
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
  dateTime.setHours(dateTime.getHours() + 6);
  return getDeltaTextRelativeToNow(dateTime.toISOString());
}