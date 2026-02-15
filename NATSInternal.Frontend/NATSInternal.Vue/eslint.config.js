// @ts-check
import globals from "globals";
import pluginJs from "@eslint/js";
import tseslint from "typescript-eslint";
import stylistic from "@stylistic/eslint-plugin";
import pluginVue from "eslint-plugin-vue";
import { defineConfig } from "eslint/config";

/** @type {import("eslint").Linter.FlatConfig[]} */
export default defineConfig([
  { files: ["**/*.{js,mjs,cjs,ts,jsx,tsx,vue}"] },
  { languageOptions: { globals: globals.browser } },
  pluginJs.configs.recommended,
  ...tseslint.configs.recommended,
  ...pluginVue.configs["flat/recommended"],
  {
    files: ["**/*.{ts,tsx,d.ts}"],
    rules: {
      "no-undef": "off",
      "no-unused-vars": "off",
    },
  },
  {
    plugins: {
      "@stylistic": stylistic,
    },
    rules: {
      "@stylistic/semi-style": ["error", "last"],
      "@stylistic/semi": ["error", "always"],
      semi: "off",

      "no-restricted-imports": [
        "error",
        {
          patterns: [
            {
              regex: "@/(api|helpers|stores)/([a-zA-Z-_]+)",
              message:
                "Must import via index.ts when when importing from the outside of the package.",
            },
          ],
        },
      ],

      "@typescript-eslint/no-unused-vars": [
        "error",
        {
          varsIgnorePattern: "^_",
          argsIgnorePattern: "^_",
          ignoreRestSiblings: true,
        },
      ],

      "@typescript-eslint/no-explicit-any": "off",
      "@typescript-eslint/no-unsafe-function-type": "off",
      "@typescript-eslint/no-namespace": "off",
      "@typescript-eslint/no-empty-object-type": "off",
    },
  },
]);
