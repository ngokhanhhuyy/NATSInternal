export type PhoneNumberHelper = {
  formatRawPhoneNumber(rawPhoneNumber: string): string;
};

const phoneNumberHelper: PhoneNumberHelper = {
  formatRawPhoneNumber(rawPhoneNumber: string): string {
    const newPhoneNumber = "09" + rawPhoneNumber.substring(0, 3) + rawPhoneNumber.substring(4);
    return newPhoneNumber.replace(/(\d{4})(\d{3})(\d+)/, "$1 $2 $3");
  }
};

export function usePhoneNumberHelper(): PhoneNumberHelper {
  return phoneNumberHelper;
}
