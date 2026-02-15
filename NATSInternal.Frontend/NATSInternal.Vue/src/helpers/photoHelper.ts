export type PhotoHelper = {
  getDefaultPhotoUrl(): string;
};

const helper: PhotoHelper = {
  getDefaultPhotoUrl(): string {
    return "/images/default.jpg";
  }
};

export function usePhotoHelper(): PhotoHelper {
  return helper;
}