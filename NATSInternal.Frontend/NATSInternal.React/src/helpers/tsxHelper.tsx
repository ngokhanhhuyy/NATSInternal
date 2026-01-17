import React from "react";

export type TsxHelper = {
  compute<T>(computer: () => T): T;
  joinClassName(...classNames: (string | undefined | null | false)[]): string | undefined;
  Switch: typeof Switch;
  Case: typeof Case;
  Fallback: typeof Fallback;
  Show: typeof Show;
  True: typeof True;
  False: typeof False;
};

type ShowProps = { children: React.ReactNode | React.ReactNode[] };

function Show(props: ShowProps): React.ReactNode {
  // Template.
  const childrenAsArray = React.Children.toArray(props.children);
  if (childrenAsArray.length < 1 || childrenAsArray.length > 2) {
    throw new Error("<Show> element can only contain 1 or 2 elements.");
  }

  if (childrenAsArray.length == 1) {
    return childrenAsArray[0];
  }

  let trueWhen: any = null;
  let trueElement, falseElement: React.ReactNode | null = null;

  for (let index = 0; index < childrenAsArray.length; index ++) {
    const child = childrenAsArray[index];
    if (!React.isValidElement(child)) {
      throw new Error(`Invalid element at ${index} position.`);
    }

    if (child.type == True) {
      if (trueElement != null) {
        throw new Error("<Show> element can contain only 1 <True> element.");
      }

      const trueProps = child.props as TrueProps<any>;
      trueWhen = trueProps.when;
      trueElement = trueProps.render(trueProps.when as Truthy<any>);
    }

    if (child.type == False) {
      if (falseElement != null) {
        throw new Error("<Show> element can contain only 1 <False> element.");
      }

      const falseProps = child.props as FalseProps;
      falseElement = falseProps.render();
    }
  }

  return trueWhen ? trueElement : falseElement;
}

type TrueProps<T> = {
  when: T;
  render(value: Truthy<T>): React.ReactNode;
};

function True<T>(props: TrueProps<T>): React.ReactNode {
  // Template.
  return props.render(props.when as Truthy<T>);
}

type FalseProps = { render: () => React.ReactNode; };

function False(props: { render: () => React.ReactNode }): React.ReactNode {
  // Template.
  return props.render();
}

type SwitchProps = {
  children: React.ReactNode[];
};

function Switch(props: SwitchProps): React.ReactNode {
  let fallback: (() => React.ReactNode | React.ReactNode[]) | React.ReactNode | React.ReactNode[] | null = null;

  for (const child of React.Children.toArray(props.children)) {
    if (!React.isValidElement(child)) {
      continue;
    }

    if (child.type === Case) {
      const { when, children: caseChildren, render: renderCase } = child.props as CaseProps<any>;

      if (when) {
        return <>{caseChildren ?? renderCase?.(when as Truthy<any>)}</>;
      }
    }

    if (child.type === Fallback) {
      fallback = (child.props as FallbackProps).children;
    }
  }

  if (fallback == null) {
    return null;
  }

  if (typeof fallback == "function") {
    return <>{(fallback as (() => React.ReactNode | React.ReactNode[]))()}</>;
  }

  return fallback;
}

type CaseProps<T> = {
  when: T;
  children?: React.ReactNode | React.ReactNode[];
  render?: (value: Truthy<T>) => React.ReactNode | React.ReactNode[];
};

function Case<T>(props: CaseProps<T>): React.ReactNode {
  // Template.
  return (
    <>
      {props.children}
    </>
  );
}

type FallbackProps = {
  children?: React.ReactNode | React.ReactNode[];
  render?: () => React.ReactNode | React.ReactNode[];
};

function Fallback(props: FallbackProps): React.ReactNode {
  // Template.
  return (
    <>
      {props.children}
    </>
  );
}

const helper: TsxHelper = {
  compute<T>(computer: () => T): T {
    return computer();
  },
  joinClassName: (...classNames) => {
    return classNames.filter(name => name).join(" ") || undefined;
  },
  Switch,
  Case,
  Fallback,
  Show,
  True,
  False
};

export function useTsxHelper(): TsxHelper {
  return helper;
}