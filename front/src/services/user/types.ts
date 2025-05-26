import { Role } from "../role/types";

export interface User {
    id: string;
    userName: string;
    email: string;
    firstName: string;
    lastName: string;
    image: string | null;
    roles: Role[];
}