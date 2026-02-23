declare global {
  type BrandGetListRequestDto = ImplementsPartial<
      ISearchableListRequestDto &
      IPageableListRequestDto &
      ISortableListRequestDto, {
    sortByAscending: boolean;
    sortByFieldName: string;
    page: number;
    resultsPerPage: number;
    searchContent: string;
  }>;

  type BrandGetListResponseDto = Implements<IPageableListResponseDto<BrandGetListBrandResponseDto>, {
    items: BrandGetListBrandResponseDto[];
    pageCount: number;
    itemCount: number;
  }>;

  type BrandGetDetailResponseDto = {
    id: string;
    name: string;
    website: string | null;
    socialMediaUrl: string | null;
    phoneNumber: string | null;
    email: string | null;
    address: string | null;
    createdDateTime: string;
    country: CountryBasicResponseDto | null;
  };

  type BrandUpsertRequestDto = {
    name: string;
    website: string | null;
    socialMediaUrl: string | null;
    phoneNumber: string | null;
    email: string | null;
    address: string | null;
    countryId: string | null;
  };

  type BrandGetListBrandResponseDto = {
    id: string;
    name: string;
    countryName: string;
    productCount: number;
  };
}

export { };