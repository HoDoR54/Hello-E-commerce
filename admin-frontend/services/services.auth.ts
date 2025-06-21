import { Truculenta } from "next/font/google";
import { AdminResponse, LoginRequest } from "../types/auth.types";
import { FetchResponse } from "../types/general.types";
import axios from "axios";
import helloAxios from "../lib/helloAxios";

export const loginAsync = async (
  url: string,
  { arg }: { arg: LoginRequest }
) => {
  try {
    const res = await helloAxios.post<FetchResponse<AdminResponse>>(url, arg);
    console.log(res.data.ok && res.data.data);
    localStorage.setItem("isLoggedIn", JSON.stringify(true));
    return res.data.ok && res.data.data;
  } catch (error: any) {
    localStorage.setItem("isLoggedIn", JSON.stringify(false));
    throw error;
  }
};
