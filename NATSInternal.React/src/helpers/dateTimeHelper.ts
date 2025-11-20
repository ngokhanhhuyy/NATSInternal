/**
 * Convert datetime string extracted from local-datetime {@link HTMLInputElement} to ISO 8601 format string
 * (ICT-Indochina timezone) to be used as data for request DTOs.
 * 
 * @param dateTimeInputValue String value extracted from datetime-local {@link HTMLInputElement}.
 * @returns An ISO string as data for request DTOs.
 * @example "1997-08-30T21:00" => "1997-08-30T21:00:00"
 */
function getDateTimeISOString(dateTimeInputValue: string): string {
  return dateTimeInputValue + ":00";
}

/**
 * Convert date string extracted from date {@link HTMLInputElement} to ISO 8601 format string to be used as data for
 * request DTOs.
 * 
 * @param dateInputValue String value extracted from date {@link HTMLInputElement}.
 * @returns An ISO string as data for request DTOs.
 * @example "1997-08-30" => "1997-08-30"
 */
function getDateISOString(dateInputValue: string): string {
  return dateInputValue;
}

/**
 * Convert time string extracted from time {@link HTMLInputElement} to ISO 8601 format string to be used as data for
 * request DTOs.
 * 
 * @param timeInputValue String value extracted from date {@link HTMLInputElement}.
 * @returns An ISO string as data for request DTOs.
 * @example "21:00" => "21:00:00"
 */
function getTimeISOString(timeInputValue: string): string {
  return timeInputValue + ":00";
}

/**
 * Convert datetime ISO string retrieved from the server to the format that is used as local-datetime
 * {@link HTMLInputElement} value. If the value is null, the result will be an empty string.
 * 
 * @param responseDtoValue An datetime ISO string retrived from the server.
 * @returns A string used as {@link HTMLInputElement} value.
 * @example
 * "1997-08-30T21:00:00" => "1997-08-30T21:00"
 * null => ""
 */
function getHTMLDateTimeInputString(responseDtoValue: string | null): string {
  if (responseDtoValue) {
    return removeUnnecessaryParts(responseDtoValue);
  }

  return "";
}

/**
 * Generate a string which represents the current datetime as value for local-datetime {@link HTMLInputElement}.
 * 
 * @returns A string representing the current datetime as value for {@link HTMLInputElement}.
 */
function getCurrentDateTimeHTMLInputString(): string {
  const date = new Date();
  date.setMilliseconds(7 * 3_600_000);
  return removeUnnecessaryParts(date.toISOString());
}

/**
 * Convert date ISO string retrieved from the server to the format that is used as date {@link HTMLInputElement} value.
 * If the value is null, the result will be an empty string.
 * 
 * @param responseDtoValue A date ISO string retrived from the server.
 * @returns A string used as date HTMLInputElement value.
 * @example "1997-08-30" => "1997-08-30"
 */
function getHTMLDateInputString(responseDtoValue: string | null): string {
  return responseDtoValue ?? "";
}

/**
 * Generate a string which represents today as value for date {@link HTMLInputElement}.
 * 
 * @returns A string representing the today as value for date {@link HTMLInputElement}.
 */
function getCurrentDateHTMLInputString(): string {
  const date = new Date();
  date.setMilliseconds(7 * 3_600_000);
  return date.toISOString().split("T")[0];
}

/**
 * Convert time ISO string retrieved from the server to the format that is used as time {@link HTMLInputElement} value.
 * If the value is null, the result will be an empty string.
 * 
 * @param responseDtoValue A time ISO string retrived from the server.
 * @returns A string used as time {@link HTMLInputElement} value.
 * @example "21:00" => 21:00"
 */
function getHTMLTimeInputString(responseDtoValue: string | null): string {
  return responseDtoValue ?? "";
}

/**
 * Generate a string which represents the current time as value for time {@link HTMLInputElement}.
 * 
 * @returns A string representing the current time as value for time {@link HTMLInputElement}.
 */
function getCurrentTimeHTMLInputString(): string {
  const date = new Date();
  date.setMilliseconds(7 * 3_600_000);
  return removeUnnecessaryParts(date.toISOString()).split("T")[1];
}

