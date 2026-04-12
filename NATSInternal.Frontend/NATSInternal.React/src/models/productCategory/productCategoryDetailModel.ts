import { createUserBasicModel } from "../shared/userBasicModel";
import { createProductCategoryBasicModel } from "../shared/productCategoryBasicModel";
import { useDateTimeHelper, useRouteHelper } from "@/helpers";

declare global {
  type ProductCategoryDetailModel = Readonly<{
    id: string;
    name: string;
    createdDateTime: string;
    createdUser: UserBasicModel;
    lastUpdatedDateTime: string | null;
    lastUpdatedUser: UserBasicModel | null;
    updateRoutePath: string;
    toBasicModel(): ProductCategoryBasicModel;
  }>;
}

const { getDisplayDateTimeString }  = useDateTimeHelper();
const { getProductCategoryUpdateRoutePath } = useRouteHelper();

export function createProductCategoryDetailModel(responseDto: ProductCategoryGetDetailResponseDto): ProductCategoryDetailModel {
  return {
    ...responseDto,
    createdDateTime: getDisplayDateTimeString(responseDto.createdDateTime),
    createdUser: createUserBasicModel(responseDto.createdUser),
    lastUpdatedDateTime: responseDto.lastUpdatedDateTime && getDisplayDateTimeString(responseDto.lastUpdatedDateTime),
    lastUpdatedUser: responseDto.lastUpdatedUser && createUserBasicModel(responseDto.lastUpdatedUser),
    updateRoutePath: getProductCategoryUpdateRoutePath(responseDto.id),
    toBasicModel(): ProductCategoryBasicModel {
      return createProductCategoryBasicModel(this);
    }
  };
}