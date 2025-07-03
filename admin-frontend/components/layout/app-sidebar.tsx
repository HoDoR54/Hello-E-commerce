"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import {
  BarChart2,
  Boxes,
  ClipboardList,
  PackageCheck,
  User2,
  Users,
  Calendar,
  CreditCard,
  Folder,
  ShoppingCart,
  Truck,
  ChevronUp,
} from "lucide-react";

import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarGroup,
  SidebarGroupContent,
  SidebarGroupLabel,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuItem,
  SidebarMenuButton,
  useSidebar,
} from "@/components/ui/sidebar";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { spawn } from "child_process";

const groups = [
  {
    label: "Statistics",
    items: [
      { title: "Sales Stats", icon: BarChart2, href: "/sales-stats" },
      { title: "Product Stats", icon: Boxes, href: "/product-stats" },
      { title: "Customer Logs", icon: ClipboardList, href: "/customer-logs" },
    ],
  },
  {
    label: "Inventory",
    items: [
      { title: "In-Stock Items", icon: PackageCheck, href: "/inventory" },
      { title: "Orders", icon: ShoppingCart, href: "/orders" },
      { title: "Suppliers", icon: Truck, href: "/suppliers" },
      { title: "Categories", icon: Folder, href: "/categories" },
    ],
  },
  {
    label: "Users & Billing",
    items: [
      { title: "Users", icon: Users, href: "/users" },
      { title: "Billing", icon: CreditCard, href: "/billing" },
      { title: "Calendar", icon: Calendar, href: "/calendar" },
    ],
  },
];

export function AppSidebar() {
  const pathname = usePathname();
  const { open } = useSidebar();

  return (
    <Sidebar collapsible="icon">
      <SidebarHeader>
        <div className="flex items-center justify-center p-2">
          <h1 className="text-2xl font-bold tracking-widest">
            {open ? (
              <>
                <span className="text-primary">Hello </span>
                <span className="text-accent">E-commerce</span>
              </>
            ) : (
              <span className=" text-primary">H</span>
            )}
          </h1>
        </div>
      </SidebarHeader>

      <SidebarContent>
        {groups.map(({ label, items }) => (
          <SidebarGroup key={label}>
            <SidebarGroupLabel>{label}</SidebarGroupLabel>
            <SidebarGroupContent>
              <SidebarMenu>
                {items.map(({ title, icon: Icon, href }) => {
                  const isActive = pathname === href;
                  return (
                    <SidebarMenuItem key={title}>
                      <SidebarMenuButton asChild isActive={isActive}>
                        <Link href={href}>
                          <Icon className="w-4 h-4" />
                          <span>{title}</span>
                        </Link>
                      </SidebarMenuButton>
                    </SidebarMenuItem>
                  );
                })}
              </SidebarMenu>
            </SidebarGroupContent>
          </SidebarGroup>
        ))}
      </SidebarContent>

      <SidebarFooter>
        <SidebarMenu>
          <SidebarMenuItem>
            <DropdownMenu>
              <DropdownMenuTrigger asChild>
                <SidebarMenuButton>
                  <User2 className="w-4 h-4" />
                  <span>Username</span>
                  <ChevronUp className="ml-auto w-4 h-4" />
                </SidebarMenuButton>
              </DropdownMenuTrigger>
              <DropdownMenuContent
                side="top"
                align="start"
                className="w-[--radix-popper-anchor-width]"
              >
                <DropdownMenuItem asChild>
                  <Link href="/account">Account</Link>
                </DropdownMenuItem>
                <DropdownMenuItem asChild>
                  <Link href="/billing">Billing</Link>
                </DropdownMenuItem>
                <DropdownMenuItem asChild>
                  <Link href="/logout">Sign out</Link>
                </DropdownMenuItem>
              </DropdownMenuContent>
            </DropdownMenu>
          </SidebarMenuItem>
        </SidebarMenu>
      </SidebarFooter>
    </Sidebar>
  );
}
