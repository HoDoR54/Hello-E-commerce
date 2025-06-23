import type { Metadata } from "next";
import "../styles/globals.css";
import ProtectionWrapper from "../components/wrapper/protection-wrapper";
import { Toaster } from "sonner";

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
        className={`antialiased bg-gray-900 text-gray-100 flex items-center justify-center min-w-screen min-h-screen`}
      >
        <ProtectionWrapper>{children}</ProtectionWrapper>
        <Toaster richColors position="top-center" closeButton={true} />
      </body>
    </html>
  );
}
