import { useHttpClient } from "./httpClient";
import type { VerifyUserNameAndPasswordRequestDto, ChangePasswordRequestDto } from "./dtos";
import { AuthorizationError } from "./errors";

const httpClient = useHttpClient();

async function signInAsync(requestDto: VerifyUserNameAndPasswordRequestDto): Promise<void> {
  return await httpClient.postAndIgnoreAsync("/authentication/getAccessCookie", requestDto);
}

async function clearAccessToken(): Promise<void> {
  return await httpClient.postAndIgnoreAsync("/authentication/clearAccessCookie", {  });
}

async function changePasswordAsync(requestDto: ChangePasswordRequestDto): Promise<void> {
  return await httpClient.postAndIgnoreAsync("/authentication/changePassword", requestDto);
}

async function checkAuthenticationStatusAsync(): Promise<boolean> {
  try {
    await httpClient.getAsync("/authentication/checkAuthenticationStatus", { });
    return true;
  } catch (error) {
    if (error instanceof AuthorizationError) {
      return false;
    }

    throw error;
  }
}

export { signInAsync, clearAccessToken, changePasswordAsync, checkAuthenticationStatusAsync };