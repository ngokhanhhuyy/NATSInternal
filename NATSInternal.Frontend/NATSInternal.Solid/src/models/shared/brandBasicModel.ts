import { useRouteHelper } from "@/helpers";

const { getBrandDetailRoutePath } = useRouteHelper();

declare global {
  type BrandBasicModel = Readonly<{
    id: string;
    name: string;
    detailRoute: string;
  }>;
}

export function createBrandBasicModel(responseDto: BrandBasicResponseDto): BrandBasicModel {
  return {
    id: responseDto.id,
    name: responseDto.name,
    detailRoute: getBrandDetailRoutePath(responseDto.id)
  };
}