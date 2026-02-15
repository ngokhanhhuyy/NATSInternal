/// <reference types="@rsbuild/core/types" />
/// <reference types="solid-labels" />

import * as Solid from "solid-js";

declare global {
  namespace JSX {
    type Element = Solid.JSX.Element;
    type HTMLDivAttributes = Solid.JSX.IntrinsicElements["div"];
    type HTMLSpanAttributes = Solid.JSX.IntrinsicElements["span"];
    type HTMLInputAttributes = Solid.JSX.IntrinsicElements["input"];
    type HTMLTextAreaAttributes = Solid.JSX.IntrinsicElements["textarea"];
    type HTMLSelectAttributes = Solid.JSX.IntrinsicElements["select"];
    type HTMLFormAttributes = Solid.JSX.IntrinsicElements["form"];
    type HTMLButtonAttributes = Solid.JSX.IntrinsicElements["button"];
    type CSSProperties = Solid.JSX.CSSProperties;
    
    interface IntrinsicElements extends Solid.JSX.IntrinsicElements {
    }

    interface ElementChildrenAttribute extends Solid.JSX.ElementChildrenAttribute {}
  }
}


type Props<T> = T extends (props: infer P) => Solid.JSX.Element ? P : never;