export function getDefaultAvatarUrlByFullName(fullName: string): string {
  const computedFullName = fullName.replaceAll(" ", "+");
  return `https://ui-avatars.com/api/?name=${computedFullName}&background=random`;
}
