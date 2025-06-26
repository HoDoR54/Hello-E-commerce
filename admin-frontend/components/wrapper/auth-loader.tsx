"use client";

import React, { useEffect, useState, ReactNode } from "react";
import { useRouter, usePathname } from "next/navigation";
import { ClipLoader } from "react-spinners";
import {
  useAuthenticateAsync,
  useRefreshAsync,
} from "../../services/services.auth";
import useUserSessionStore from "../../store/useUserSessionStore";

interface AuthLoaderProps {
  children: ReactNode;
}

const AuthLoader: React.FC<AuthLoaderProps> = ({ children }) => {
  const [isLoading, setIsLoading] = useState(true);
  const router = useRouter();
  const pathname = usePathname();
  const { trigger: authenticate } = useAuthenticateAsync();
  const { trigger: refresh } = useRefreshAsync();
  const setCurrentUser = useUserSessionStore((state) => state.setCurrentUser);

  const publicRoutes = ["/login", "/register"];

  useEffect(() => {
    const loadAuth = async () => {
      if (publicRoutes.includes(pathname)) {
        setIsLoading(false);
        return;
      }

      try {
        let authResult = await authenticate();

        if (!authResult?.ok) {
          const refreshResult = await refresh();

          if (refreshResult === true) {
            authResult = await authenticate();
          }
        }

        if (authResult?.ok && authResult?.data) {
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

    loadAuth();
  }, [pathname, authenticate, refresh, setCurrentUser, router]);

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
