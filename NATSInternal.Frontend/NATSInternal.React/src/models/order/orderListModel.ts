import { createOrderBasicModel } from "../shared/orderBasicModel";
import { createStatsMonthYearModel } from "../shared/statsMonthYearModel";
import { metadata } from "@/metadata";

declare global {
  type OrderListModel = Implements<IHasStatsListModel<OrderBasicModel>, {
    sortByAscending: boolean;
    sortByFieldName: string;
    sortByFieldNameOptions: string[];
    page: number;
    resultsPerPage: number;
    customer: CustomerBasicModel | null;
    debtOrdersOnly: boolean;
    statsMonthYear: StatsMonthYearModel | null;
    statsMonthYearOptions: StatsMonthYearModel[];
    items: OrderBasicModel[];
    pageCount: number;
    itemCount: number;
    mapFromResponseDto(responseDto: OrderListResponseDto): OrderListModel;
    toRequestDto(): OrderListRequestDto;
  }>;
}

export function createOrderListModel(): OrderListModel {
  return {
    sortByAscending: metadata.listOptionsList.order.defaultSortByAscending,
    sortByFieldName: metadata.listOptionsList.order.defaultSortByFieldName,
    sortByFieldNameOptions: metadata.listOptionsList.order.sortByFieldNameOptions,
    page: 1,
    resultsPerPage: metadata.listOptionsList.order.defaultResultsPerPage,
    customer: null,
    debtOrdersOnly: false,
    statsMonthYear: null,
    statsMonthYearOptions: metadata.statsMonthYearSeries.orderSeries.map(createStatsMonthYearModel),
    items: [],
    pageCount: 0,
    itemCount: 0,
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
        customerId: this.customer?.id,
        debtOrdersOnly: this.debtOrdersOnly,
        statsYear: this.statsMonthYear?.year,
        statsMonth: this.statsMonthYear?.month
      };
    }
  };
}
