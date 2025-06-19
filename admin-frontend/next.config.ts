import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  devIndicators: {
    buildActivity: false, // Disables the "Building..." indicator
    buildActivityPosition: "bottom-right", // Optional: if you want to keep it but move it
  },
  // Configurations here
};

export default nextConfig;
