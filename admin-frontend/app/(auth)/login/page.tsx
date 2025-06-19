"use client";

import React from "react";
import Form from "../../../components/UI/Form";

const handleLogin = async () => {};

const LoginPage = () => {
  return (
    <main>
      <Form
        onSubmit={() => {}}
        titleText="Welcome back to Hello!"
        fields={[
          {
            name: "E-mail",
            label: "E-mail:",
            type: "email",
            placeholder: "Enter your email",
          },
          {
            name: "Password",
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
