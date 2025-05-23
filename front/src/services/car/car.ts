import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { env } from "../../env";
import type { ServiceResponse } from "../../types";

export const carApi = createApi({
    baseQuery: fetchBaseQuery({ baseUrl: env.apiUrl }),
    tagTypes: ["Car"],
    endpoints: (builder) => ({
        createCar: builder.mutation<ServiceResponse<null>, FormData>({
            query: (data) => ({
                url: "car",
                method: "POST",
                body: data
            }),
            invalidatesTags: ["Car"]
        })
    }),
});

export const { useCreateCarMutation } = carApi;