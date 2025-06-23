import { create } from "zustand";
import { AdminResponse } from "../types/auth.types";
import { createJSONStorage, persist } from "zustand/middleware";

interface UserSessionState {
  currentUser: AdminResponse | null;
  setCurrentUser: (payload: AdminResponse) => void;
  clearCurrentUser: () => void;
  hasHydrated: boolean;
  setHasHydrated: (payload: boolean) => void;
}

const useUserSessionStore = create<UserSessionState>()(
  persist(
    (set) => ({
      currentUser: null,
      setCurrentUser: (payload) => set({ currentUser: payload }),
      clearCurrentUser: () => set({ currentUser: null }),
      hasHydrated: false,
      setHasHydrated: (payload: boolean) => set({ hasHydrated: payload }),
    }),
    {
      name: "user-session",
      storage: createJSONStorage(() => localStorage),
      onRehydrateStorage: () => (state) => {
        state?.setHasHydrated(true);
      },
    }
  )
);

export default useUserSessionStore;
