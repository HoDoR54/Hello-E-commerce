import { create } from "zustand";
import { AdminResponse } from "../types/auth.types";

interface UserSessionState {
  currentUser: AdminResponse | null;
  setCurrentUser: (payload: AdminResponse) => void;
  clearCurrentUser: () => void;
}

const useUserSessionStore = create<UserSessionState>((set) => ({
  currentUser: null,
  setCurrentUser: (payload) => set({ currentUser: payload }),
  clearCurrentUser: () => set({ currentUser: null }),
}));

export default useUserSessionStore;
