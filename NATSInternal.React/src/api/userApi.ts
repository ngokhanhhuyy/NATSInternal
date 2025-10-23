export type UserApi = {
  getListAsync(requestDto: UserGetListRequestDto): Promise<UserGetListResponseDto>;
  getDetailByIdAsync(id: string): Promise<UserGetDetailResponseDto>;
  getDetailByUserNameAsync(userName: string): Promise<UserGetDetailResponseDto>;
};