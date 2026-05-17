import React, { useContext } from "react";
import { joinClassName, compute } from "@/helpers";
import { FormFieldContext } from "@/components/form/FormField";

// Child components.
import { CheckIcon } from "@heroicons/react/24/solid";

// Props.
type CheckBoxInputProps = {
  label?: string;
  isChecked: boolean;
  onInput(isChecked: boolean): any;
  className?: string;
  disabled?: boolean;
};

// Components.
export default function CheckBoxInput(props: CheckBoxInputProps): React.ReactNode {
  // Dependencies.
  const formFieldContext = useContext(FormFieldContext);

  // Computed.
  const className = compute<string | undefined>(() => {
    return joinClassName(
      "border rounded w-4.5 h-4.5 p-0 self-center cursor-pointer transition-color duration-100 p-0.5",
      props.isChecked
        ? (
          "bg-blue-600 dark:bg-blue-500 border-transparent"
        ) : (
          "bg-neutral-900/5 dark:bg-neutral-50/15 " +
          "border-neutral-900/25 dark:border-neutral-50/25 " +
          "hover:border-blue-600 dark:hover:border-blue-500"
        ),
      props.disabled && "pointer-events-none opacity-50",
      props.className
    );
  });

  // Template.
  return (
    <div className="flex gap-1 items-center">
      <input type="hidden" name={formFieldContext?.path} checked={props.isChecked} readOnly />
      <button type="button" className={className} onClick={() => props.onInput(!props.isChecked)}>
        {props.isChecked && (
          <CheckIcon className="stroke-white fill-white" />
        )}
      </button>
      {props.label && <span>{props.label}</span>}
    </div>
  );
}