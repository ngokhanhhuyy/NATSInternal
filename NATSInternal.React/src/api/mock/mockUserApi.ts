import { useMockDatabase, type User, type Role } from "./mockDatabase";
import type { UserApi } from "../userApi";

const mockDatabase = useMockDatabase();

const userApi: UserApi = {
  getListAsync: async (requestDto) => {
    const users = mockDatabase.users.map(user => {
      const roleIds = mockDatabase.userRoles
        .filter(userRole => userRole.userId == user.id)
        .map(userRole => userRole.roleId);
        
      return {
        ...user,
        roles: mockDatabase.roles
          .filter(role => roleIds.includes(role.id))
          .map(role => ({
            ...role,
            permissionNames: mockDatabase.permissions.filter(permission => permission.roleId === role.id)
          }))
      }
    });
    const filteredUsers: User[]

    // const filteredUsers = mockDatabase.users.filter(user => {
    //   let shouldSelect = false;
    //   if (requestDto.searchContent && requestDto.searchContent.toLowerCase().includes(user.userName.toLowerCase())) {
    //     shouldSelect = true;
    //   }

    //   if (requestDto.roleId && user.r)
    // });
  } 
};

function doesUserMatchSearchContent(user: User, searchContent: string): boolean {
  const lowerCaseSearchContent = searchContent.toLowerCase();
  if (lowerCaseSearchContent.includes(user.userName.toLowerCase())) {
    return true;
  }

  if (lowerCaseSearchContent.in)
}