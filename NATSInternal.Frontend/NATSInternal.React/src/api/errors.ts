export type ApiErrorDetails = { [key: string]: string; };

class ApiError extends Error { }

class ApiMessageError extends ApiError {
  protected readonly _modelStateErrors: ApiErrorDetails;

  constructor(modelStateErrors: ApiErrorDetails) {
    super();
    this._modelStateErrors = modelStateErrors;
  }

  get errors(): ApiErrorDetails {
    return this._modelStateErrors;
  }
}

// Exceptions representing request error.
export class ValidationError extends ApiMessageError { }
export class OperationError extends ApiMessageError { }
export class NotFoundError extends ApiError { }
export class AuthenticationError extends ApiError { }
export class AuthorizationError extends ApiError { }
export class ConcurrencyError extends ApiError { }
export class InternalServerError extends ApiError { }
export class UndefinedError extends Error { }
export class ConnectionError extends Error { }
export class FileTooLargeError extends Error { }