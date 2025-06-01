import type { Role } from "../role/types";

export interface CreateUser {
    email: string;
    userName: string;
    firstName: string;
    lastName: string;
    password: string;
    emailConfirmed: boolean;
    roles: string[];
    image: File | null;
}

export interface User {
    id: string;
    email: string;
    userName: string;
    firstName: string;
    lastName: string;
    image: string;
    roles: Role[]
}