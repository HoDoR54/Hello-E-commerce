import React, { ReactNode } from "react";
import AuthFooter from "../../components/layout/auth-footer";
import "@/styles/globals.css";
import { Toaster } from "sonner";

interface LoginLayoutProps {
  children: ReactNode;
}

export default function AuthLayout({ children }: LoginLayoutProps) {
  return (
    <html lang="en">
      <body className="min-h-screen bg-black">
        <section className="flex w-screen h-screen relative flex-col items-center justify-center">
          {children}
          <AuthFooter />
        </section>
        <Toaster richColors position="top-center" closeButton={true} />
      </body>
    </html>
  );
}
