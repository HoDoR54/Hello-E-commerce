"use client";

import useUserSessionStore from "../store/useUserSessionStore";

export default function Home() {
  const currentUser = useUserSessionStore((state) => state.currentUser);

  return (
    <main className="flex items-center justify-center bg-gray-50 p-6">
      <section className="bg-white rounded-md shadow-md p-6 w-full max-w-sm text-center">
        <h1 className="text-xl font-semibold mb-4 text-gray-800">
          Welcome to the Protected Dashboard!
        </h1>

        {currentUser ? (
          <div className="space-y-2 text-gray-700">
            <p>
              <strong>User ID:</strong> {currentUser.id}
            </p>
            <p>
              <strong>Email:</strong> {currentUser.email}
            </p>
            <p>
              <strong>Role:</strong> {currentUser.role.toString()}
            </p>
            <p>
              <strong>Created At:</strong>{" "}
              {new Date(currentUser.createdAt).toLocaleString()}
            </p>
          </div>
        ) : (
          <p className="text-gray-500">No user fetched.</p>
        )}
      </section>
    </main>
  );
}
