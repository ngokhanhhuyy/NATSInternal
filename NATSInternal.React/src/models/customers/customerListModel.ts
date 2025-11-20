import { createCustomerBasicModel } from "@/models";
import { getMetadata } from "@/metadata";

declare global {
  type CustomerListModel = {
    sortByAscending: boolean;
    sortByFieldName: string;
    page: number;
    resultsPerPage: number | null;
    searchContent: string;
    items: CustomerBasicModel[];
    pageCount: number;
    mapFromResponseDto(responseDto: CustomerGetListResponseDto): CustomerListModel;
    toRequestDto(): CustomerGetListRequestDto;
  };
}

const customerListOptions = getMetadata().listOptionsList.customer;

export function createCustomerListModel(responseDto: CustomerGetListResponseDto): CustomerListModel {
  const model: CustomerListModel = {
    sortByAscending: customerListOptions.defaultSortByAscending ?? true,
    sortByFieldName: customerListOptions.defaultSortByFieldName ?? "",
    page: 1,
    resultsPerPage:  customerListOptions.defaultResultsPerPage,
    searchContent: "",
    items: [],
    pageCount: 0,
    mapFromResponseDto(responseDto: CustomerGetListResponseDto): CustomerListModel {
      return {
        ...this,
        items: responseDto.items.map(item => createCustomerBasicModel(item)),
        pageCount: responseDto.pageCount
      };
    },
    toRequestDto(): CustomerGetListRequestDto {
      const requestDto: CustomerGetListRequestDto = {
        sortByAscending: this.sortByAscending,
        sortByFieldName: this.sortByFieldName,
        page: this.page,
      };

      if (this.resultsPerPage) {
        requestDto.resultsPerPage = this.resultsPerPage;
      }

      if (this.searchContent) {
        requestDto.searchContent = this.searchContent;
      }

      return requestDto;
    }
  };

  return model.mapFromResponseDto(responseDto);
}