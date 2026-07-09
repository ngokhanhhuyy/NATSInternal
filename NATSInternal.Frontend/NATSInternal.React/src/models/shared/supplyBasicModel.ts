import { getSupplyDetailRoutePath, getDisplayDateString, getDisplayAmountText } from "@/helpers";

declare global {
  type SupplyBasicModel = {
    id: number;
    amount: number;
    productCount: number;
    statsDate: string;
    thumbnailUrl: string | null;
    authorization: SupplyExistingAuthorizationResponseDto;
    displayStatsDate: string;
    displayAmount: string
    detailRoutePath: string;
  };
}

export function createSupplyBasicModel(responseDto: SupplyBasicResponseDto): SupplyBasicModel {
  return {
    id: responseDto.id,
    amount: responseDto.amount,
    productCount: responseDto.productCount,
    statsDate: responseDto.statsDate,
    thumbnailUrl: responseDto.thumbnailUrl,
    authorization: responseDto.authorization,
    displayStatsDate: getDisplayDateString(responseDto.statsDate),
    displayAmount: getDisplayAmountText(responseDto.amount, { suffix: " vnđ" }),
    detailRoutePath: getSupplyDetailRoutePath(responseDto.id)
  };
}
