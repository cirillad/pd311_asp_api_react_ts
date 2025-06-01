import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import type { ServiceResponse } from "../../types";
import type { Role } from "./types";
import { env } from "../../env";

export const roleApi = createApi({
    reducerPath: "roleApi",
    baseQuery: fetchBaseQuery({ baseUrl: env.apiUrl }),
    tagTypes: ["Role"],
    endpoints: (builder) => ({
        getRoles: builder.query<ServiceResponse<Role[]>, void>({
            query: () => "role",
            providesTags: ["Role"],
        }),
        deleteRole: builder.mutation<ServiceResponse<null>, string>({
            query: (id) => ({
                url: "role",
                method: "DELETE",
                params: { id },
            }),
            invalidatesTags: ["Role"],
        }),
        createRole: builder.mutation<ServiceResponse<null>, { name: string }>({
            query: (role) => ({
                url: "role",
                method: "POST",
                body: role,
            }),
            invalidatesTags: ["Role"],
        }),
        updateRole: builder.mutation<ServiceResponse<null>, Role>({
            query: (role) => ({
                url: "role",
                method: "PUT",
                body: role,
            }),
            invalidatesTags: ["Role"],
        })
    }),
});

export const {
    useGetRolesQuery,
    useDeleteRoleMutation,
    useCreateRoleMutation,
    useUpdateRoleMutation
} = roleApi;
