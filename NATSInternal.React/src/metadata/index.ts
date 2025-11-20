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
  return getMetadata().displayNameList[key];
}

export function getFieldToSortOptions(): ListOptionsListResponseDto {
  return getMetadata().listOptionsList;
}