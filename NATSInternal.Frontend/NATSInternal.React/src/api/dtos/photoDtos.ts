declare global {
  type PhotoUpsertRequestDto = {
    id: number | null;
    file: string;
    isThumbnail: boolean;
    isChanged: boolean;
    isDeleted: boolean;
  };
}
