import type { Metadata } from "next";
import "../../styles/globals.css";
import { Toaster } from "sonner";
import AuthLoader from "../../components/wrapper/auth-loader";
import { SidebarProvider, SidebarTrigger } from "@/components/ui/sidebar";
import { AppSidebar } from "@/components/layout/app-sidebar";

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
      <body className="min-h-screen bg-black">
        <AuthLoader>
          <SidebarProvider defaultOpen={true}>
            <AppSidebar />
            <main>
              <SidebarTrigger />
              {children}
            </main>
          </SidebarProvider>
        </AuthLoader>
        <Toaster richColors position="top-center" closeButton={true} />
      </body>
    </html>
  );
}
