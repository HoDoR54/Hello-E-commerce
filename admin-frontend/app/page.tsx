"use client";

import useUserSessionStore from "../store/useUserSessionStore";

export default function Home() {
  const currentUser = useUserSessionStore((state) => state.currentUser);

  return (
    <main>
      <section>Welcome to the Protected Dashboard!</section>
      {currentUser ? (
        <div className="bg-blue-200 p-5 flex items-center justify-center">
          User ID: {currentUser?.userId}
          Admin ID: {currentUser?.adminId}
          Name: {currentUser?.name}
          Created At: {currentUser?.createdAt}
        </div>
      ) : (
        <div>No user fetched.</div>
      )}
    </main>
  );
}
