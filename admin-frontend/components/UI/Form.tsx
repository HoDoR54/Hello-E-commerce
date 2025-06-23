"use client";

import React, { ReactNode, useState } from "react";
import InputField from "./input-field";
import Link from "next/link";
import Button from "./button";
import { LoginRequest } from "../../types/auth.types";

type InputType = "text" | "email" | "password" | "number";

export interface FieldConfig {
  name: string;
  label: string;
  type: InputType;
  placeholder?: string;
}

interface FormProps {
  titleText: string;
  fields: FieldConfig[];
  onSubmit: (data: LoginRequest) => void;
  additionalStyling?: string;
  link?: string;
  linkLabel?: string;
  buttonLabel: string;
}

const Form: React.FC<FormProps> = ({
  titleText,
  fields,
  onSubmit,
  additionalStyling,
  link,
  linkLabel,
  buttonLabel,
}) => {
  const [formData, setFormData] = useState<Record<string, string>>({});

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const req: LoginRequest = {
      email: formData["email"],
      password: formData["password"],
    };
    onSubmit(req);
  };

  return (
    <form
      onSubmit={handleSubmit}
      className={`rounded-2xl shadow-xs bg-primary-light flex flex-col gap-4 py-4 px-6 bg-gray-100 shadow-black text-gray-950 ${additionalStyling}`}
    >
      <h2 className="text-xl font-semibold w-full flex items-center justify-center">
        {titleText}
      </h2>
      {fields.map((field) => (
        <InputField
          key={field.name}
          {...field}
          value={formData[field.name] || ""}
          onChange={handleChange}
        />
      ))}
      {link && (
        <div className="w-full flex items-center justify-center">
          <Link href={link} className="underline text-blue-800">
            {linkLabel ? linkLabel : "Click here!"}
          </Link>
        </div>
      )}
      <Button
        type="submit"
        className="mt-auto py-2 px-4 rounded-lg hover:opacity-90 transition"
        variant="primary"
      >
        {buttonLabel}
      </Button>
    </form>
  );
};

export default Form;
