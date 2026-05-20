import React, { useState } from "react";
import { useLoaderData } from "react-router";

// Child components.
import { MainContainer } from "@/components/layouts";

// Component.
export default function OrderListPage(): React.ReactNode {
  // Dependencies.
  const initialModel = useLoaderData<OrderListModel>();

  // States.
  const [model, _] = useState(() => initialModel);

  // Template.
  return (
    <MainContainer>
      <div className="panel">
        <div className="panel-header">
          <span className="panel-header-title">
            Danh sách đơn hàng
          </span>
        </div>

        <div className="panel-body p-3">
          <pre>{JSON.stringify(model, null, 2)}</pre>
        </div>
      </div>
    </MainContainer>
  );
}
