import { useRouteHelper } from "@/helpers";

declare global {
  type UserBasicModel = {
    id: string;
    userName: string;
    isDeleted: boolean;
    get detailRoute(): string;
  };
}

const { getUserProfileRoutePath } = useRouteHelper();

export function createUserBasicModel(responseDto: UserBasicResponseDto): UserBasicModel {
  return {
    ...responseDto,
    get detailRoute(): string {
      return getUserProfileRoutePath(this.id);
    }
  };
}
