import { createContext } from "svelte";
export type FormFieldContextPayload = {
  isValidated: boolean;
  hasError: boolean;
  path?: string;
  displayName?: string;
};

export const [getFormFieldContext, setFormFieldContext] = createContext<FormFieldContextPayload>();