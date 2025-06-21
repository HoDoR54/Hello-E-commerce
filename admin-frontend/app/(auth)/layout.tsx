import React, { ReactNode } from "react";
import AuthFooter from "../../components/layout/auth-footer";

interface LoginLayoutProps {
  children: ReactNode;
}

export default function AuthLayout({ children }: LoginLayoutProps) {
  return (
    <section className="flex w-screen h-screen relative flex-col items-center justify-center">
      {children}
      <AuthFooter />
    </section>
  );
}
