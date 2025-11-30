import { useApi } from "@/api";

const api = useApi();
const metadata = await api.metadata.getMetadataAsync();

export function getMetadata(): MetadataResponseDto {
  if (metadata == null) {
    throw new Error("Metadata has not been loaded.");
  }

  return metadata;
}

export function getDisplayName(key: string): string | null {
  let camelCaseKey: string = key;
  if (camelCaseKey[0] !== camelCaseKey[0].toLowerCase()) {
    camelCaseKey = camelCaseKey[0].toLowerCase() + camelCaseKey.substring(1);
  }

  return getMetadata().displayNameList[camelCaseKey];
}

export function getFieldToSortOptions(): ListOptionsListResponseDto {
  return getMetadata().listOptionsList;
}
