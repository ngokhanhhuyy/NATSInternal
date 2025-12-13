import { getMetadata } from "@/metadata";
import { useCurrencyHelper, useRouteHelper } from "@/helpers";

declare global {
  type ProductListModel = Implements<IPageableListModel<ProductListProductModel>, {
    sortByAscending: boolean;
    sortByFieldName: string;
    page: number;
    resultsPerPage: number | null;
    searchContent: string;
    items: ProductListProductModel[];
    pageCount: number;
    get sortByFieldNameOptions(): string[];
    get createRoute(): string;
    mapFromResponseDto(responseDto: ProductGetListResponseDto): ProductListModel;
    toRequestDto(): ProductGetListRequestDto;
  }>;

  type ProductListProductModel = Readonly<{
    id: string;
    name: string;
    unit: string;
    defaultAmountBeforeVatPerUnit: number;
    defaultVatPercentagePerUnit: number;
    stockingQuantity: number;
    isResupplyNeeded: boolean;
    thumbnailUrl: string | null;
    authorization: ProductExistingAuthorizationResponseDto;
    formattedDefaultAmountBeforeVatPerUnit: string;
    detailRoute: string;
  }>;
}

const { getAmountDisplayText } = useCurrencyHelper();
const { getProductCreateRoutePath, getProductDetailRoutePath } = useRouteHelper();
const productListOptions = getMetadata().listOptionsList.product;

export function createProductListModel(responseDto?: ProductGetListResponseDto): ProductListModel {
  const model: ProductListModel = {
    sortByAscending: productListOptions.defaultSortByAscending ?? true,
    sortByFieldName: productListOptions.defaultSortByFieldName ?? "",
    page: 1,
    resultsPerPage:  productListOptions.defaultResultsPerPage,
    searchContent: "",
    items: [],
    pageCount: 0,
    get sortByFieldNameOptions(): string[] {
      return productListOptions.sortByFieldNameOptions;
    },
    get createRoute(): string {
      return getProductCreateRoutePath();
    },
    mapFromResponseDto(responseDto: ProductGetListResponseDto): ProductListModel {
      return {
        ...this,
        items: responseDto.items.map(createProductListProductModel),
        pageCount: responseDto.pageCount
      };
    },
    toRequestDto(): ProductGetListRequestDto {
      const requestDto: ProductGetListRequestDto = {
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

  if (responseDto) {
    return model.mapFromResponseDto(responseDto);
  }

  return model;
}

export function createProductListProductModel(responseDto: ProductGetListProductResponseDto): ProductListProductModel {
  return {
    ...responseDto,
    formattedDefaultAmountBeforeVatPerUnit: getAmountDisplayText(responseDto.defaultAmountBeforeVatPerUnit),
    detailRoute: getProductDetailRoutePath(responseDto.id)
  };
}