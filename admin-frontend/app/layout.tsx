import type { Metadata } from "next";
import "../styles/globals.css";
import ProtectionProvider from "../providers/ProtectionProvider";

export const metadata: Metadata = {
  title: "Hello E-commerce Admin Panel",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body
        className={`antialiased bg-blue-950 text-gray-100 flex items-center justify-center min-w-screen min-h-screen`}
      >
        <ProtectionProvider>{children}</ProtectionProvider>
      </body>
    </html>
  );
}
