import React from "react";
import { Link } from "react-router";
import { useTsxHelper } from "@/helpers";

// Child component.
import Row from "./Row";
import { Block } from "@/components/ui";
import { PlusIcon } from "@heroicons/react/24/outline";

// Props.
type TableBlockProps = { model: CustomerListModel };

// Component.
export default function TableBlockProps(props: TableBlockProps): React.ReactNode {
  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // Template.
  return (
    <Block
      className="mb-3"
      bodyClassName="overscroll-x-none h-fit overflow-x-auto w-full max-w-full"
      title="Danh sách kết quả"
      headerChildren={(
        <Link className="button gap-1 shrink-0" to={props.model.createRoute}>
          <PlusIcon className="size-4.5" />
          <span>Tạo mới</span>
        </Link>
      )}
    >
      {props.model.items.length ? (
        <table className="border-collapse min-w-max w-full">
          <thead className="whitespace-nowrap">
            <tr className={joinClassName(
              "border-b border-black/15 dark:border-white/15",
              "text-black/50 dark:text-white/50 font-bold"
            )}>
              <td className="px-3 py-2">Họ và tên</td>
              <td className="px-3 py-2">Biệt danh</td>
              <td className="px-3 py-2">Giới tính</td>
              <td className="px-3 py-2">Số điện thoại</td>
              <td className="px-3 py-2">Ngày sinh</td>
              <td className="px-3 py-2">Nợ còn lại</td>
              <td/>
            </tr>
          </thead>
          <tbody>
            {props.model.items.map((customer, index) => (
              < Row model={customer} key={index}/>
            ))}
          </tbody>
        </table>
      ) : (
        <div className="flex justify-center items-center w-full p-8 opacity-50">
          Không có kết quả
        </div>
      )}
    </Block>
  );
}