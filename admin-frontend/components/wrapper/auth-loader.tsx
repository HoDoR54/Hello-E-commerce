"use client";

import React, { useEffect, useState, ReactNode } from "react";
import { useRouter, usePathname } from "next/navigation";
import { ClipLoader } from "react-spinners";
import { useAuthenticateAsync } from "../../services/services.auth";
import { UserResponse } from "../../types/auth.types";
import useUserSessionStore from "../../store/useUserSessionStore";

interface AuthLoaderProps {
  children: ReactNode;
}

const AuthLoader: React.FC<AuthLoaderProps> = ({ children }) => {
  const [isLoading, setIsLoading] = useState(true);
  const router = useRouter();
  const pathname = usePathname();
  const { trigger: authenticate } = useAuthenticateAsync();
  const setCurrentUser = useUserSessionStore((state) => state.setCurrentUser);

  useEffect(() => {
    if (pathname === "/login") {
      setIsLoading(false);
      return;
    }

    const authenticateToken = async () => {
      try {
        const authResult = await authenticate();

        if (authResult?.data) {
          setCurrentUser(authResult.data);
        } else {
          router.replace("/login");
        }
      } catch (error) {
        router.replace("/login");
      } finally {
        setIsLoading(false);
      }
    };

    authenticateToken();
  }, [pathname, router, authenticate, setCurrentUser]);

  if (isLoading) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <ClipLoader color="#f5f5f5" size={50} />
      </div>
    );
  }

  return <>{children}</>;
};

export default AuthLoader;
