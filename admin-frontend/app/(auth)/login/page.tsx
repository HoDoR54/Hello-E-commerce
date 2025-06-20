"use client";

import React from "react";
import Form from "../../../components/UI/Form";
import { loginAsync } from "../../../services/services.auth";

const handleLogin = async (data: Record<string, string>) => {
  try {
    const loginRequest = {
      email: data.email,
      password: data.password,
    };
    const result = await loginAsync("/auth/admins/login", {
      arg: loginRequest,
    });
    console.log("Login success:", result);
  } catch (error) {
    console.error("Login failed:", error);
  }
};
const LoginPage = () => {
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
        additionalStyling="md:min-w-[300px]"
        buttonLabel="Log In"
      />
    </main>
  );
};

export default LoginPage;
