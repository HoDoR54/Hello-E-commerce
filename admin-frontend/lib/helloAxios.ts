import axios from "axios";
import Cookies from "js-cookie";

const helloAxios = axios.create({
  baseURL: "https://localhost:7000/api",
  headers: {
    Accept: "application/json",
    "Content-Type": "application/json",
  },
  withCredentials: true,
});

helloAxios.interceptors.request.use((config) => {
  const token = Cookies.get("access_token");
  if (token && config.headers) {
    config.headers["Authorization"] = `Bearer ${token}`;
  }
  return config;
});

helloAxios.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      console.warn("401 Unauthorized");
    }
    return Promise.reject(error);
  }
);

export default helloAxios;
