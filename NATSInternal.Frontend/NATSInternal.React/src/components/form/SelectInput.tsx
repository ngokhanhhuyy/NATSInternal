import { useTsxHelper } from "@/helpers";

// Child component.
import Input from "./Input";
import { Menu, MenuButton, MenuItem, MenuItems } from "@headlessui/react";
import { CheckIcon } from "@heroicons/react/24/outline";

// Props.
export type SelectInputOption = {
  value: string;
  displayName?: string;
};

export type SelectInputProps = {
  options: SelectInputOption[];
  value: string;
  onValueChanged(newValue: string): any;
  disabled?: boolean;
} & Omit<React.ComponentPropsWithoutRef<"div">, "children">;

// Component.
export default function SelectInput(props: SelectInputProps): React.ReactNode {
  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // Template.
  function renderInput(className?: string, path?: string) {
    return (
      <Menu>
        <input type="hidden" name={path} id={props.id} />

        <MenuButton className={joinClassName(
          className,
          props.className,
          "form-control text-start hover:cursor-pointer",
          "data-open:border-blue-500 data-open:outline-blue-500"
        )}>
          {props.options.find(option => option.value == props.value)?.displayName}
        </MenuButton>

        <MenuItems
          className={joinClassName(
            "bg-white/50 dark:bg-neutral-800/65 border border-black/25 dark:border-white/25 outline-none",
            "rounded-lg shadow-lg p-1.5 backdrop-blur-md origin-center",
            "w-(--button-width) [--anchor-gap:--spacing(1.5)] max-h-100 overflow-y-auto",
            "origin-center transition duration-200 ease-out data-closed:scale-95 data-closed:opacity-0"
          )}
          anchor="bottom start"
          modal={false}
          transition
        >
          {props.options.map((option, index) => (
            <MenuItem key={index}>
              <div
                className={joinClassName(
                  "flex items-center gap-2 cursor-pointer px-3 py-1 rounded-lg",
                  "data-focus:bg-blue-500 data-focus:text-white",
                  option.value !== props.value && "ps-9"
                )}
                onClick={() => props.onValueChanged(option.value)}
              >
                {option.value === props.value && (<CheckIcon className="size-4" />)}
                <span>{option.displayName ?? option.value}</span>
              </div>
            </MenuItem>
          ))}
        </MenuItems>
      </Menu>
    );
  }

  return <Input render={renderInput} />;
}
