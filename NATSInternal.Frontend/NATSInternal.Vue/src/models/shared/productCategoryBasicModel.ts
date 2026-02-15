declare global {
  type ProductCategoryBasicModel = {
    id: string;
    name: string;
  };
}

function createProductCategoryBasicModel(responseDto: ProductCategoryBasicResponseDto): ProductCategoryBasicModel {
  return {
    id: responseDto.id,
    name: responseDto.name
  };
}

export { createProductCategoryBasicModel };