/**
 * Convert datetime ISO string in response DTOs retrieved from server to a string used for dislaying.
 * 
 * @param responseDtoValue An ISO string retrieved server datetime.
 * @returns A formatted datetime string used for displaying.
 * @example "1997-08-30T21:00:00" => "21:00 30-08-1997"
 */
function getDisplayDateTimeString(responseDtoValue: string): string {
  const formattedValue = responseDtoValue.split(".")[0];
  const date = new Date(formattedValue);
  const [day, month, year] = [
    date.getDate().toString().padStart(2, "0"),
    (date.getMonth() + 1).toString(),
    date.getFullYear().toString().padStart(4, "0")
  ];
  const timeValues = [
    date.getHours().toString().padStart(2, "0"),
    date.getMinutes().toString().padStart(2, "0"),
  ];

  return `${timeValues.join("g")}, ngày ${day} tháng ${month}, ${year}`;
}

/**
 * Convert a time ISO String retrieved from the server into a string used for displaying.
 * 
 * @param responseDtoValue A time ISO string in response DTOs retrieved from server.
 * @returns A formatted time string used for displaying.
 * @example "21:00:00" => "21:00"
 */
function getDisplayTimeString(responseDtoValue: string): string {
  const date = new Date(responseDtoValue + "+07:00");
  return date.getHours().toString().padStart(2, "0") +
    "g" + date.getMinutes().toString().padStart(2, "0");
}

/**
 * Convert a date ISO String retrieved from the server into a string used for displaying.
 * 
 * @param responseDtoValue A date ISO string in response DTOs retrieved from server.
 * @returns A date string for displaying.
 * @example "1997-08-30" => "30-08-1997"
 */
function getDisplayDateString(responseDtoValue: string): string {
  const date = new Date(responseDtoValue);
  const [day, month, year] = [
    date.getDate().toString().padStart(2, "0"),
    (date.getMonth() + 1).toString(),
    date.getFullYear().toString().padStart(4, "0")
  ];
  return `Ngày ${day} tháng ${month}, ${year}`;
}

/**
 * Compare 2 dates given their ISO format strings. If the strings contain time parts, they will be removed, only date
 * parts are compared.
 * 
 * @param targetDateString An ISO string representing a date.
 * @param comparedDateString An ISO string representing a date to be compared.
 * @returns 1 if the target date is greater, 0 if they are equal, -1 if the compared date is greater.
 */
function compareDates(targetDateString: string, comparedDateString: string): number {
  return compareDateTimes(
    targetDateString.split("T")[0],
    comparedDateString.split("T")[0]);
}

/**
 * Compare 2 datetimes given their ISO format strings. If the strings contain only date parts, they will be compared
 * with time parts being 00:00:00.
 * 
 * @param targetDateTimeString An ISO string representing a datetime.
 * @param comparedDateTimeString An ISO string representing a datetime to be compared.
 * @returns 1 if the target datetime is greater, 0 if they are equal, -1 if the compared datetime is greater.
 */
function compareDateTimes(
  targetDateTimeString: string,
  comparedDateTimeString: string): number {
  let targetDateTime = new Date(targetDateTimeString);
  if (targetDateTimeString.split("T").length == 1) {
    targetDateTime = new Date(`${targetDateTime}T00:00:00+07:00`);
  }


  let comparedDateTime = new Date(comparedDateTimeString);
  if (comparedDateTimeString.split("T").length == 1) {
    comparedDateTime = new Date(`${comparedDateTime}T00:00:00+07:00`);
  }

  if (targetDateTime.getTime() > comparedDateTime.getTime()) {
    return 1;
  } else if (targetDateTime.getTime() === comparedDateTime.getTime()) {
    return 0;
  } else {
    return -1;
  }
}

/**
 * Remove milliseconds and timezone parts in the string extracted from {@link HTMLInputElement}.
 * 
 * @param value The string extracted from local-datetime {@link HTMLInputElement}.
 * @returns An ISO string representing local datetime.
 * @example
 * RemoveMilliSecondsAndTimeZone("1997-08-30T21:00:00.710Z")
 * => "1997-08-30T21:00:00"
 */
