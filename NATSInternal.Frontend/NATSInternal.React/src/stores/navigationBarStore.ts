import { create } from "zustand";

export type NavigationBarStore = {
  isExpanded: boolean;
  isSmallScreen: boolean;
  expand(): void;
  collapse(): void;
  toggle(): void;
};

export const useNavigationBarStore = create<NavigationBarStore>((set, get) => ({
  isExpanded: false,
  isSmallScreen: false,
  expand(): void {
    set({ isExpanded: true });
  },
  collapse(): void {
    set({ isExpanded: false });
  },
  toggle(): void {
    set({ isExpanded: !get().isExpanded });
  },
}));

