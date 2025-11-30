import { useMockDatabase, type User, type Role } from "./mockDatabase";
import { getUserExistingAuthorization } from "./mockAuthorizationApi";
import type { UserApi } from "../userApi";
import { throttleAsync } from "./throttle";
import { NotFoundError } from "../errors";

const mockDatabase = useMockDatabase();

const mockUserApi: UserApi = {
  async getListAsync(requestDto: UserGetListRequestDto): Promise<UserGetListResponseDto> {
    await throttleAsync();
    const filteredUsers: User[] = [];
    for (const user of mockDatabase.users) {
      let shouldPick = false;

      if (requestDto.searchContent && requestDto.searchContent.toLowerCase().includes(user.userName.toLowerCase())) {
        shouldPick = true;
      }

      if (requestDto.roleId && user.roles.map((role) => role.id).includes(requestDto.roleId)) {
        shouldPick = true;
      }

      if (shouldPick) {
        filteredUsers.push(user);
      }
    }

    if (filteredUsers.length === 0) {
      return {
        pageCount: 0,
        items: []
      };
    }

    let sortedUsers: User[] = [];
    switch (requestDto.sortByFieldName) {
      case "CreatedDateTime":
        sortedUsers = filteredUsers.sort((prev, current) => {
          return compareCreatedDateTime(prev.createdDateTime, current.createdDateTime);
        });

        break;
      case "UserName":
        sortedUsers = filteredUsers.sort((prev, current) => {
          return compareUserName(prev.userName, current.userName);
        });

        break;
      case "RoleMaxPowerLevel":
        sortedUsers = filteredUsers.sort((prev, current) => {
          return compareRoleMaxPowerLevel(prev.roles, current.roles);
        });

        break;
      default:
        throw new Error("Not implemented");
    }

    const pageOrDefault = requestDto.page ?? 1;
    const resultsPerPageOrDefault = requestDto.resultsPerPage ?? 15;

    const pageCount = Math.ceil(sortedUsers.length / resultsPerPageOrDefault);
    const skipUserCount = resultsPerPageOrDefault * (pageOrDefault - 1);
    let takenUserCount = 0;
    const paginatedUsers: User[] = [];

    for (let index = 0; index < sortedUsers.length; index++) {
      if (index < skipUserCount || takenUserCount > resultsPerPageOrDefault) {
        continue;
      }

      paginatedUsers.push(sortedUsers[index]);
      takenUserCount += 1;
    }

    return {
      pageCount,
      items: paginatedUsers.map(createUserBasicResponseDto)
    };
  },
  async getDetailByIdAsync(id: string): Promise<UserGetDetailResponseDto> {
    await throttleAsync();
    const user = mockDatabase.users.find((u) => u.id === id);
    if (!user) {
      throw new NotFoundError();
    }

    return createUserGetDetailResponseDto(user);
  },
  async getDetailByUserNameAsync(userName: string): Promise<UserGetDetailResponseDto> {
    await throttleAsync();
    const user = mockDatabase.users.find((u) => u.userName === userName);
    if (!user) {
      throw new NotFoundError();
    }

    return createUserGetDetailResponseDto(user);
  }
};

export function useMockUserApi(): UserApi {
  return mockUserApi;
}

export function createUserBasicResponseDto(user: User): UserBasicResponseDto {
  return {
    ...user,
    roles: user.roles.map((role) => ({
      id: role.id,
      name: role.name,
      displayName: role.displayName,
      powerLevel: role.powerLevel
    })),
    authorization: getUserExistingAuthorization(user)
  };
}

export function createRoleBasicResponseDto(role: Role): RoleBasicResponseDto {
  return {
    id: role.id,
    name: role.name,
    displayName: role.displayName,
    powerLevel: role.powerLevel
  };
}

export function createUserGetDetailResponseDto(user: User): UserGetDetailResponseDto {
  return {
    ...user,
    roles: user.roles.map((role) => ({
      id: role.id,
      name: role.name,
      displayName: role.displayName,
      powerLevel: role.powerLevel,
      permissionNames: role.permissions.map((permission) => permission.name)
    })),
    authorization: getUserExistingAuthorization(user)
  };
}

function compareCreatedDateTime(previousDate: Date, currentDate: Date, byAscending: boolean = true): number {
  let comparedResult = currentDate.getTime() - previousDate.getTime();
  if (!byAscending) {
    comparedResult = -comparedResult;
  }

  return comparedResult;
}

function compareUserName(previousUserName: string, currentUserName: string, byAscending: boolean = true): number {
  if (byAscending) {
    return previousUserName.localeCompare(currentUserName);
  }

  return currentUserName.localeCompare(previousUserName);
}

function compareRoleMaxPowerLevel(previousRoles: Role[], currentRoles: Role[], byAscending: boolean = true): number {
  const previousPowerLevel = getMaxPowerLevelFromRoles(previousRoles);
  const currentPowerLevel = getMaxPowerLevelFromRoles(currentRoles);
  let comparedResult = currentPowerLevel - previousPowerLevel;
  if (byAscending) {
    comparedResult = -comparedResult;
  }

  return comparedResult;
}

function getMaxPowerLevelFromRoles(roles: Role[]): number {
  return Math.max(...roles.map((role) => role.powerLevel));
}
