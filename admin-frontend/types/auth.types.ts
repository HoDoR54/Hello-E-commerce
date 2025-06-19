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
