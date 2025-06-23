import { AdminResponse, LoginRequest } from "../types/auth.types";
import { FetchResponse } from "../types/general.types";
import helloAxios from "../lib/helloAxios";
import useSWRMutation from "swr/mutation";

const loginAsync = async (url: string, { arg }: { arg: LoginRequest }) => {
  try {
    const res = await helloAxios.post<FetchResponse<AdminResponse>>(url, arg);
    return res.data;
  } catch (error: any) {
    if (error.response?.data) {
      return error.response.data as FetchResponse<AdminResponse>;
    }
    throw error;
  }
};

export const useLoginAsync = () =>
  useSWRMutation("/auth/admins/login", loginAsync);
