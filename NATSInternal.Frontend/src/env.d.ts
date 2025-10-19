/// <reference types="@rsbuild/core/types" />

import * as Solid from "solid-js";

declare global {
  namespace JSX {
    type Element = Solid.JSX.Element;
    type DivElementProps = Solid.JSX.IntrinsicElements["div"];
    type SpanElementProps = Solid.JSX.IntrinsicElements["span"];
    type InputElementProps = Solid.JSX.IntrinsicElements["input"];
    type TextAreaElementProps = Solid.JSX.IntrinsicElements["textarea"];
    type SelectAreaElementProps = Solid.JSX.IntrinsicElements["select"];
    type FormElementProps = Solid.JSX.IntrinsicElements["form"];
    type CSSProperties = Solid.JSX.CSSProperties;
    interface IntrinsicElements extends Solid.JSX.IntrinsicElements {}
    interface ElementChildrenAttribute extends Solid.JSX.ElementChildrenAttribute {}
  }
}