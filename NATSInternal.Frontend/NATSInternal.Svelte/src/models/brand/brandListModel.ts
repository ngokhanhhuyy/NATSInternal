import { getMetadata } from "@/metadata";
import { useRouteHelper } from "@/helpers";

declare global {
  type BrandListModel = Implements<
      ISearchableListModel<BrandListBrandModel> &
      ISortableListModel<BrandListBrandModel> &
      IPageableListModel<BrandListBrandModel> &
      IUpsertableListModel<BrandListBrandModel>, {
    sortByAscending: boolean;
    sortByFieldName: string;
    page: number;
    resultsPerPage: number;
    searchContent: string;
    items: BrandListBrandModel[];
    pageCount: number;
    itemCount: number;
    get sortByFieldNameOptions(): string[];
    get createRoutePath(): string;
    mapFromResponseDto(responseDto: BrandGetListResponseDto): BrandListModel;
    toRequestDto(): BrandGetListRequestDto;
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

export function createBrandListModel(responseDto?: BrandGetListResponseDto): BrandListModel {
  const model: BrandListModel = {
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
      return getBrandCreateRoutePath();
    },
    mapFromResponseDto(responseDto: BrandGetListResponseDto): BrandListModel {
      return {
        ...this,
        items: responseDto.items.map(createBrandListBrandModel),
        pageCount: responseDto.pageCount,
        itemCount: responseDto.itemCount
      };
    },
    toRequestDto(): BrandGetListRequestDto {
      const requestDto: BrandGetListRequestDto = {
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