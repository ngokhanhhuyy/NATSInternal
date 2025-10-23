declare global {
  type MockApiErrorDetails<TRequestDto> = { [K in keyof TRequestDto & string]?: string };
}

type Nullable<T> = T | null | undefined;

type ValidationState = {
  notNullOrUndefined<T>(value: T, path: string, displayName: string): ValidationState;
  notEmpty<T extends string | any[]>(value: T, path: string, displayName: string): ValidationState;
  greaterThan<T extends number | bigint| Date>(
    value: T,
    comparisionValue: NonNullable<T>,
    path: string,
    displayName: string): ValidationState;
  greaterThanOrEqualTo<T extends number | bigint| Date>(
    value: T,
    comparisionValue: NonNullable<T>,
    path: string,
    displayName: string): ValidationState;
  lessThan<T extends number | bigint| Date>(
    value: T,
    comparisionValue: NonNullable<T>,
    path: string,
    displayName: string): ValidationState;
  lessThanOrEqualTo<T extends number | bigint| Date>(
    value: T,
    comparisionValue: NonNullable<T>,
    path: string,
    displayName: string): ValidationState;
};

export function validate<T>(value: T) {
  const errors: { [key: string]: string } = { };
  const state: ValidationState = {
    notNullOrUndefined<T>(value: T, path: string, displayName: string): ValidationState {
      if (value == null) {
        errors[path] = `${displayName} is required.`;
      }

      return this;
    },
    notEmpty<T extends Nullable<string | any[]>>(value: T, path: string, displayName: string): ValidationState {
      if (value == null) {
        return this;
      }

      if (value.length === 0) {
        errors[path] = `${displayName} cannot be empty.`;
      }

      return this;
    },
    greaterThan<T extends Nullable<number | bigint | Date>>(
        value: T,
        comparisonValue: NonNullable<T>,
        path: string,
        displayName: string): ValidationState {
      if (value == null) {
        return this;
      }

      if ((typeof value === "number" || typeof value === "bigint") && value > (comparisonValue as number | bigint)) {
        return this;
      }

      if (value instanceof Date && value.getTime() > (comparisonValue as Date).getTime()) {
        return this;
      }

      errors[path] = `${displayName} must be greater than ${comparisonValue}.`;
      return this;
    },

  }
}

function compare<T extends number | bigint | Date | string>(value: T, comparisonValue: T): number {
  if (typeof value === "bigint" || typeof comparisonValue === "bigint") {
    const trucatedValue = typeof value
    return BigInt(value as number | bigint) - BigInt(comparisonValue as number | bigint);
  }

  if (|| typeof value === "bigint"))

  if (value instanceof Date && value.getTime() > (comparisonValue as Date).getTime()) {
    return this;
  }
}