export type UserApi = {
  getListAsync(requestDto: UserGetListRequestDto): Promise<UserGetListResponseDto>;
  getDetailAsync(id: string): Promise<UserGetDetailResponseDto>;
};