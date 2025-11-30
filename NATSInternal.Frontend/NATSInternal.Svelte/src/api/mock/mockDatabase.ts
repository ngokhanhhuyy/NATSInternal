import { fakerVI } from "@faker-js/faker";

export type User = {
  id: string;
  userName: string;
  password: string;
  createdDateTime: Date;
  deletedDateTime: Date | null;
  roles: Role[];
};

export type Role = {
  id: string;
  name: string;
  displayName: string;
  powerLevel: number;
  users: User[];
  permissions: Permission[];
};

export type UserRole = {
  userId: string;
  roleId: string;
  user: User;
  role: Role;
};

export type Permission = {
  id: string;
  name: string;
  roleId: string;
  role: Role[];
};

export type Product = {
  id: string;
  name: string;
  description: string | null;
  unit: string;
  defaultAmountBeforeVatPerUnit: number;
  defaultVatPercentagePerUnit: number;
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
  name: string;
  website: string | null;
  socialMediaUrl: string | null;
  phoneNumber: string | null;
  email: string | null;
  address: string | null;
  createdDateTime: Date;
};

type Database = {
  users: User[];
  roles: Role[];
  userRoles: UserRole[];
  permissions: Permission[];
  products: Product[];
  productCategories: ProductCategory[];
  brands: Brand[];
};

function generateUsers(): User[] {
  console.log("Generating users.");

  const dataList: Pick<User, "userName" | "password">[] = [
    { userName: "admin", password: "admin123" },
    { userName: "developer", password: "developer123" },
    { userName: "staff", password: "staff123" }
  ];

  return dataList.map((data) => ({
    id: crypto.randomUUID(),
    ...data,
    createdDateTime: new Date(),
    deletedDateTime: null,
    roles: []
  }));
}

function generateRoles(): Role[] {
  console.log("Generating roles.");

  const dataList: Pick<Role, "name" | "displayName" | "powerLevel">[] = [
    { name: "Admin", displayName: "Admin", powerLevel: 50 },
    { name: "Developer", displayName: "Nhà phát triển", powerLevel: 40 },
    { name: "Staff", displayName: "Nhân viên", powerLevel: 10 }
  ];

  return dataList.map((data) => ({
    ...data,
    id: crypto.randomUUID(),
    users: [],
    permissions: []
  }));
}

function generateUserRoles(users: User[], roles: Role[]): UserRole[] {
  console.log("Generating userRoles.");

  const userRoles: UserRole[] = [];
  for (const user of users) {
    const userPowerLevel = roles
      .filter((role) => role.name.toLowerCase() === user.userName)
      .map((role) => role.powerLevel)[0];

    for (const role of roles.filter((eveluatingRole) => eveluatingRole.powerLevel <= userPowerLevel)) {
      user.roles.push(role);
      role.users.push(user);
      userRoles.push({
        userId: user.id,
        roleId: role.id,
        user,
        role
      });
    }
  }

  return userRoles;
}

function generatePermissions(_: Role[]): Permission[] {
  console.log("Generating permissions.");

  return [];
}

function generateBrands(): Brand[] {
  console.log("Generating brands.");

  const brands: Brand[] = [];
  for (let i = 0; i < 10; i++) {
    const name = fakerVI.company.name();
    const hyphenJoinedName = name
      .split(new RegExp("(\\.|,)\\s+"))
      .map((nameElement) => nameElement.toLowerCase())
      .join("-");
    const website = `https://${hyphenJoinedName}.${["net", "com", "info", "org"][Math.floor(Math.random() * 4)]}`;

    brands.push({
      id: crypto.randomUUID(),
      name,
      website,
      socialMediaUrl: `https://facebook.com/${hyphenJoinedName}`,
      phoneNumber: fakerVI.phone.number({ style: "international" }),
      email: `${["services", "customers", "contact"][Math.floor(Math.random() * 3)]}@${website}`,
      address: `${fakerVI.location.streetAddress()}, ${fakerVI.location.city}`,
      createdDateTime: new Date()
    });
  }

  return brands;
}

function generateProductsAndProductCategories(brands: Brand[]): [Product[], ProductCategory[]] {
  console.log("Generating products and product categories.");

  const products: Product[] = [];
  const productCategories: ProductCategory[] = [];
  for (let i = 0; i < 30; i++) {
    let category: ProductCategory | null = null;
    if (Math.random() > 0.5) {
      if (Math.random() > 0.25) {
        category = {
          id: crypto.randomUUID(),
          name: fakerVI.commerce.productAdjective(),
          createdDateTime: new Date(),
          lastUpdatedDateTime: null
        };

        productCategories.push(category);
      } else {
        category = productCategories[Math.floor(Math.random() * productCategories.length)];
      }
    }

    products.push({
      id: crypto.randomUUID(),
      name: fakerVI.commerce.product(),
      description: fakerVI.commerce.productDescription(),
      unit: ["Chai", "Lọ", "Hộp", "Thùng"][Math.floor(Math.random() * 4)],
      defaultAmountBeforeVatPerUnit: 0,
      defaultVatPercentagePerUnit: 0,
      isForRetail: Math.random() > 0.5,
      isDiscontinued: Math.random() > 0.9,
      categoryId: category?.id ?? null,
      brandId: Math.random() > 0.5 ? brands[Math.floor(Math.random() * brands.length)].id : null,
      createdDateTime: new Date(),
      lastUpdatedDateTime: null,
      deletedDateTime: null
    });
  }

  return [products, productCategories];
}

function generateDatabase(): Database {
  const users: User[] = generateUsers();
  const roles: Role[] = generateRoles();
  const userRoles: UserRole[] = generateUserRoles(users, roles);
  const permissions: Permission[] = generatePermissions(roles);
  const brands = generateBrands();
  const [products, productCategories] = generateProductsAndProductCategories(brands);

  return {
    users,
    roles,
    userRoles,
    permissions,
    brands,
    productCategories,
    products
  };
}

const database = generateDatabase();

export function useMockDatabase(): Database {
  return database as Database;
}
