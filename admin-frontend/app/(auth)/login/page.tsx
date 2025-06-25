"use client";

import React from "react";
import { useLoginAsync } from "../../../services/services.auth";
import { useRouter } from "next/navigation";
import useUserSessionStore from "../../../store/useUserSessionStore";
import Form from "../../../components/UI/form";
import { toast } from "sonner";
import { AdminResponse, LoginRequest } from "../../../types/auth.types";

const LoginPage = () => {
  const router = useRouter();
  const setCurrentUser = useUserSessionStore((state) => state.setCurrentUser);
  const { trigger: login } = useLoginAsync();

  const handleLogin = async (data: LoginRequest) => {
    if (!data.email?.trim() || !data.password?.trim()) {
      toast("You need to fill all the fields.");
      return;
    }

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
    <main>
      <Form
        onSubmit={handleLogin}
        titleText="Welcome back to Hello!"
        fields={[
          {
            name: "email",
            label: "Email:",
            type: "email",
            placeholder: "Enter your email",
          },
          {
            name: "password",
            label: "Password:",
            type: "password",
            placeholder: "Enter your password",
          },
        ]}
        additionalStyling="md:min-w-[400px]"
        buttonLabel="Log In"
      />
    </main>
  );
};

export default LoginPage;
