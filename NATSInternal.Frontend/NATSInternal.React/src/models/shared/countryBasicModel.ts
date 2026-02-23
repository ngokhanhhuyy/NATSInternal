declare global {
  type CountryBasicModel = Readonly<{
    id: string;
    code: string;
    name: string;
  }>;
}

export function createCountryBasicModel(responseDto: CountryBasicResponseDto): CountryBasicModel {
  return { ...responseDto };
}