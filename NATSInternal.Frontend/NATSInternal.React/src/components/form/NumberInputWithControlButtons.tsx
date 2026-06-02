import React from "react";
import { joinClassName } from "@/helpers";

// Child components.
import NumberInput from "./NumberInput";
import { MinusIcon, PlusIcon } from "@heroicons/react/24/outline";

// Props.
type NumberInputWithControlButtonsProps = {
  step?: number;
  inputClassName?: string;
} & React.ComponentProps<typeof NumberInput>;

// Components.
export default function NumberInputWithControlButtons(props: NumberInputWithControlButtonsProps): React.ReactNode {
  // Props.
  const { className, step = 1, ...domProps } = props;

  // Template.
  return (
    <div className={joinClassName("grid grid-cols-[auto_1fr_auto]", props.className)}>
      <button
        type="button"
        className="btn border-e-0 rounded-e-none"
        onClick={() => domProps.onValueChanged(domProps.value - step)}
        disabled={domProps.min != null && domProps.value - step < domProps.min}
      >
        <MinusIcon />
      </button>

      <NumberInput
        {...domProps}
        className={joinClassName("rounded-none z-1 text-center", props.inputClassName)}
      />

      <button
        type="button"
        className="btn border-s-0 rounded-s-none"
        onClick={() => domProps.onValueChanged(domProps.value + step)}
        disabled={domProps.max != null && domProps.value + step > domProps.max}
      >
        <PlusIcon />
      </button>
    </div>
  );
}
