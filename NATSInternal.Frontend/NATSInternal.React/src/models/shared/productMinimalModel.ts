declare global {
  type ProductMinimalModel = {
    id: number;
    name: string;
  };
}

function create(responseDto: ProductMinimalResponseDto | ProductBasicResponseDto): ProductMinimalModel {
  return {
    id: responseDto.id,
    name: responseDto.name
  };
}

export { create as createProductMinimalModel };
