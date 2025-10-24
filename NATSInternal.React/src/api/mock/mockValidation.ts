import type { ApiErrorDetails } from "../errors";

type Nullable<T> = T | null | undefined;
type Orderable = number | bigint | Date | string;

type ValidationState = {
  notNullOrUndefined<T>(value: T, path: string, displayName: string): ValidationState;
  notEmpty<T extends string | any[]>(value: T, path: string, displayName: string): ValidationState;
  greaterThan<T extends Orderable>(
    value: T,
    comparisionValue: NonNullable<T>,
    path: string,
    displayName: string): ValidationState;
  greaterThanOrEqualTo<T extends Orderable>(
    value: T,
    comparisionValue: NonNullable<T>,
    path: string,
    displayName: string): ValidationState;
  lessThan<T extends Orderable>(
    value: T,
    comparisionValue: NonNullable<T>,
    path: string,
    displayName: string): ValidationState;
  lessThanOrEqualTo<T extends Orderable>(
    value: T,
    comparisionValue: NonNullable<T>,
    path: string,
    displayName: string): ValidationState;
  must(asserter: () => boolean, path: string, displayName: string): ValidationState;
  get errors(): ValidationState | null;
  get isValid(): boolean;

};

export function createValidator<TRequestDto>(getRequestDto: () => TRequestDto, )

export function createValidationState() {
  const errors: ApiErrorDetails = { };
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
    greaterThan<T extends Nullable<Orderable>>(
        value: T,
        comparisonValue: NonNullable<T>,
        path: string,
        displayName: string): ValidationState {
      if (value == null) {
        return this;
      }

      if (compare(value, comparisonValue) > 0) {
        return this;
      }

      errors[path] = `${displayName} must be greater than ${comparisonValue}.`;
      return this;
    },
    greaterThanOrEqualTo<T extends Nullable<Orderable>>(
        value: T,
        comparisonValue: NonNullable<T>,
        path: string,
        displayName: string): ValidationState {
      if (value == null) {
        return this;
      }

      if (compare(value, comparisonValue) >= 0) {
        return this;
      }

      errors[path] = `${displayName} must be greater than or equal to ${comparisonValue}.`;
      return this;
    },
    lessThan<T extends Nullable<Orderable>>(
        value: T,
        comparisonValue: NonNullable<T>,
        path: string,
        displayName: string): ValidationState {
      if (value == null) {
        return this;
      }

      if (compare(value, comparisonValue) < 0) {
        return this;
      }

      errors[path] = `${displayName} must be less than ${comparisonValue}.`;
      return this;
    },
    lessThanOrEqualTo<T extends Nullable<Orderable>>(
        value: T,
        comparisonValue: NonNullable<T>,
        path: string,
        displayName: string): ValidationState {
      if (value == null) {
        return this;
      }

      if (compare(value, comparisonValue) <= 0) {
        return this;
      }

      errors[path] = `${displayName} must be less than or equal to ${comparisonValue}.`;
      return this;
    },
  };
}

function compare<T extends Orderable>(value: T, comparisonValue: T): number {
  if (typeof value === "bigint") {
    return Number(value - (comparisonValue as bigint));
  }

  if (typeof value === "number") {
    return value - (comparisonValue as number);
  }

  if (value instanceof Date) {
    return value.getTime() - (comparisonValue as Date).getTime();
  }

  return value.localeCompare(comparisonValue as string);
}