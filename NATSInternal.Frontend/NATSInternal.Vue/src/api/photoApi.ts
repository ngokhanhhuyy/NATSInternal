import { useHttpClient } from "./httpClient";

const httpClient = useHttpClient();

export type PhotoApi = {
  getMultipleByProductIdsAsync(requestDto: PhotoGetMultipleByProductIds): Promise<PhotoBasicResponseDto[]>;
};

const photoApi: PhotoApi = {
  async getMultipleByProductIdsAsync(requestDto: PhotoGetMultipleByProductIds): Promise<PhotoBasicResponseDto[]> {
    return await httpClient.getAsync("/photos", requestDto);
  },
};

export function usePhotoApi(): PhotoApi {
  return photoApi;
}