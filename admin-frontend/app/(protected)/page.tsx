"use client";

import { SidebarProvider } from "@/components/ui/sidebar";
import useUserSessionStore from "../../store/useUserSessionStore";

export default function Home() {
  const currentUser = useUserSessionStore((state) => state.currentUser);

  return (
    <section className="flex min-h-screen items-center justify-center"></section>
  );
}
