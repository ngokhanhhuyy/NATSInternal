import { useHttpClient } from "./httpClient";

export type MetadataApi = {
  getMetadataAsync(): Promise<MetadataGetResponseDto>;
};

const httpClient = useHttpClient();

const metadataApi = {
  async getMetadataAsync(): Promise<MetadataGetResponseDto> {
    return await httpClient.getAsync("/metadata");
  }
};

export function useMetadataApi(): MetadataApi {
  return metadataApi;
}
