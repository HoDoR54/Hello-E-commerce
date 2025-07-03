"use client";

import React from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { useRouter } from "next/navigation";
import { toast } from "sonner";

import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";

import { useLoginAsync } from "../../../services/services.auth";
import useUserSessionStore from "../../../store/useUserSessionStore";

const loginSchema = z.object({
  email: z.string().email("Invalid email"),
  password: z.string().min(1, "Password is required"),
});

type LoginFormValues = z.infer<typeof loginSchema>;

const LoginPage = () => {
  const router = useRouter();
  const setCurrentUser = useUserSessionStore((state) => state.setCurrentUser);
  const { trigger: login } = useLoginAsync();

  const form = useForm<LoginFormValues>({
    resolver: zodResolver(loginSchema),
    defaultValues: {
      email: "",
      password: "",
    },
  });

  const onSubmit = async (data: LoginFormValues) => {
    const response = await login({
      email: data.email.trim(),
      password: data.password,
    });

    if (!response) {
      toast.error("Login failed with no response.");
      return;
    }

    if (!response.ok) {
      toast.error(
        `${response.statusCode}: ${response.errorMessage}` || "Login failed."
      );
      return;
    }

    if (response.data) {
      setCurrentUser(response.data);
      toast.success("Logged in successfully!");
      router.push("/");
    }
  };

  return (
    <main className="min-h-screen flex items-center justify-center">
      <div className="bg-white shadow-md rounded p-8 md:min-w-[400px]">
        <h2 className="text-2xl font-bold mb-6 text-center">
          Welcome back to Hello!
        </h2>

        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
            <FormField
              control={form.control}
              name="email"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Email</FormLabel>
                  <FormControl>
                    <Input
                      placeholder="Enter your email"
                      type="email"
                      {...field}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="password"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Password</FormLabel>
                  <FormControl>
                    <Input
                      placeholder="Enter your password"
                      type="password"
                      {...field}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <Button type="submit" className="w-full">
              Log In
            </Button>
          </form>
        </Form>
      </div>
    </main>
  );
};

export default LoginPage;
