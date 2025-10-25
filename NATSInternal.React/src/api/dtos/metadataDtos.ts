declare global {
  type DisplayNameListResponseDto = Record<string, string>;
  type FieldToSortOptionResponseDto = { options: string[]; defaultOption: string | null };
  type FieldToSortOptionListResponseDto = {
    user: FieldToSortOptionResponseDto;
    product: FieldToSortOptionResponseDto;
  };

  type MetadataResponseDto = {
    displayNameList: DisplayNameListResponseDto;
    fieldToSortOptionsList: FieldToSortOptionListResponseDto;
  };
}