import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";

// This middleware runs on every request by default
export function middleware(request: NextRequest) {
  // Do nothing for now — just let the request through
  return NextResponse.next();
}

// Optional: specify which paths should be matched by this middleware
export const config = {
  matcher: [
    /*
      Example matchers:
      '/about/:path*',
      '/dashboard/:path*',
    */
  ],
};
