export interface CreateCar {
    model: string;
    brand: string;
    year: number;
    price: number;
    color: string;
    gearbox: string;
    manufacture: string;
    images: File[];
}

export interface Car {
    id: string;
    model: string;
    brand: string;
    year: number;
    price: number;
    color: string;
    gearbox: string;
    manufacture: string;
    images: string[];
}

export interface CarListParams {
    page: number;
    pageSize: number;
    manufacture: string;
}

export interface ListResponse {
    cars: Car[],
    page: number;
    totalCount: number;
    pageCount: number;
}