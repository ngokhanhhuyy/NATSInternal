import { createSupplyBasicModel } from "../shared/supplyBasicModel";
import { createStatsMonthYearModel } from "../shared/statsMonthYearModel";
import { metadata } from "@/metadata";
import { getOrderCreateRoutePath } from "@/helpers";

declare global {
  type SupplyListModel = Implements<IHasStatsListModel<SupplyBasicModel> & IUpsertableListModel<SupplyBasicModel>, {
    sortByAscending: boolean;
    sortByFieldName: string;
    sortByFieldNameOptions: string[];
    page: number;
    resultsPerPage: number;
    product: ProductBasicModel | null;
    statsMonthYear: StatsMonthYearModel | null;
    statsMonthYearOptions: StatsMonthYearModel[];
    items: SupplyBasicModel[];
    pageCount: number;
    itemCount: number;
    createRoutePath: string;
    mapFromResponseDto(responseDto: SupplyListResponseDto): SupplyListModel;
    toRequestDto(): SupplyListRequestDto;
  }>;
}

export function createSupplyListModel(): SupplyListModel {
  const statsMonthYearOptions = metadata.statsMonthYearSeries.supplySeries.map(createStatsMonthYearModel);

  return {
    sortByAscending: metadata.listOptionsList.supply.defaultSortByAscending,
    sortByFieldName: metadata.listOptionsList.supply.defaultSortByFieldName,
    sortByFieldNameOptions: metadata.listOptionsList.supply.sortByFieldNameOptions,
    page: 1,
    resultsPerPage: metadata.listOptionsList.supply.defaultResultsPerPage,
    product: null,
    statsMonthYear: statsMonthYearOptions.length ? statsMonthYearOptions[statsMonthYearOptions.length - 1] : null,
    statsMonthYearOptions,
    items: [],
    pageCount: 0,
    itemCount: 0,
    createRoutePath: getOrderCreateRoutePath(),
    mapFromResponseDto(responseDto: SupplyListResponseDto): SupplyListModel {
      return {
        ...this,
        items: responseDto.items.map(createSupplyBasicModel),
        pageCount: responseDto.pageCount,
        itemCount: responseDto.itemCount
      };
    },
    toRequestDto(): SupplyListRequestDto {
      return {
        sortByAscending: this.sortByAscending,
        sortByFieldName: this.sortByFieldName,
        page: this.page,
        resultsPerPage: this.resultsPerPage,
        productId: this.product?.id,
        statsYear: this.statsMonthYear?.year,
        statsMonth: this.statsMonthYear?.month
      };
    }
  };
}
