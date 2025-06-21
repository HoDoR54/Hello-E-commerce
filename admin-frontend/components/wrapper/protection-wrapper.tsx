"use client";

import React, { useEffect, useState, ReactNode } from "react";
import { useRouter, usePathname } from "next/navigation";
import { FetchResponse } from "../../types/general.types";
import { AdminResponse } from "../../types/auth.types";

interface ProtectionWrapperProps {
  children: ReactNode;
}

const ProtectionWrapper: React.FC<ProtectionWrapperProps> = ({ children }) => {
  const [isLoading, setIsLoading] = useState(true);
  const [isLoggedIn, setIsLoggedIn] = useState<boolean | null>(null);
  const router = useRouter();
  const pathname = usePathname();

  useEffect(() => {
    let isMounted = true;

    const checkAuth = async () => {
      if (pathname === "/login") {
        if (isMounted) {
          setIsLoading(false);
          setIsLoggedIn(false);
        }
        return;
      }

      try {
        const result = localStorage.getItem("isLoggedIn");
        if (result && JSON.parse(result) === true) {
          setIsLoggedIn(true);
        } else {
          router.replace("/login");
        }
      } catch (error) {
        console.error("Authentication failed:", error);
        if (isMounted) {
          router.replace("/login");
          setIsLoading(false);
        }
      }
    };

    checkAuth();

    return () => {
      isMounted = false;
    };
  }, [pathname, router]);

  if (isLoading) return <div>Loading...</div>;

  if (pathname === "/login") {
    return <>{children}</>;
  }

  return <>{isLoggedIn ? children : null}</>;
};

export default ProtectionWrapper;
