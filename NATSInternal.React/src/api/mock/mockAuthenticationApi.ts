import { useMockDatabase, type User } from "./mockDatabase";
import type { AuthenticationApi } from "../authenticationApi";
import { createUserGetDetailResponseDto } from "./mockUserApi";
import { OperationError, AuthenticationError } from "../errors";
import { toAsyncMicrotask } from "./mockApiHelpers";
import { validateUsingSchema } from "./mockValidation";
import * as v from "valibot";

export type Caller = Readonly<UserGetDetailResponseDto & {
  hasPermission(permissionName: string): boolean;
  get powerLevel(): number;
}>;

const mockDatabase = useMockDatabase();

async function getAccessCookie(requestDto: VerifyUserNameAndPasswordRequestDto): Promise<void> {
  await new Promise(resolve => resolve(null));
  console.log(123);
  validateUsingSchema(VerifyUserNameAndPasswordRequestDto, requestDto);
  const user = mockDatabase.users.find(u => u.userName === requestDto.userName);
  if (!user) {
    throw new OperationError({ userName: "Không tồn tại" });
  }

  if (user.password !== requestDto.password) {
    throw new OperationError({ password: "Không chính xác." });
  }

  setCaller(user);
}

function clearAccessCookie(): void {
  clearCaller();
}

function changePassword(requestDto: ChangePasswordRequestDto): void {
  validateUsingSchema(ChangePasswordRequestDto, requestDto);
  const caller = getAndEnsureCallerExists();
  const user = mockDatabase.users.find((u) => u.id === caller.id);
  if (!user) {
    clearAccessCookie();
    throw new AuthenticationError();
  }

  if (user?.password !== requestDto.currentPassword) {
    throw new OperationError({ currentPassword: "Mật khẩu hiện tại không chính xác." });
  }

  user.password = requestDto.newPassword;
}

function checkAuthenticationStatus(): void {
  getAndEnsureCallerExists();
}

const mockAuthenticationApi: AuthenticationApi = {
  getAccessCookieAsync: getAccessCookie,
  clearAccessCookieAsync: toAsyncMicrotask(clearAccessCookie),
  changePasswordAsync: toAsyncMicrotask(changePassword),
  checkAuthenticationStatusAsync: toAsyncMicrotask(checkAuthenticationStatus)
};

export function useMockAuthenticationApi(): AuthenticationApi {
  return mockAuthenticationApi;
}

const _caller: Caller | null = null;
const callerKey = "caller";
export function getAndEnsureCallerExists(): Caller {
  if (_caller) {
    return _caller;
  }

  const callerData = localStorage.getItem(callerKey);
  if (callerData == null) {
    localStorage.removeItem(callerKey);
    throw new AuthenticationError();
  }

  try {
    const callerAsUser = JSON.parse(callerData) as UserGetDetailResponseDto;
    return {
      ...callerAsUser,
      hasPermission(permissionName: string): boolean {
        return this.roles
          .flatMap(role => role.permissionNames)
          .includes(permissionName);
      },
      get powerLevel(): number {
        return Math.max(...this.roles.map(role => role.powerLevel));
      }
    };
  } catch {
    localStorage.removeItem(callerKey);
    throw new AuthenticationError();
  }
}

export function setCaller(user: User): void {
  localStorage.setItem(callerKey, JSON.stringify(createUserGetDetailResponseDto(user)));
}

export function clearCaller(): void {
  localStorage.removeItem(callerKey);
}

const VerifyUserNameAndPasswordRequestDto = v.object({
  userName: v.pipe(v.string(), v.minLength(1)),
  password: v.pipe(v.string(), v.minLength(1))
}) satisfies v.GenericSchema<VerifyUserNameAndPasswordRequestDto>;

const ChangePasswordRequestDto = v.pipe(
  v.object({
    currentPassword: v.pipe(v.string(), v.minLength(6)),
    newPassword: v.pipe(v.string(), v.minLength(6)),
    confirmationPassword: v.pipe(v.string())
  }),
  v.check((input) => input.confirmationPassword === input.newPassword, "Passwords mismatch.")
);