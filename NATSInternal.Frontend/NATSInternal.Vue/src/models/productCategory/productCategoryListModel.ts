import { getMetadata } from "@/metadata";
import { useRouteHelper } from "@/helpers";

declare global {
  type ProductCategoryListModel = Implements<
      ISearchableListModel<ProductCategoryListProductCategoryModel> &
      ISortableListModel<ProductCategoryListProductCategoryModel> &
      IPageableListModel<ProductCategoryListProductCategoryModel> &
      IUpsertableListModel<ProductCategoryListProductCategoryModel>, {
    sortByAscending: boolean;
    sortByFieldName: string;
    page: number;
    resultsPerPage: number;
    searchContent: string;
    items: ProductCategoryListProductCategoryModel[];
    pageCount: number;
    itemCount: number;
    get sortByFieldNameOptions(): string[];
    get createRoutePath(): string;
    mapFromResponseDto(responseDto: ProductCategoryGetListResponseDto): ProductCategoryListModel;
    toRequestDto(): ProductCategoryGetListRequestDto;
  }>;

  type ProductCategoryListProductCategoryModel = Readonly<{
    id: string;
    name: string;
    countryName: string;
  }>;
}

const { getProductCategoryCreateRoutePath } = useRouteHelper();
const listOptions = getMetadata().listOptionsList.brand;

function createListModel(responseDto?: ProductCategoryGetListResponseDto): ProductCategoryListModel {
  const model: ProductCategoryListModel = {
    sortByAscending: listOptions.defaultSortByAscending ?? true,
    sortByFieldName: listOptions.defaultSortByFieldName ?? "",
    page: 1,
    resultsPerPage:  listOptions.defaultResultsPerPage,
    searchContent: "",
    items: [],
    pageCount: 0,
    itemCount: 0,
    get sortByFieldNameOptions(): string[] {
      return listOptions.sortByFieldNameOptions;
    },
    get createRoutePath(): string {
      return getProductCategoryCreateRoutePath();
    },
    mapFromResponseDto(responseDto: ProductCategoryGetListResponseDto): ProductCategoryListModel {
      return {
        ...this,
        items: responseDto.items.map(createProductCategoryModel),
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

      return requestDto;
    }
  };

  if (responseDto) {
    return model.mapFromResponseDto(responseDto);
  }

  return model;
}

function createProductCategoryModel(
    responseDto: ProductCategoryGetListProductCategoryResponseDto): ProductCategoryListProductCategoryModel {
  return { ...responseDto };
}

export {
  createListModel as createProductCategoryListModel,
  createProductCategoryModel as createProductCategoryListProductCategoryModel
};