function removeUnnecessaryParts(value: string): string {

  const splittedISOString = value.split("T");
  const dateISOString = splittedISOString[0];
  const timeISOString = splittedISOString[1]
    .split(".")[0]
    .slice(0, 5);
  return [dateISOString, timeISOString].join("T");
}

/**
 * Get the text describing the difference (delta) between the current datetime and the datetime represented by the given
 * string.
 * 
 * @param targetString An ISO string representing the {@link Date} to compare.
 * @returns The text describing the difference.
 */
function getDeltaTextRelativeToNow(targetString: string) {
  const targetDate = new Date(targetString.split(".")[0] + "+07:00");
  const currentDate = new Date();
  const targetTime = targetDate.getTime();
  const currentTime = currentDate.getTime();
  const seconds = (currentTime - targetTime) / 1000;
  const absSeconds = Math.abs(seconds);

  let prefix: string;
  let suffix: string;
  if (seconds > 0) {
    prefix = "";
    suffix = " trước";
  } else {
    prefix = "";
    suffix = " nữa";
  }

  if (absSeconds < 60) {
    return "Vừa xong";
  }

  const minutes = absSeconds / 60;
  if (minutes < 60) {
    return prefix + `${Math.floor(minutes)} phút` + suffix;
  }

  const hours = minutes / 60;
  if (hours < 24) {
    return prefix + `${Math.floor(hours)} giờ` + suffix;
  }

  const days = hours / 24;
  let pastDate: Date;
  let futureDate: Date;
  if (targetTime < currentTime) {
    [pastDate, futureDate] = [targetDate, currentDate];
  } else {
    [pastDate, futureDate] = [currentDate, targetDate];
  }

  const oneMonthEarlierOfFutureDate = getOneMonthEarlierDate(futureDate);
  if (pastDate.getTime() > oneMonthEarlierOfFutureDate.getTime()) {
    return prefix + `${Math.floor(days)} ngày` + suffix;
  }

  const oneYearEarlierOfFutureDate = getOneYearEarlierDate(futureDate);
  if (pastDate.getTime() > oneYearEarlierOfFutureDate.getTime()) {
    const months = getMonthsDifference(pastDate, futureDate);
    return prefix + `${months} tháng` + suffix;
  }

  const years = getYearsDifference(pastDate, futureDate);
  return prefix + `${years} năm` + suffix;
}

/**
 * Get the months difference between the 2 given {@link Date} objects. The value will be rounded down if it contains
 * decimal part.
 * 
 * @param pastDate A {@link Date} object.
 * @param futureDate A {@link Date} object which represents later date and time than the `startingDate` parameter.
 * @returns The number of months difference between the given {@link Date} objects.
 */
function getMonthsDifference(pastDate: Date, futureDate: Date): number {
  const pastYear = pastDate.getFullYear();
  const futureYear = futureDate.getFullYear();
  const pastTotalMonths = pastYear * 12 + pastDate.getMonth();
  const futureTotalMonths = futureYear * 12 + futureDate.getMonth();
  const monthDifference = Math.abs(pastTotalMonths - futureTotalMonths);

  // Add months difference to past date to check if the difference is full.
  // Example: The difference between 2023-12-31 and 2024-01-01 is 1 month, but since the difference in days is only 1
  // day, it's not a full month difference, so it should be adjusted to 0 month difference (rounded down).
  const daysInPastMonth = getDaysInMonth(
    pastDate.getFullYear(),
    pastDate.getMonth() + 1);
  const simulatingDate = futureDate;
  simulatingDate.setDate(Math.min(pastDate.getDate(), daysInPastMonth));
  if (simulatingDate.getTime() > futureDate.getTime()) {
    return monthDifference - 1;
  }

  return monthDifference;
}

/**
 * Get the years difference between the 2 given {@link Date} objects. The value will be rounded down if it contains
 * decimal part.
 * 
 * @param pastDate A {@link Date} object.
 * @param futureDate A {@link Date} object which represents later date and time than the `startingDate` parameter.
 * @returns The number of years difference between the given {@link Date} objects.
 */
