import type { ApiErrorDetails } from "@/api";

declare global {
  type ErrorDetailModel = { propertyPath: string; message: string; };
  type ErrorCollectionModel = {
    isValidated: boolean;
    details: ErrorDetailModel[];
    mapFromApiErrorDetails(apiErrors: ApiErrorDetails): ErrorCollectionModel;
    clear(): ErrorCollectionModel;
  };
}

export function createErrorCollectionModel(): ErrorCollectionModel {
  const model: ErrorCollectionModel = {
    isValidated: false,
    details: [],
    mapFromApiErrorDetails(apiErrors) {
      return {
        ...model,
        details: Object
          .entries(apiErrors)
          .map(([key, value]) => ({ propertyPath: key, message: value }))
          .filter((detail) => detail.message)
        };
    },
    clear: () => ({ ...model, details: [] }),
  };

  return model;
}