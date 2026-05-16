import { httpClient } from "./httpClient";

export type UserApi = {
  getListAsync(requestDto: UserListRequestDto): Promise<UserListResponseDto>;
  getDetailByIdAsync(id: number): Promise<UserDetailResponseDto>;
  getDetailByUserNameAsync(userName: string): Promise<UserDetailResponseDto>;
};

export const userApi: UserApi = {
  getListAsync(requestDto: UserListRequestDto): Promise<UserListResponseDto> {
    return httpClient.getAsync("/users", requestDto);
  },
  getDetailByIdAsync(id: number): Promise<UserDetailResponseDto> {
    return httpClient.getAsync(`/users/${id}`);
  },
  getDetailByUserNameAsync(userName: string): Promise<UserDetailResponseDto> {
    return httpClient.getAsync(`/users/${userName}`);
  }
};
