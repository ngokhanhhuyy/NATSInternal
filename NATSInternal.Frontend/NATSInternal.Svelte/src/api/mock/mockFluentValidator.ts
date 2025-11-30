import type { ApiErrorDetails } from "../errors";

type ArrayElement<T> = T extends (infer E)[] ? E : never;
type Enumerable = string | any[];

interface IRuleBuilderOptions {
  notNullOrUndefined(): this;
  withMessage(message: string): this;
  withName(name: string): this;
}

interface IEnumerableRuleBuilderOptions<P extends string | any[]> extends IRuleBuilderOptions {
  notEmpty(): this;
  minimumLength(length: number): this;
  maximumLength(length: number): this;
  length(len: number): this;
  contains(element: ArrayElement<P>): this;
}

type RuleBuilder = {
  <R, P>(getProperty: (requestDto: R) => P): IRuleBuilderOptions;
  <R, P extends Enumerable>(getProperty: (requestDto: R) => P): IEnumerableRuleBuilderOptions<P>;
};

type RuleInformation = {
  propertyPath: string;
  displayName: string;
  validators: (() => void)[];
};

export function createValidator<R>(requestDto: R, ruleFor: (builder: RuleBuilder) => object) {
  const ruleInformationList: RuleInformation[] = [];
  const errors: ApiErrorDetails = {};
  const builder = <P>(getProperty: (dto: R) => P) => {
    const property = getProperty(requestDto);
    if (typeof requestDto === "string" || Array.isArray(property)) {
      return;
    }
  };
}

function createRuleBuilderOptions<R, P>(
  requestDto: R,
  property: P,
  ruleInformationList: RuleInformation[],
  errors: ApiErrorDetails
): IRuleBuilderOptions<R, P> {
  return {
    notNullOrUndefined(): this {}
  };
}
