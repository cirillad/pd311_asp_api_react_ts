import type { Role } from "../role/types";

export interface User {
    id: string;
    email: string;
    userName: string;
    firstName: string;
    lastName: string;
    image: string;
    roles: Role[]
}