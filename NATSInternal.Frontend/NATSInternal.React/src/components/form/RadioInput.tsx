import React, { useContext } from "react";
import { joinClassName, compute } from "@/helpers";
import { FormFieldContext } from "@/components/form/FormField";

// Props.
type RadioInputProps = {
  label?: string;
  isChecked: boolean;
  onInput(isChecked: boolean): any;
  className?: string;
  disabled?: boolean;
};

// Components.
export default function RadioInput(props: RadioInputProps): React.ReactNode {
  // Dependencies.
  const formFieldContext = useContext(FormFieldContext);

  // Computed.
  const className = compute<string | undefined>(() => {
    return joinClassName(
      "border border-black/25 dark:border-white/25",
      "rounded-full w-4.5 h-4.5 p-0.5 self-center cursor-pointer transition-color duration-100 p-0.5",
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
          <div className="bg-blue-700 dark:bg-blue-400 rounded-full size-full" />
        )}
      </button>
      {props.label && <span>{props.label}</span>}
    </div>
  );
}
