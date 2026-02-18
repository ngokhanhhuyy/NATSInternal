declare global {
  type PhotoBasicModel = Readonly<{
    id: string;
    url: string;
    isThumbnail: boolean;
  }>;
}

export function createPhotoBasicModel(responseDto: PhotoBasicResponseDto): PhotoBasicModel {
  return { ...responseDto };
}