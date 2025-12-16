declare global {
  type PhotoCreateOrUpdateModel = {
    id: string | null;
    url: string | null;
    file: string | null;
    isThumbnail: boolean;
    isChanged: boolean;
    isDeleted: boolean;
  };
}

export function createPhotoCreateOrUpdateModel(args: string | PhotoBasicResponseDto): PhotoCreateOrUpdateModel {
  const model: PhotoCreateOrUpdateModel = {
    id: null,
    url: null,
    file: null,
    isThumbnail: false,
    isChanged: false,
    isDeleted: false
  };

  if (typeof args === "string") {
    model.file = args;
    return model;
  }

  return {
    id: args.id,
    url: args.url,
    file: null,
    isThumbnail: args.isThumbnail,
    isChanged: false,
    isDeleted: false
  };
}