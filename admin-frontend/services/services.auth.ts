import { AdminResponse, LoginRequest, UserResponse } from "../types/auth.types";
import { FetchResponse } from "../types/general.types";
import helloAxios from "../lib/helloAxios";
import useSWRMutation from "swr/mutation";

const loginAsync = async (url: string, { arg }: { arg: LoginRequest }) => {
  try {
    const res = await helloAxios.post<FetchResponse<UserResponse>>(url, arg);
    console.log(res.data.data);
    return res.data;
  } catch (error: any) {
    if (error.response?.data) {
      return error.response.data as FetchResponse<UserResponse>;
    }
    throw error;
  }
};

export const useLoginAsync = () => useSWRMutation("/auth/login", loginAsync);

const authenticateAsync = async (url: string) => {
  try {
    const res = await helloAxios.post<FetchResponse<UserResponse>>(url);
    return res.data;
  } catch (error: any) {
    return error.response.data as FetchResponse<UserResponse>;
  }
};

export const useAuthenticateAsync = () =>
  useSWRMutation("auth/authenticate", authenticateAsync);

const refreshAsync = async (url: string) => {
  try {
    const res = await helloAxios.post<FetchResponse<string>>(url);
    return res.data.ok;
  } catch (error: any) {
    return error.response.data as FetchResponse<string>;
  }
};

export const useRefreshAsync = () =>
  useSWRMutation("auth/refresh", refreshAsync);
