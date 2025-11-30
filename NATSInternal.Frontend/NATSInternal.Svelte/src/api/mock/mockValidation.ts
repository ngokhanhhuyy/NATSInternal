import * as v from "valibot";
import { ValidationError } from "../errors";

const formatExpected = (expected: string): string => {
  return expected.replace(/[^a-zA-Z0-9]/g, "");
};

v.setSpecificMessage(v.minLength, (issue) => {
  const elementDisplayName = typeof issue.input === "string" ? "ký tự" : "phần tử";
  const minLength = parseInt(formatExpected(issue.expected));
  console.log(minLength);
  if (minLength === 1) {
    return "Không được để trống.";
  }

  return `Phải chứa ít nhất ${minLength} ${elementDisplayName}.`;
});

export function validateUsingSchema<
  TSchema extends v.BaseSchema<TInput, TOutput, TIssue>,
  TInput,
  TOutput,
  TIssue extends v.BaseIssue<TOutput>
>(schema: TSchema, input: TInput): void {
  const parsingResult = v.safeParse(schema, input);
  if (!parsingResult.success) {
    const mappedErrors: { [key: string]: string } = {};
    for (const issue of parsingResult.issues) {
      console.log(issue.kind);
      const dottedPath = v.getDotPath(issue) ?? "";
      mappedErrors[dottedPath] = issue.message;
    }

    throw new ValidationError(mappedErrors);
  }
}
