import { getMetadata } from "@/metadata";
import { useRouteHelper } from "@/helpers";

declare global {
  type BrandListModel = Implements<ISortableAndPageableListModel<BrandListBrandModel>, {
    sortByAscending: boolean;
    sortByFieldName: string;
    page: number;
    resultsPerPage: number;
    searchContent: string;
    items: BrandListBrandModel[];
    pageCount: number;
    get sortByFieldNameOptions(): string[];
    get createRoute(): string;
    mapFromResponseDto(responseDto: BrandGetListResponseDto): BrandListModel;
    toRequestDto(): ProductGetListRequestDto;
  }>;

  type BrandListBrandModel = Readonly<{
    id: string;
    name: string;
    countryName: string;
    detailRoute: string;
  }>;
}

const { getBrandDetailRoutePath, getBrandCreateRoutePath } = useRouteHelper();
const listOptions = getMetadata().listOptionsList.brand;

export function createProductListModel(responseDto?: BrandGetListResponseDto): BrandListModel {
  const model: BrandListModel = {
    sortByAscending: listOptions.defaultSortByAscending ?? true,
    sortByFieldName: listOptions.defaultSortByFieldName ?? "",
    page: 1,
    resultsPerPage:  listOptions.defaultResultsPerPage,
    searchContent: "",
    items: [],
    pageCount: 0,
    get sortByFieldNameOptions(): string[] {
      return listOptions.sortByFieldNameOptions;
    },
    get createRoute(): string {
      return getBrandCreateRoutePath();
    },
    mapFromResponseDto(responseDto: BrandGetListResponseDto): BrandListModel {
      return {
        ...this,
        items: responseDto.items.map(createBrandListBrandModel),
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

export function createBrandListBrandModel(responseDto: BrandGetListBrandResponseDto): BrandListBrandModel {
  return {
    ...responseDto,
    detailRoute: getBrandDetailRoutePath(responseDto.id)
  };
}