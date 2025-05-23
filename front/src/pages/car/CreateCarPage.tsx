import React, { useState, type ChangeEvent } from "react";
import { InputText } from "primereact/inputtext";
import { Button } from "primereact/button";
import type { CreateCar } from "../../services/car/types";
import { useCreateCarMutation } from "../../services/car/car";

const CreateCarPage: React.FC = () => {
    const [carData, setCarData] = useState<CreateCar>({
        brand: "",
        model: "",
        color: "",
        gearbox: "",
        price: 0,
        year: 0,
        manufacture: "",
        images: [],
    });
    const [images, setImages] = useState<File[]>([]);
    const [createCar] = useCreateCarMutation();

    const inputChangeHandler = (e: ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setCarData((prev) => {
            return { ...prev, [name]: value };
        });
    };

    const inputImagesHandler = (e: ChangeEvent<HTMLInputElement>) => {
        if(e.target.files && e.target.files?.length > 0) {
            for (const file of e.target.files) {
                setImages((prev) => {
                    return [...prev, file];
                })
            }
        }
    }

    const submitHandler = async (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault();
        
        const data = new FormData();
        data.append("brand", carData.brand);
        data.append("model", carData.model);
        data.append("color", carData.color);
        data.append("price", carData.price.toString());
        data.append("year", carData.year.toString());
        data.append("gearbox", carData.gearbox);
        data.append("manufacture", carData.manufacture);
        images.forEach((image) => {
            data.append("images", image);
        })

        await createCar(data);
    }

    return (
        <div>
            <div>
                <InputText onChange={inputChangeHandler} placeholder="Brand" name="brand" />
            </div>
            <div>
                <InputText onChange={inputChangeHandler} placeholder="Model" name="model" />
            </div>
            <div>
                <InputText onChange={inputChangeHandler} placeholder="Year" type="number" name="year" />
            </div>
            <div>
                <InputText onChange={inputChangeHandler} placeholder="Price" type="number" name="price" />
            </div>
            <div>
                <InputText onChange={inputChangeHandler} placeholder="Color" name="color" />
            </div>
            <div>
                <InputText onChange={inputChangeHandler} placeholder="Gearbox" name="gearbox" />
            </div>
            <div>
                <InputText onChange={inputChangeHandler} placeholder="Manufacture" name="manufacture" />
            </div>
            <div>
                <input onChange={inputImagesHandler} type="file" accept="image/*" multiple />
            </div>
            <div>
                <Button onClick={submitHandler}>Add</Button>
            </div>
        </div>
    );
};

export default CreateCarPage;
