export interface ServiceResponse<T> {
    isSuccess: boolean;
    message: string;
    payload: T | null;
}