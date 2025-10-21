import globals from "globals";
import pluginJs from "@eslint/js";
import tseslint from "typescript-eslint";
import pluginReact from "eslint-plugin-react";
import stylistic from "@stylistic/eslint-plugin";
import { defineConfig } from "eslint/config";

/** @type {import("eslint").Linter.Config[]} */
export default defineConfig([
  { files: ["**/*.{js,mjs,cjs,ts,jsx,tsx}"] },
  { languageOptions: { globals: globals.browser } },
  pluginJs.configs.recommended,
  ...tseslint.configs.recommended,
  pluginReact.configs.flat.recommended,
  {
    plugins: {
      "@stylistic": stylistic
    },
    rules: {
      "@stylistic/semi-style": ["error", "last"],
      "@stylistic/semi": ["error", "always"],
      "semi": "off",
      "no-restricted-imports": [
        "error",
        {
          "patterns": [{
            "regex": "@/(api|helpers|stores)/([a-zA-Z-_]+)",
            "message": "Must import via index.ts when when importing from the outside of the package."
          }]
        }
      ],
      "@typescript-eslint/no-unused-vars": [
        "warn",
        {
          "varsIgnorePattern": "^_",
          "argsIgnorePattern": "^_",
          "ignoreRestSiblings": true
        }
      ],
      "@typescript-eslint/no-explicit-any": "off",
      "@typescript-eslint/no-unsafe-function-type": "off",
      "@typescript-eslint/no-namespace": "off",
      "@typescript-eslint/no-empty-object-type": "off",
      "no-case-declarations": "off",
      "react/react-in-jsx-scope": "off",
      "react/prop-types": "off"
    }
  }
]);