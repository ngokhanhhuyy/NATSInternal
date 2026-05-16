import { api } from "@/api";

export const metadata = await api.metadata.getMetadataAsync();

export function getDisplayName(key: string): string | null {
  let camelCaseKey: string = key;
  if (camelCaseKey[0] !== camelCaseKey[0].toLowerCase()) {
    camelCaseKey = camelCaseKey[0].toLowerCase() + camelCaseKey.substring(1);
  }

  return metadata.displayNameList[camelCaseKey];
}

export function getFieldToSortOptions(): MetadataListOptionsListResponseDto {
  return metadata.listOptionsList;
}