function getYearsDifference(pastDate: Date, futureDate: Date): number {
  const pastYear = pastDate.getFullYear();
  const futureYear = futureDate.getFullYear();
  const yearsDifference = futureYear - pastYear;

  // Add years difference to past date to check if the difference is full.
  // Example: The difference between 2023-12 and 2024-01 is 1 year, but
  // since the difference in months is only 1 month, it's not a full year
  // difference, so it should be adjusted to 0 year difference (rounded down).
  const simulatingDate = new Date(pastDate.toISOString());
  simulatingDate.setFullYear(futureDate.getFullYear());
  if (simulatingDate.getTime() > futureDate.getTime()) {
    return yearsDifference - 1;
  }

  return yearsDifference;
}

/**
 * Get the number of total days in the specified month and year.
 * 
 * @param year The year to which the month to be checked belongs.
 * @param month The month to be checked.
 * @returns The number of total days in the month.
 */
function getDaysInMonth(year: number, month: number): number {
  const date = new Date(year, month, 0);
  return date.getDate();
}

/**
 * Get the {@link Date} object representing the date which is exactly 1 year different from the given date. The result
 * date will have the same month, same date (adjusted to the last date of month if exceeds, same time).
 * 
 * @param date A {@link Date} object which the result is based on.
 * @returns A {@link Date} object representing the date with the exactly 1 year different from the given date.
 */
function getOneYearEarlierDate(date: Date): Date {
  const previousYear = date.getFullYear() - 1;
  const thisMonth = date.getMonth() + 1;
  const daysInPreviousYearThisMonth = getDaysInMonth(previousYear, thisMonth);
  return new Date(
    previousYear,
    thisMonth,
    Math.min(daysInPreviousYearThisMonth, date.getDate() + 1));
}

/**
 * Get a {@link Date} object representing the moment which is exactly 1 month earlier relative to the given date.
 * 
 * @param date A {@link Date} object to be based on.
 * @returns A {@link Date} object representing the moment which is exactly 1 month earlier.
 */
function getOneMonthEarlierDate(date: Date): Date {
  const previousMonth = (date.getMonth() + 1) - 1;
  let previousMonthYear = date.getFullYear();
  if (previousMonth === 12) {
    previousMonthYear -= 1;
  }
  const daysInPreviousMonth = getDaysInMonth(
    previousMonthYear,
    previousMonth);
  const oneMonthEarlierDate = new Date(date.toISOString());
  oneMonthEarlierDate.setDate(oneMonthEarlierDate.getDate() - daysInPreviousMonth);
  return oneMonthEarlierDate;
}

/**
 * Gets an array which contains [year, month, date of month] from an ISO format string retrieved from the server.
 * 
 * @param isoFormatString A string in ISO format, representing the date, retrieved from the server.
 * @returns An array with `[year, month, dateOfMonth]` format.
 */
function getDateFromISOString(isoFormatString: string): [number, number, number] {
  const dateAsString = isoFormatString.split("T")[0];
  return dateAsString.split("-").map(value => parseInt(value)) as [number, number, number];
}

/**
 * Get the string in ISO format which represents a date from an array containing a date information.
 * 
 * @param date An array containing [year, month, date].
 * @returns The ISO format string reprenseting the date.
 */
function getISOStringFromDate(date: [number, number, number]): string {
  const [year, month, dateOfMonth] = date;
  const monthAsString = month.toString().padStart(2, "0");
  const dateOfMonthAsString = dateOfMonth.toString().padStart(2, "0");
  return `${year}/${monthAsString}/${dateOfMonthAsString}`;
}

export function useDateTimeUtility() {
  return {
    getDateTimeISOString,
    getDateISOString,
    getTimeISOString,
    getHTMLDateTimeInputString,
    getCurrentDateTimeHTMLInputString,
    getHTMLDateInputString,
    getCurrentDateHTMLInputString,
    getHTMLTimeInputString,
    getCurrentTimeHTMLInputString,
    getDisplayDateTimeString,
    getDisplayDateString,
    getDisplayTimeString,
    compareDates,
    compareDateTimes,
    getDeltaTextRelativeToNow,
    getDateFromISOString,
    getISOStringFromDate
  };
}