import React from "react";
import { env } from "../../env";

const CarListPage: React.FC = () => {
    return (
        <img
            src={
                env.imagesUrl +
                "cars\\490a1a2a-b4fd-4928-9c62-8182e976981a\\f8796bae-64bc-4939-98a0-0cdd19185628.webp"
            }
        />
    );
};

export default CarListPage;
