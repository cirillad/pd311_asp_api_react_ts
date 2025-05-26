import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { ApiResponse } from "../../types";
import { User } from "./types";

export const userApi = createApi({
    reducerPath: "userApi",
    baseQuery: fetchBaseQuery({ baseUrl: import.meta.env.VITE_API_URL }),
    tagTypes: ["User"],
    endpoints: (builder) => ({
        getUsers: builder.query<ApiResponse<User[]>, void>({
            query: () => "user",
            providesTags: ["User"],
        }),
        deleteUser: builder.mutation<ApiResponse<null>, string>({
            query: (id) => ({
                url: "user",
                method: "DELETE",
                params: { id },
            }),
            invalidatesTags: ["User"],
        }),
    }),
});

export const { useGetUsersQuery, useDeleteUserMutation } = userApi;
