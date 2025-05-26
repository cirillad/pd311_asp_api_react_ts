import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { ApiResponse } from "../../types";
import { Role } from "./types";

export const roleApi = createApi({
    reducerPath: "roleApi",
    baseQuery: fetchBaseQuery({ baseUrl: import.meta.env.VITE_API_URL }),
    tagTypes: ["Role"],
    endpoints: (builder) => ({
        getRoles: builder.query<ApiResponse<Role[]>, void>({
            query: () => "role",
            providesTags: ["Role"],
        }),
        updateRole: builder.mutation<ApiResponse<null>, Role>({
            query: (role) => ({
                url: "role",
                method: "PUT",
                body: role,
            }),
            invalidatesTags: ["Role"],
        }),
        deleteRole: builder.mutation<ApiResponse<null>, string>({
            query: (id) => ({
                url: "role",
                method: "DELETE",
                params: { id },
            }),
            invalidatesTags: ["Role"],
        }),
        createRole: builder.mutation<ApiResponse<null>, string>({
            query: (roleName) => ({
                url: "role",
                method: "POST",
                body: {name: roleName},
            }),
            invalidatesTags: ["Role"],
        }),
    }),
});

export const {
    useGetRolesQuery,
    useUpdateRoleMutation,
    useDeleteRoleMutation,
    useCreateRoleMutation
} = roleApi;
