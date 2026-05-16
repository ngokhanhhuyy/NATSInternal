export function formatRawPhoneNumber(rawPhoneNumber: string): string {
  const newPhoneNumber = "09" + rawPhoneNumber.substring(0, 3) + rawPhoneNumber.substring(4);
  return newPhoneNumber.replace(/(\d{4})(\d{3})(\d+)/, "$1 $2 $3");
}
