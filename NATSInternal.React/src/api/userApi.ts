import { useHttpClient } from "./httpClient";

export type UserApi = {
  getListAsync(requestDto: UserGetListRequestDto): Promise<UserGetListResponseDto>;
  getDetailByIdAsync(id: string): Promise<UserGetDetailResponseDto>;
  getDetailByUserNameAsync(userName: string): Promise<UserGetDetailResponseDto>;
};

const httpClient = useHttpClient();

const userApi: UserApi = {
  getListAsync(requestDto: UserGetListRequestDto): Promise<UserGetListResponseDto> {
    return httpClient.getAsync("/users", requestDto);
  },
  getDetailByIdAsync(id: string): Promise<UserGetDetailResponseDto> {
    return httpClient.getAsync(`/users/${id}`);
  },
  getDetailByUserNameAsync(userName: string): Promise<UserGetDetailResponseDto> {
    return httpClient.getAsync(`/users/${userName}`);
  }
};

export function useUserApi(): UserApi {
  return userApi;
}