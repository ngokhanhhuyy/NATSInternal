import { createContext, createMemo, useContext, Show } from "solid-js";
import { useHTMLHelper } from "@/helpers";

// Shared components.
import { FormContext } from "@/components/form/Form";

// Context.
export type FormFieldContextPayload = { isValidated: boolean; hasError: boolean; };
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
  const htmlHelper = useHTMLHelper();

  // Computed states.
  function computeErrorMessage(): string | undefined {
    console.log("Triggered");
    if (formContext && props.propertyPath) {
      const messages = formContext.getErrorCollection().details
        .filter(d => d.propertyPath === props.propertyPath)
        .map(d => d.message);

      return messages.length >= 1 ? messages[0] : undefined;
    }
  }

  function computeHasErrorMessage(): boolean {
    return !!computeErrorMessage();
  }

  return (
    <div class={htmlHelper.joinClassName("form-group", props.class)}>
      <pre>{JSON.stringify(formContext?.getErrorCollection().details, null, 2)}</pre>
      {/* Label */}
      <Show when={props.label}>
        <label class="form-label">{props.label}</label>
      </Show>

      {/* Input */}
      <FormFieldContext.Provider value={{
        isValidated: !!formContext?.getErrorCollection?.().isValidated,
        hasError: computeHasErrorMessage()
      }}>
        {props.children}
      </FormFieldContext.Provider>

      {/* Message */}
      <Show when={computeHasErrorMessage()}>
        <span class="text-danger">{computeErrorMessage()}</span>
      </Show>
    </div>
  );
}