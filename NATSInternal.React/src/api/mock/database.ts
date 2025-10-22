import { faker } from "@faker-js/faker";

export type User = {
  id: string;
  userName: string;
  password: string;
  createdDateTime: Date;
  deletedDateTime: Date | null;
};

export type UserRole = {
  userId: string;
  roleId: string;
};

export type Role = {
  id: string;
  name: string;
  displayName: string;
  powerLevel: number;
};

export type Permission = {
  id: string;
  name: string;
  roleId: string;
};

export type Product = {
  id: string;
  name: string;
  description: string | null;
  unit: string;
  defaultAmountBeforeVatPerUnit: number;
  defailtVatPercentagePerUnit: number;
  isForRetail: boolean;
  isDiscontinued: boolean;
  categoryId: string | null;
  brandId: string | null;
  createdDateTime: Date;
  lastUpdatedDateTime: Date | null;
  deletedDateTime: Date | null;
};

export type ProductCategory = {
  id: string;
  name: string;
  createdDateTime: Date;
  lastUpdatedDateTime: Date | null;
};

export type Brand = {
  id: string;
  website: string | null;
  socialMediaUrl: string | null;
  phoneNumber: string | null;
  email: string | null;
  address: string | null;
  createdDateTime: Date;
};

type Database = {
  users: User[];
  userRoles: UserRole[];
  roles: Role[];
  permissions: Permission[];
  products: Product[];
  productCategories: ProductCategory[];
  brands: Brand[];
};

function generateUsers(): User[] {
  const dataList: Pick<User, "userName" | "password">[] = [
    { userName: "admin", password: "admin123"  },
    { userName: "developer", password: "developer123"  },
    { userName: "staff", password: "staff123"  },
  ];

  return dataList.map(data => ({
    id: crypto.randomUUID(),
    ...data,
    createdDateTime: new Date(),
    deletedDateTime: null
  }));
}

function generateRoles(): Role[] {
  const dataList: Pick<Role, "name" | "displayName" | "powerLevel">[] = [
    { name: "Admin", displayName: "Admin", powerLevel: 50 },
    { name: "Developer", displayName: "Nhà phát triển", powerLevel: 40 },
    { name: "Staff", displayName: "Nhân viên", powerLevel: 10 }
  ];

  return dataList.map(data => ({
    id: crypto.randomUUID(),
    ...data
  }));
}

function generateUserRoles(users: User[], roles: Role[]): UserRole[] {
  const userRoleMap = new Map<string, string>();
  for (const user of users) {
    const userPowerLevel = roles
      .filter(role => role.name.toLowerCase() === user.userName)
      .map(role => role.powerLevel)[0];
    
    for (const role of roles.filter(eveluatingRole => eveluatingRole.powerLevel <= userPowerLevel)) {
      userRoleMap.set(user.id, role.id);
    }
  }

  return userRoleMap.entries().map(entry => )
}

function generateDatabase(): Database {
  const database = { } as Database;
  database.users = generateUsers();
  database.roles = generateRoles();

  return database;
}

const database = generateDatabase();

export function useDatabase(): Database {
  return database as Database;
}