import * as authenticationApi from "./authenticationApi";

interface IApi {
  authentication: typeof authenticationApi
}

export function useApi(): IApi {
  return {
    authentication: authenticationApi
  };
}

export * from "./dtos";
export * from "./errors";