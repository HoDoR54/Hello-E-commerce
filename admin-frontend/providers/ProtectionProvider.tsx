"use client";

import React, { useEffect, useState, ReactNode } from "react";
import { useRouter, usePathname } from "next/navigation";
import { authenticate } from "../services/services.auth";
import { FetchResponse } from "../types/general.types";
import { AdminResponse } from "../types/auth.types";

interface ProtectionProviderProps {
  children: ReactNode;
}

const ProtectionProvider: React.FC<ProtectionProviderProps> = ({
  children,
}) => {
  const [isLoading, setIsLoading] = useState(true);
  const [isLoggedIn, setIsLoggedIn] = useState(false);
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
        const result: FetchResponse<AdminResponse> = await authenticate();
        if (isMounted) {
          if (result.ok) {
            setIsLoggedIn(true);
          } else {
            router.replace("/login");
            console.log("redirected.");
          }
          setIsLoading(false);
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

export default ProtectionProvider;
