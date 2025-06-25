import { UserRole } from "./general.types";

export type LoginRequest = {
  email: string;
  password: string;
};

export type AdminResponse = {
  userId: string;
  email: string;
  adminId: string;
  name: string;
  phoneNumber: string;
  isSuperAdmin: boolean;
  createdAt: string;
  updatedAt: string;
};

export interface UserResponse {
  id: string;
  email: string;
  role: UserRole;

  isWarned: boolean;
  warningLevel?: number | null;

  isBanned: boolean;
  bannedDays?: number | null;

  createdAt: string;
  updatedAt: string;
}
