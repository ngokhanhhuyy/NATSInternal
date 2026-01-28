import { useTsxHelper } from "@/helpers";

// Child component.
import Input from "./Input";
import { Menu, MenuButton, MenuItem, MenuItems } from "@headlessui/react";

// Props.
type SelectInputOption = {
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
          "data-active:border-blue-500 data-active:outline-blue-500")}
        >
          {props.options.find(option => option.value == props.value)?.displayName}
        </MenuButton>

        <MenuItems
          className={joinClassName(
            "bg-white/50 dark:bg-neutral-800/50 border border-black/15  dark:border-white/15",
            "rounded-lg shadow-lg p-1.5 backdrop-blur-md scale-y-100",
            "w-(--button-width) [--anchor-gap:--spacing(1.5)]",
            "origin-top transition duration-200 ease-out data-closed:scale-y-0 data-closed:opacity-0"
          )}
          anchor="bottom start"
          modal={false}
          transition>
          {props.options.map((option, index) => (
            <MenuItem key={index}>
              <div
                className="data-focus:bg-blue-500 data-focus:text-white cursor-pointer px-3 py-1 rounded-lg"
                onClick={() => props.onValueChanged(option.value)}>
                {option.displayName ?? option.value}
              </div>
            </MenuItem>
          ))}
        </MenuItems>
      </Menu>
    );
  }

  return <Input render={renderInput} />;
}
