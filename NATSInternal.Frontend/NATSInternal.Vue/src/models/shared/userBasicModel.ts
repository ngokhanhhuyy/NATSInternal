import { useRouteHelper } from "@/helpers";

declare global {
  type UserBasicModel = Readonly<{
    id: string;
    userName: string;
    isDeleted: boolean;
    detailRoute: string;
  }>;
}

const { getUserProfileRoutePath } = useRouteHelper();

export function createUserBasicModel(responseDto: UserBasicResponseDto): UserBasicModel {
  return {
    ...responseDto,
    detailRoute: getUserProfileRoutePath(responseDto.id)
  };
}