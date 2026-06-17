import { createOrderBasicModel } from "../shared/orderBasicModel";
import { createStatsMonthYearModel } from "../shared/statsMonthYearModel";
import { metadata } from "@/metadata";
import { getOrderCreateRoutePath } from "@/helpers";

declare global {
  type OrderListModel = Implements<IHasStatsListModel<OrderBasicModel> & IUpsertableListModel<OrderBasicModel>, {
    sortByAscending: boolean;
    sortByFieldName: string;
    sortByFieldNameOptions: string[];
    page: number;
    resultsPerPage: number;
    type: OrderType | null;
    customer: CustomerBasicModel | null;
    product: ProductBasicModel | null;
    debtOrdersOnly: boolean;
    statsMonthYear: StatsMonthYearModel | null;
    statsMonthYearOptions: StatsMonthYearModel[];
    items: OrderBasicModel[];
    pageCount: number;
    itemCount: number;
    createRoutePath: string;
    mapFromResponseDto(responseDto: OrderListResponseDto): OrderListModel;
    toRequestDto(): OrderListRequestDto;
  }>;
}

export function createOrderListModel(): OrderListModel {
  const statsMonthYearOptions = metadata.statsMonthYearSeries.orderSeries.map(createStatsMonthYearModel);

  return {
    sortByAscending: metadata.listOptionsList.order.defaultSortByAscending,
    sortByFieldName: metadata.listOptionsList.order.defaultSortByFieldName,
    sortByFieldNameOptions: metadata.listOptionsList.order.sortByFieldNameOptions,
    page: 1,
    resultsPerPage: metadata.listOptionsList.order.defaultResultsPerPage,
    type: null,
    customer: null,
    product: null,
    debtOrdersOnly: false,
    statsMonthYear: statsMonthYearOptions.length ? statsMonthYearOptions[statsMonthYearOptions.length - 1] : null,
    statsMonthYearOptions,
    items: [],
    pageCount: 0,
    itemCount: 0,
    createRoutePath: getOrderCreateRoutePath(),
    mapFromResponseDto(responseDto: OrderListResponseDto): OrderListModel {
      return {
        ...this,
        items: responseDto.items.map(createOrderBasicModel),
        pageCount: responseDto.pageCount,
        itemCount: responseDto.itemCount
      };
    },
    toRequestDto(): OrderListRequestDto {
      return {
        sortByAscending: this.sortByAscending,
        sortByFieldName: this.sortByFieldName,
        page: this.page,
        resultsPerPage: this.resultsPerPage,
        type: this.type ?? undefined,
        customerId: this.customer?.id,
        productId: this.product?.id,
        debtOrdersOnly: this.debtOrdersOnly,
        statsYear: this.statsMonthYear?.year,
        statsMonth: this.statsMonthYear?.month
      };
    }
  };
}
