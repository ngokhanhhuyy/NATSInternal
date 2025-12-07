type ValidationHelper = {
  validatePhoneNumber(phoneNumber: string): boolean;
  validateEmail(email: string): boolean;
}

const validationHelper: ValidationHelper = {
  validatePhoneNumber(phoneNumber: string): boolean {
    return /^[0-9+]+$/g.test(phoneNumber);
  },
  validateEmail(email: string): boolean {
    return /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$/g.test(email);
  }
}

export function useValidationHelper(): ValidationHelper {
  return validationHelper;
}