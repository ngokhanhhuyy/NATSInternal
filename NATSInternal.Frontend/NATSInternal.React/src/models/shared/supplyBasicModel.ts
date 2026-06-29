import { getSupplyDetailRoutePath, getDisplayDateString } from "@/helpers";

declare global {
  type SupplyBasicModel = {
    id: number;
    shipmentFee: number;
    itemAmount: number;
    statsDate: string;
    thumbnailUrl: string | null;
    authorization: SupplyExistingAuthorizationResponseDto;
    displayStatsDate: string;
    detailRoutePath: string;
  };
}

export function createSupplyBasicModel(responseDto: SupplyBasicResponseDto): SupplyBasicModel {
  return {
    id: responseDto.id,
    shipmentFee: responseDto.shipmentFee,
    itemAmount: responseDto.itemAmount,
    statsDate: responseDto.statsDate,
    thumbnailUrl: responseDto.thumbnailUrl,
    authorization: responseDto.authorization,
    detailRoutePath: getSupplyDetailRoutePath(responseDto.id)
  };
}