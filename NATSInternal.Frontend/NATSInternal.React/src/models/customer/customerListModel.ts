import { createCustomerBasicModel } from "@/models";
import { metadata } from "@/metadata";
import { getCustomerCreateRoutePath } from "@/helpers";

declare global {
  type CustomerListModel = Implements<
      ISearchableListModel<CustomerBasicModel> &
      IUpsertableListModel<CustomerBasicModel>, {
    sortByAscending: boolean;
    sortByFieldName: string;
    page: number;
    resultsPerPage: number;
    searchContent: string;
    excludedId: number | null;
    items: CustomerBasicModel[];
    pageCount: number;
    itemCount: number;
    get sortByFieldNameOptions(): string[];
    get createRoutePath(): string;
    mapFromResponseDto(responseDto: CustomerListResponseDto): CustomerListModel;
    toRequestDto(): CustomerListRequestDto;
  }>;
}
const customerListOptions = metadata.listOptionsList.customer;

export function createCustomerListModel(responseDto?: CustomerListResponseDto): CustomerListModel {
  const model: CustomerListModel = {
    sortByAscending: customerListOptions.defaultSortByAscending ?? true,
    sortByFieldName: customerListOptions.defaultSortByFieldName ?? "",
    page: 1,
    resultsPerPage:  customerListOptions.defaultResultsPerPage,
    searchContent: "",
    excludedId: null,
    items: [],
    pageCount: 0,
    itemCount: 0,
    get sortByFieldNameOptions(): string[] {
      return customerListOptions.sortByFieldNameOptions;
    },
    get createRoutePath(): string {
      return getCustomerCreateRoutePath();
    },
    mapFromResponseDto(responseDto: CustomerListResponseDto): CustomerListModel {
      return {
        ...this,
        items: responseDto.items.map(createCustomerBasicModel),
        pageCount: responseDto.pageCount,
        itemCount: responseDto.itemCount
      };
    },
    toRequestDto(): CustomerListRequestDto {
      const requestDto: CustomerListRequestDto = {
        sortByAscending: this.sortByAscending,
        sortByFieldName: this.sortByFieldName,
        page: this.page
      };

      if (this.resultsPerPage) {
        requestDto.resultsPerPage = this.resultsPerPage;
      }

      if (this.searchContent) {
        requestDto.searchContent = this.searchContent;
      }

      if (this.excludedId) {
        requestDto.excludedId = this.excludedId;
      }

      return requestDto;
    }
  };

  if (responseDto) {
    return model.mapFromResponseDto(responseDto);
  }

  return model;
}
