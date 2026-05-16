import { httpClient } from "./httpClient";

export type MetadataApi = {
  getMetadataAsync(): Promise<MetadataResponseDto>;
};

export const metadataApi = {
  async getMetadataAsync(): Promise<MetadataResponseDto> {
    return await httpClient.getAsync("/metadata");
  }
};
