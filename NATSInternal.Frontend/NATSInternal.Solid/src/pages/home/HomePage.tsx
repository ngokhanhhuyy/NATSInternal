import { createSignal, createMemo, createContext, useContext, For, Show, type Context } from "solid-js";
import { createMutable } from "solid-js/store";

function createReactiveObject<T extends object>(target: T): T {
  const result = { };

  for (const [key, value] of Object.entries(target)) {
    if (typeof value === "function") {
      result[key] = target[key];
    }

    Object.defineProperty(result, key, {
      get() {
        return target[key];
      },

      set(newValue) {
        target[key] = newValue;
      }
    });
  }

  return result as T;
}

type Model = { userName: string; roles: string[]; };

type HomePageContext = Readonly<Model> & {
  onRoleAdded(newRole: string): void;
  onRoleReplaced(index: number, replacedRole: string): void;
  onRoleRemoved(index: number, removedRole: string): void;
};

const HomePageContext = createContext<HomePageContext>();

export default function HomePage() {
  const model = createMutable<Model>({ userName: "", roles: [] });

  return (
    <div class="w-100vh h-100vh d-block justify-content-center align-items-center p-3 bg-white">
      <pre>{JSON.stringify(model, null, 2)}</pre>
      <RoleManager
        model={model.roles}
        onRoleAdded={(newRole) => model.roles.push(newRole)}
        onRoleReplaced={(index, replacedRole) => model.roles[index] = replacedRole}
        onRoleRemoved={(index) => model.roles.splice(index, 1)}
      />
    </div>
  );
}

type RoleManagerProps = {
  model: string[];
  onRoleAdded(newRole: string): void;
  onRoleReplaced(index: number, replacedRole: string): void;
  onRoleRemoved(index: number, removedRole: string): void;
}

function RoleManager(props: RoleManagerProps) {
  const state = createMutable({ roleName: "" });

  const addRole = (): void => {
    props.model.push(state.roleName);
    state.roleName = "";
  };

  return (
    <div>
      <input
        class="mb-3 rounded-lg"
        type="text"
        value={state.roleName}
        onInput={event => state.roleName = event.target.value}
      />

      <button
        type="button"
        class="btn btn-primary mb-3"
        disabled={!state.roleName || props.model.includes(state.roleName)}
        onClick={addRole}
      >
        Add
      </button>

      <ul class="list-group">
        <For each={props.model}>
          {(role, getIndex) => (
            <li class="list-group-item d-flex justify-content-between align-items-center">
              {role}
              <button
                type="button"
                class="btn btn-danger"
                onClick={() => props.model.splice(getIndex(), 1)}
              >
                Remove
              </button>
            </li>
          )}
        </For>
      </ul>
    </div>
  );
}