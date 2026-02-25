import { useHttpClient } from "./httpClient";

const httpClient = useHttpClient();

export type CountryApi = {
  getAllAsync(): Promise<CountryBasicResponseDto[]>;
};

const countryApi: CountryApi = {
  async getAllAsync(): Promise<CountryBasicResponseDto[]> {
    return await httpClient.getAsync("/products/brands/countries");
  }
};

export function useCountryApi(): CountryApi {
  return countryApi;
}