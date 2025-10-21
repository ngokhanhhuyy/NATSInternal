import { useState, createContext } from "react";

type Model = { userName: string; roles: string[]; };

type HomePageContext = Readonly<Model> & {
  onRoleAdded(newRole: string): void;
  onRoleReplaced(index: number, replacedRole: string): void;
  onRoleRemoved(index: number, removedRole: string): void;
};

const HomePageContext = createContext<HomePageContext>(null!);

export default function HomePage() {
  const [model, setModel] = useState<Model>(() => ({ userName: "", roles: [] }));

  return (
    <div className="w-100vh h-100vh d-block justify-content-center align-items-center p-3 bg-white">
      <pre>{JSON.stringify(model, null, 2)}</pre>
      <RoleManager
        model={model.roles}
        onRoleAdded={(newRole) => setModel(m => ({ ...m, roles: [...m.roles, newRole] }))}
        onRoleReplaced={(index, replacedRole) => {
          setModel(m => {
            const roles = m.roles.map((role, evaluatingIndex) => {
              if (evaluatingIndex !== index) {
                return role;
              }

              return replacedRole;
            });

            return { ...m, roles };
          });
        }}
        onRoleRemoved={(index) => {
          setModel(m => ({
            ...m,
            roles: m.roles.filter((_, evaluatingIndex) => evaluatingIndex !== index)
          }));
        }}
      />
    </div>
  );
}

type RoleManagerProps = {
  model: string[];
  onRoleAdded(newRole: string): void;
  onRoleReplaced(index: number, replacedRole: string): void;
  onRoleRemoved(index: number, removedRole: string): void;
};

function RoleManager(props: RoleManagerProps) {
  const [roleName, setRoleName] = useState("");

  const addRole = (): void => {
    props.onRoleAdded(roleName);
    setRoleName("");
  };

  return (
    <div>
      <input
        className="form-control mb-3"
        type="text"
        value={roleName}
        onInput={event => setRoleName((event.target as HTMLInputElement).value)}
      />

      <button
        type="button"
        className="btn btn-primary mb-3"
        disabled={!roleName || props.model.includes(roleName)}
        onClick={addRole}
      >
        Add
      </button>

      <ul className="list-group">
        {props.model.map((role, index) => (
          <li className="list-group-item d-flex justify-content-between align-items-center" key={index}>
            {role}
            <button
              type="button"
              className="btn btn-danger"
              onClick={() => props.onRoleRemoved(index, role)}
            >
              Remove
            </button>
          </li>
        ))}
      </ul>
    </div>
  );
}