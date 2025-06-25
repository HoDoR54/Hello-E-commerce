import { create } from "zustand";
import { UserResponse } from "../types/auth.types";

interface UserSessionState {
  currentUser: UserResponse | null;
  setCurrentUser: (payload: UserResponse) => void;
  clearCurrentUser: () => void;
  hasHydrated: boolean;
  setHasHydrated: (payload: boolean) => void;
}

const useUserSessionStore = create<UserSessionState>((set) => ({
  currentUser: null,
  setCurrentUser: (payload) => set({ currentUser: payload }),
  clearCurrentUser: () => set({ currentUser: null }),
  hasHydrated: false,
  setHasHydrated: (payload: boolean) => set({ hasHydrated: payload }),
}));

export default useUserSessionStore;
