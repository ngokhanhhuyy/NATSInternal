import globals from "globals";
import pluginJs from "@eslint/js";
import tseslint from "typescript-eslint";
import stylistic from "@stylistic/eslint-plugin";
// import pluginVue from "eslint-plugin-vue";
import vueParser from "vue-eslint-parser";
import { defineConfig } from "eslint/config";

/** @type {import("eslint").Linter.FlatConfig[]} */
export default defineConfig([
  { files: ["**/*.{js,mjs,cjs,ts,jsx,tsx,vue}"] },
  { languageOptions: { globals: globals.browser } },
  pluginJs.configs.recommended,
  ...tseslint.configs.recommended,
  // ...pluginVue.configs["flat/recommended"],
  {
    files: ["**/*.{ts,tsx,d.ts,vue}"],
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
        "warn",
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
      "vue/multi-word-component-names": "off",
      "vue/v-bind-style": "off"
    },
  },
  {
    files: ["**/*.vue"],
    languageOptions: {
      parser: vueParser,
      parserOptions: {
        parser: {
          ts: tseslint.parser,
          tsx: tseslint.parser,
          js: tseslint.parser,
          jsx: tseslint.parser,
        },
        ecmaVersion: "latest",
        sourceType: "module",
        extraFileExtensions: [".vue"],
        ecmaFeatures: {
          jsx: true,
        },
      },
    },
  }
]);
