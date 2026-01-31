import { createBrandBasicModel } from "../shared/brandBasicModel";
import { createProductCategoryBasicModel } from "../shared/productCategoryBasicModel";
import { getMetadata } from "@/metadata";
import { useCurrencyHelper, useRouteHelper } from "@/helpers";
import defaultImageUrl from "@/assets/images/default.jpg";

declare global {
  type ProductListModel = Implements<
      ISearchableListModel<ProductListProductModel> &
      ISortableListModel<ProductListProductModel> &
      IPageableListModel<ProductListProductModel> &
      IUpsertableListModel<ProductListProductModel>, {
    sortByAscending: boolean;
    sortByFieldName: string;
    page: number;
    resultsPerPage: number;
    searchContent: string;
    brand: BrandBasicModel | null;
    category: ProductCategoryBasicModel | null;
    items: ProductListProductModel[];
    pageCount: number;
    itemCount: number;
    get sortByFieldNameOptions(): string[];
    get createRoutePath(): string;
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
    thumbnailUrl: string;
    authorization: ProductExistingAuthorizationResponseDto;
    category: ProductCategoryBasicModel | null;
    brand: BrandBasicModel | null;
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
    brand: null,
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
    mapFromResponseDto(responseDto: ProductGetListResponseDto): ProductListModel {
      return {
        ...this,
        items: responseDto.items.map(createProductListProductModel),
        pageCount: responseDto.pageCount,
        itemCount: responseDto.itemCount
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

      if (this.brand) {
        requestDto.brandId = this.brand.id;
      }

      if (this.category) {
        requestDto.categoryId = this.category.id;
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
    thumbnailUrl: responseDto.thumbnailUrl ?? defaultImageUrl,
    formattedDefaultAmountBeforeVatPerUnit: getAmountDisplayText(responseDto.defaultAmountBeforeVatPerUnit),
    category: responseDto.category && createProductCategoryBasicModel(responseDto.category),
    brand: responseDto.brand && createBrandBasicModel(responseDto.brand),
    detailRoute: getProductDetailRoutePath(responseDto.id)
  };
}