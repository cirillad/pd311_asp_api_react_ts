import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import type { ServiceResponse } from "../../types";
import type { User } from "./types";
import { env } from "../../env";

export const userApi = createApi({
    reducerPath: "userApi",
    baseQuery: fetchBaseQuery({ baseUrl: env.apiUrl }),
    tagTypes: ["User"],
    endpoints: (builder) => ({
        getUsers: builder.query<ServiceResponse<User[]>, void>({
            query: () => "user",
            providesTags: ["User"]
        }),
        createUser: builder.mutation<ServiceResponse<null>, FormData>({
            query: (data) => ({
                url: "user",
                method: "POST",
                body: data
            }),
            invalidatesTags: ["User"]
        })
    })
})

export const { useGetUsersQuery, useCreateUserMutation } = userApi;