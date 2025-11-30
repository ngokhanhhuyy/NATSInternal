import { useHttpClient } from "./httpClient";

export type MetadataApi = {
  getMetadataAsync(): Promise<MetadataResponseDto>;
};

const httpClient = useHttpClient();

const metadataApi = {
  async getMetadataAsync(): Promise<MetadataResponseDto> {
    return await httpClient.getAsync("/metadata");
  }
};

export function useMetadataApi(): MetadataApi {
  return metadataApi;
}
