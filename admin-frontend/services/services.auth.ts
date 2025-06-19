import { Truculenta } from "next/font/google";
import { AdminResponse } from "../types/auth.types";
import { FetchResponse } from "../types/general.types";
import axios from "axios";

export const registerUser = () => {};

export const authenticate = async (): Promise<FetchResponse<AdminResponse>> => {
  const response: AdminResponse = {
    userId: "abc-123",
    email: "admin@example.com",
    adminId: "admin-456",
    name: "Admin User",
    phoneNumber: "+1234567890",
    createdAt: new Date().toISOString(),
    updatedAt: new Date().toISOString(),
    isSuperAdmin: true,
  };

  return {
    ok: false,
    statusCode: 200,
    data: response,
  };
};

// export const loginAdmin = async (): Promise<FetchResponse<AdminResponse>> => {

// }
