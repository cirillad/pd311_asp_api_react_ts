import React, { type CSSProperties } from "react";
import { useGetCarsQuery } from "../../services/car/car";
import { ProgressSpinner } from "primereact/progressspinner";
import CarCard from "../../Components/cards/CarCard";
import { Button } from "primereact/button";
import { Link } from "react-router";

const carsContainer: CSSProperties = {
    display: "grid",
    gridTemplateColumns: "1fr 1fr 1fr",
    gap: "15px",
};

const CarListPage: React.FC = () => {
    const listParams = {
        page: 1,
        pageSize: 4,
        manufacture: "",
    };
    const { data, isLoading, isError } = useGetCarsQuery(listParams);

    return (
        <div>
            {isLoading ? (
                <ProgressSpinner />
            ) : !isError && data?.payload ? (
                <div>
                    <div style={carsContainer}>
                        {data.payload.cars.map((car) => (
                            <CarCard car={car} />
                        ))}
                    </div>
                    <div style={{ marginTop: "10px" }}>
                        <Link to="create">
                            <Button>Add car</Button>
                        </Link>
                    </div>
                </div>
            ) : (
                isError && (
                    <div
                        style={{
                            display: "flex",
                            alignItems: "center",
                            flexDirection: "column",
                        }}
                    >
                        <h2 style={{ color: "red" }}>
                            Failed connect to server
                        </h2>
                        <img
                            alt="error"
                            src="https://t4.ftcdn.net/jpg/03/58/75/95/360_F_358759526_1DbvGd9bnHops9Vns8LRxyCgpk0mrA5i.jpg"
                        />
                    </div>
                )
            )}
        </div>
    );
};

export default CarListPage;
