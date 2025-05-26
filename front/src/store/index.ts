import { configureStore } from '@reduxjs/toolkit'
import { roleApi } from '../services/role/role'
import { userApi } from '../services/user/user'
// ...

export const store = configureStore({
  reducer: {
    [roleApi.reducerPath]: roleApi.reducer,
    [userApi.reducerPath]: userApi.reducer,
  },
  middleware: (getDefaultMiddleware) => 
    getDefaultMiddleware().concat(roleApi.middleware, userApi.middleware)
})

// Infer the `RootState` and `AppDispatch` types from the store itself
export type RootState = ReturnType<typeof store.getState>
// Inferred type: {posts: PostsState, comments: CommentsState, users: UsersState}
export type AppDispatch = typeof store.dispatch