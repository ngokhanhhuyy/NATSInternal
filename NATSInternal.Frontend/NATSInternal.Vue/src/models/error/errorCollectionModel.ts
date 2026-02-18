import type { ApiErrorDetails } from "@/api";

declare global {
  type ErrorDetailModel = { propertyPath: string; message: string; };
  type ErrorCollectionModel = {
    isValidated: boolean;
    details: ErrorDetailModel[];
    get isValid(): boolean;
    mapFromApiErrorDetails(apiErrors: ApiErrorDetails): void;
    clear(): void;
  };
}

export function createErrorCollectionModel(): ErrorCollectionModel {
  const model: ErrorCollectionModel = {
    isValidated: false,
    details: [],
    get isValid(): boolean {
      return this.details.length === 0;
    },
    mapFromApiErrorDetails(apiErrors): void {
      this.isValidated = true;
      this.details = Object
        .entries(apiErrors)
        .map(([key, value]) => ({ propertyPath: key, message: value }))
        .filter((detail) => detail.message);
    },
    clear(): void {
      this.details = [];
    },
  };

  return model;
}