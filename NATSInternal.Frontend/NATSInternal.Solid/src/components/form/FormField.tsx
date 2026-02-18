import { createContext, useContext, createMemos, Show } from "@/solid";
import { useTsxHelper } from "@/helpers";

// Shared components.
import { FormContext } from "@/components/form/Form";

// Context.
export type FormFieldContextPayload = Readonly<{ isValidated: boolean; hasError: boolean; }>;
export const FormFieldContext = createContext<FormFieldContextPayload>();

// Props.
export type FormFieldProps = {
  label?: string;
  propertyPath?: string;
  children: JSX.Element;
} & JSX.IntrinsicElements["div"];

// Components.
export default function FormField(props: FormFieldProps) {
  // Dependencies.
  const formContext = useContext(FormContext);
  const { joinClassName } = useTsxHelper();

  // Memos.
  const memos = createMemos({
    errorMessage: (): string | undefined => {
      if (formContext && props.propertyPath) {
        const messages = formContext.errorCollection.details
          .filter(d => d.propertyPath === props.propertyPath)
          .map(d => d.message);

        return messages.length >= 1 ? messages[0] : undefined;
      }
    },
    hasErrorMessage: () => !!memos.errorMessage
  });

  // Payload.
  const payload: FormFieldContextPayload = {
    get isValidated() {
      return !!formContext?.errorCollection?.isValidated;
    },
    get hasError() {
      return memos.hasErrorMessage;
    }
  };

  return (
    <div class={joinClassName("form-group", props.class)}>
      <pre>{JSON.stringify(formContext?.errorCollection.details, null, 2)}</pre>
      {/* Label */}
      <Show when={props.label}>
        <label class="form-label">{props.label}</label>
      </Show>

      {/* Input */}
      <FormFieldContext.Provider value={payload}>
        {props.children}
      </FormFieldContext.Provider>

      {/* Message */}
      <Show when={memos.hasErrorMessage}>
        <span class="text-danger">{memos.errorMessage}</span>
      </Show>
    </div>
  );
}