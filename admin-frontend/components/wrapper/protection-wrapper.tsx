"use client";

import React, { useEffect, useState, ReactNode } from "react";
import { useRouter, usePathname } from "next/navigation";
import useUserSessionStore from "../../store/useUserSessionStore";
import { ClipLoader } from "react-spinners";

// wrapper for protected routes

interface ProtectionWrapperProps {
  children: ReactNode;
}

const ProtectionWrapper: React.FC<ProtectionWrapperProps> = ({ children }) => {
  const [isLoading, setIsLoading] = useState(true);
  const router = useRouter();
  const pathname = usePathname();

  const currentUser = useUserSessionStore((state) => state.currentUser);
  const hasHydrated = useUserSessionStore((state) => state.hasHydrated);

  useEffect(() => {
    if (!hasHydrated) return;

    if (pathname === "/login") {
      setIsLoading(false);
      return;
    }

    if (!currentUser) {
      router.replace("/login");
    }

    setIsLoading(false);
  }, [pathname, currentUser, hasHydrated, router]);

  if (isLoading || !hasHydrated)
    return <ClipLoader color="#f5f5f5" size={50} />;

  return <>{children}</>;
};

export default ProtectionWrapper;
