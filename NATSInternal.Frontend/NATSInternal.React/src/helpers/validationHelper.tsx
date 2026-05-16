export function validatePhoneNumber(phoneNumber: string): boolean {
  return /^[0-9+]+$/g.test(phoneNumber);
}

export function validateEmail(email: string): boolean {
  return /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$/g.test(email);
}
