import React from "react";
import { useTsxHelper } from "@/helpers";

// Child components.
import { Button, NewTabLink, MainPaginator } from "@/components/ui";
import { CheckIcon } from "@heroicons/react/24/solid";

// Props.
type ListProps = {
  model: CustomerListModel;
  onModelChanged(changedData: Partial<CustomerListModel>): any;
  onPicked(customer: CustomerListCustomerModel): any;
  isReloading: boolean;
} & React.ComponentPropsWithoutRef<"div">;

// Component.
export default function List(props: ListProps): React.ReactNode {
  const { model, onModelChanged, onPicked, isReloading, ...domProps } = props;
  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // Template.
  return (
    <div
      {...domProps}
      className={joinClassName(
        "flex flex-col justify-start items-stretch transition-opacity",
        isReloading && "opacity-50 pointer-events-none"
      )}
    >
      {/* List */}
      <ul className="list-group mb-3">
        {model.items.length ? model.items.map((customer, index) => (
          <li className="list-group-item flex justify-between items-center px-3 py-2" key={index}>
            <div className="flex flex-col">
              <NewTabLink className="font-bold" href={customer.detailRoute}>
                {customer.fullName}
              </NewTabLink>
              <span className="text-sm opacity-75">
                {customer.nickName}
              </span>
            </div>
            <Button className="small aspect-square" onClick={() => onPicked(customer)}>
              <CheckIcon className="size-4.5" />
            </Button>
          </li>
        )) : (
          <li className="list-group-item py-5">
            <span className="opacity-50">Không có kết quả</span>
          </li>
        )}
      </ul>

      {/* Paginator */}
      <MainPaginator
        page={model.page}
        pageCount={model.pageCount}
        onPageChanged={page => onModelChanged({ page })}
        isReloading={isReloading}
      />
    </div>
  );
}