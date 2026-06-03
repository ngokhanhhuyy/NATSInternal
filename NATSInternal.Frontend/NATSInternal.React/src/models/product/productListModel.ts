import { createProductBasicModel } from "@/models";
import { metadata } from "@/metadata";
import { getProductCreateRoutePath } from "@/helpers";

declare global {
  type ProductListModel = Implements<
      ISearchableListModel<ProductBasicModel> &
      IUpsertableListModel<ProductBasicModel>, {
    sortByAscending: boolean;
    sortByFieldName: string;
    page: number;
    resultsPerPage: number;
    searchContent: string;
    outOfStockProductsIncluded: boolean;
    discontinuedProductsIncluded: boolean;
    deletedProductsIncluded: boolean;
    category: ProductCategoryBasicModel | null;
    items: ProductBasicModel[];
    pageCount: number;
    itemCount: number;
    get sortByFieldNameOptions(): string[];
    get createRoutePath(): string;
    mapFromResponseDto(responseDto: ProductListResponseDto): ProductListModel;
    toRequestDto(): ProductListRequestDto;
  }>;
}

const productListOptions = metadata.listOptionsList.product;

export function createProductListModel(responseDto?: ProductListResponseDto): ProductListModel {
  const model: ProductListModel = {
    sortByAscending: productListOptions.defaultSortByAscending ?? true,
    sortByFieldName: productListOptions.defaultSortByFieldName ?? "",
    page: 1,
    resultsPerPage:  productListOptions.defaultResultsPerPage,
    searchContent: "",
    outOfStockProductsIncluded: true,
    discontinuedProductsIncluded: true,
    deletedProductsIncluded: false,
    category: null,
    items: [],
    pageCount: 0,
    itemCount: 0,
    get sortByFieldNameOptions(): string[] {
      return productListOptions.sortByFieldNameOptions;
    },
    get createRoutePath(): string {
      return getProductCreateRoutePath();
    },
    mapFromResponseDto(responseDto: ProductListResponseDto): ProductListModel {
      return {
        ...this,
        items: responseDto.items.map(createProductBasicModel),
        pageCount: responseDto.pageCount,
        itemCount: responseDto.itemCount
      };
    },
    toRequestDto(): ProductListRequestDto {
      const requestDto: ProductListRequestDto = {
        sortByAscending: this.sortByAscending,
        sortByFieldName: this.sortByFieldName,
        page: this.page,
        outOfStockProductsIncluded: this.outOfStockProductsIncluded,
        discontinuedProductsIncluded: this.discontinuedProductsIncluded,
        deletedProductsIncluded: this.deletedProductsIncluded,
        categoryId: this.category?.id
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

  if (responseDto) {
    return model.mapFromResponseDto(responseDto);
  }

  return model;
}
