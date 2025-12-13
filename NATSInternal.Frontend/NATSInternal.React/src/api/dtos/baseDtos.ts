declare global {
  interface ISortableListRequestDto {
    sortByAscending: boolean;
    sortByFieldName: string;
  }

  interface IPageableListRequestDto {
    page: number;
    resultsPerPage: number;
  }

  interface ISortableAndPageableListRequestDto extends ISortableListRequestDto, IPageableListRequestDto { }
}

export { };