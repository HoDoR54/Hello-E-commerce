"use client";

import React, { useState } from "react";
import { Eye, EyeOff } from "lucide-react";

interface InputFieldProps {
  label: string;
  name: string;
  type: string;
  placeholder?: string;
  value: string;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
}

const InputField: React.FC<InputFieldProps> = ({
  label,
  name,
  type,
  placeholder,
  value,
  onChange,
}) => {
  const [showPassword, setShowPassword] = useState(false);
  const isPasswordType = type === "password";

  return (
    <div className="flex flex-col gap-1 relative">
      <label htmlFor={name} className="text-gray-950 font-medium">
        {label}
      </label>
      <div className="relative">
        <input
          id={name}
          name={name}
          type={isPasswordType && showPassword ? "text" : type}
          placeholder={placeholder}
          value={value}
          onChange={onChange}
          className="w-full p-2 pr-10 rounded border border-gray-300 focus:outline-none focus:ring-2 focus:ring-primary-dark"
        />
        {isPasswordType && (
          <button
            type="button"
            onClick={() => setShowPassword((prev) => !prev)}
            className="absolute right-2 top-1/2 transform -translate-y-1/2 text-gray-500 hover:text-gray-700 cursor-pointer"
            tabIndex={-1}
          >
            {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
          </button>
        )}
      </div>
    </div>
  );
};

export default InputField;
