import React from "react";
import { Link } from "react-router";
import { useTsxHelper } from "@/helpers";
import styles from "./TableBlock.module.css";

// Child component.
import TableRow from "./TableRow";
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
      bodyClassName="overscroll-x-none h-fit overflow-x-auto w-full max-w-full"
      title="Danh sách kết quả"
      headerChildren={(
        <Link className="button small gap-1 shrink-0" to={props.model.createRoute}>
          <PlusIcon className="size-4.5" />
          <span>Tạo mới</span>
        </Link>
      )}
    >
      {props.model.items.length ? (
        <table className={joinClassName("border-collapse min-w-max w-full", styles.tableBlock)}>
          <thead className="whitespace-nowrap">
            <tr className={joinClassName(
              "text-black/50 dark:text-white/50 font-bold"
            )}>
              <th className="px-3 py-2">Họ và tên</th>
              <th className="px-3 py-2">Biệt danh</th>
              <th className="px-3 py-2">Giới tính</th>
              <th className="px-3 py-2">Số điện thoại</th>
              <th className="px-3 py-2">Ngày sinh</th>
              <th className="px-3 py-2">Nợ còn lại</th>
            </tr>
          </thead>
          <tbody>
            {props.model.items.map((customer, index) => (
              < TableRow model={customer} key={index}/>
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