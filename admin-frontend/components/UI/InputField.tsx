"use client";

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
  return (
    <div className="flex flex-col gap-1">
      <label htmlFor={name} className="text-gray-950 font-medium">
        {label}
      </label>
      <input
        id={name}
        name={name}
        type={type}
        placeholder={placeholder}
        value={value}
        onChange={onChange}
        className="p-2 rounded border border-gray-300 focus:outline-none focus:ring-2 focus:ring-primary-dark"
      />
    </div>
  );
};

export default InputField;
