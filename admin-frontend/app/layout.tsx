import type { Metadata } from "next";
import "../styles/globals.css";
import ProtectionWrapper from "../components/wrapper/protection-wrapper";

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
      </body>
    </html>
  );
}
