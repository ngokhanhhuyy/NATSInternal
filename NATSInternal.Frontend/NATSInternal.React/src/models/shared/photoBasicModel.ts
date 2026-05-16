declare global {
  type PhotoBasicModel = Readonly<{
    id: number;
    url: string;
    isThumbnail: boolean;
  }>;
}

export function createPhotoBasicModel(responseDto: PhotoBasicResponseDto): PhotoBasicModel {
  return {
    id: responseDto.id,
    url: responseDto.url,
    isThumbnail: responseDto.isThumbnail
  };
}
