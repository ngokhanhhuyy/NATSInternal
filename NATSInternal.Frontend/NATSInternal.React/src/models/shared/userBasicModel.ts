import { getUserDetailRoutePath } from "@/helpers";

declare global {
  type UserBasicModel = Readonly<{
    id: number;
    userName: string;
    isDeleted: boolean;
    detailRoute: string;
  }>;
}

export function createUserBasicModel(responseDto: UserBasicResponseDto): UserBasicModel {
  return {
    id: responseDto.id,
    userName: responseDto.userName,
    isDeleted: responseDto.isDeleted,
    detailRoute: getUserDetailRoutePath(responseDto.id)
  };
}
