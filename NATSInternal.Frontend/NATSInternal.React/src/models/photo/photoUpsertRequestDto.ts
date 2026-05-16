declare global {
  type PhotoUpsertModel = {
    id: number | null;
    url: string | null;
    file: string | null;
    isThumbnail: boolean;
    isChanged: boolean;
    isDeleted: boolean;
    toRequestDto(): PhotoUpsertRequestDto;
  };
}

export function createPhotoUpsertModel(args: string | PhotoBasicResponseDto): PhotoUpsertModel {
  const model: PhotoUpsertModel = {
    id: null,
    url: null,
    file: null,
    isThumbnail: false,
    isChanged: false,
    isDeleted: false,
    toRequestDto(): PhotoUpsertRequestDto {
      return {
        id: this.id,
        file: this.file ?? "",
        isThumbnail: this.isThumbnail,
        isChanged: this.isChanged,
        isDeleted: this.isDeleted
      };
    }
  };

  if (typeof args === "string") {
    model.file = args;
    return model;
  }

  model.id = args.id;
  model.url = args.url;
  model.isThumbnail = args.isThumbnail;
  return model;
}
