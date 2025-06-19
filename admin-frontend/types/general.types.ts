export type FetchResponse<T> = {
  ok: boolean;
  statusCode: number;
  errorMessage?: string;
  data?: T;
};
