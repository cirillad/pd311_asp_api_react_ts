export interface ApiResponse<T> {
    isSuccess: boolean;
    message: string;
    payload: T | null;
}