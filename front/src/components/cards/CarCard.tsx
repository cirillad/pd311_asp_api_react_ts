import type React from "react";
import type { Car } from "../../services/car/types";
import { Card } from "primereact/card";
import { Carousel } from "primereact/carousel";
import { Image } from "primereact/image";
import { env } from "../../env";

const CarCard: React.FC<{ car: Car }> = ({ car }) => {
    const imageTemplate = (image: string) => {
        return (
            <div style={{ textAlign: "center" }}>
                <Image
                    height="200px"
                    preview
                    alt={car.model}
                    src={`${env.imagesUrl}${image}`}
                />
            </div>
        );
    };

    const header = (
        <Carousel
            value={car.images}
            numVisible={1}
            numScroll={1}
            circular
            itemTemplate={imageTemplate}
        />
    );

    return (
        <div className="card flex justify-content-center">
            <Card
                title={car.model}
                subTitle={car.brand}
                header={header}
                style={{ height: "100%" }}
                className="md:w-25rem"
            >
                <p className="m-0">Колір: {car.color}</p>
                <p className="m-0">Коробка передач: {car.gearbox}</p>
                <p className="m-0">Ціна: ${car.price}</p>
            </Card>
        </div>
    );
};

export default CarCard;
