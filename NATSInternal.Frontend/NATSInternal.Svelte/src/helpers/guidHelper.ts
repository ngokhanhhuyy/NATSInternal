type GuidHelper = {
  getEmptyGuid(): string;
};

const guidHelper: GuidHelper = {
  getEmptyGuid(): string {
    return "00000000-0000-0000-0000-000000000000";
  }
};

export function useGuidHelper(): GuidHelper {
  return guidHelper;
}