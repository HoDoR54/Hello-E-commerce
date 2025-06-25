export type FetchResponse<T> = {
  ok: boolean;
  statusCode: number;
  errorMessage?: string;
  data?: T;
};

export enum UserRole {
  Admin = "Admin",
  Customer = "Customer",
}
