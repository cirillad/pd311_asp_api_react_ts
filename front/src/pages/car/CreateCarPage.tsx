import React, { useState, type ChangeEvent, useRef } from "react";
import { InputText } from "primereact/inputtext";
import { Button } from "primereact/button";
import type { CreateCar } from "../../services/car/types";
import { useCreateCarMutation } from "../../services/car/car";
import { Image } from "primereact/image";
import { Toast } from "primereact/toast";
import { useNavigate } from "react-router";

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
    const toast = useRef<Toast>(null);
    const navigate = useNavigate();

    // toast
    const showSuccesToast = (message: string) => {
        toast.current?.show({
            severity: "success",
            summary: "Success",
            detail: message,
            life: 3000,
        });
    };

    const showErrorToast = (message: string) => {
        toast.current?.show({
            severity: "error",
            summary: "Error",
            detail: message,
            life: 3000,
        });
    };

    const showResult = (res: any) => {        
        if (!res.error) {
            if ("message" in res.data) {
                showSuccesToast(res.data.message);
            }
        } else {
            if ("data" in res.error) {
                const data = res.error.data as any;
                console.log(data);
                if ("message" in data) {
                    showErrorToast(data.message);
                } else if ("title" in data) {
                    showErrorToast(data.title);
                }
            }
        }
    };

    const inputChangeHandler = (e: ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setCarData((prev) => {
            return { ...prev, [name]: value };
        });
    };

    const inputImagesHandler = (e: ChangeEvent<HTMLInputElement>) => {
        if (e.target.files && e.target.files?.length > 0) {
            for (const file of e.target.files) {
                setImages((prev) => {
                    return [...prev, file];
                });
            }
        }
    };

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
        });

        const response = await createCar(data);
        showResult(response);

        if (!response.error) {
            navigate("/cars");
        }
    };

    return (
        <div>
            <Toast ref={toast} />
            <div>
                <InputText
                    onChange={inputChangeHandler}
                    placeholder="Brand"
                    name="brand"
                />
            </div>
            <div>
                <InputText
                    onChange={inputChangeHandler}
                    placeholder="Model"
                    name="model"
                />
            </div>
            <div>
                <InputText
                    onChange={inputChangeHandler}
                    placeholder="Year"
                    type="number"
                    name="year"
                />
            </div>
            <div>
                <InputText
                    onChange={inputChangeHandler}
                    placeholder="Price"
                    type="number"
                    name="price"
                />
            </div>
            <div>
                <InputText
                    onChange={inputChangeHandler}
                    placeholder="Color"
                    name="color"
                />
            </div>
            <div>
                <InputText
                    onChange={inputChangeHandler}
                    placeholder="Gearbox"
                    name="gearbox"
                />
            </div>
            <div>
                <InputText
                    onChange={inputChangeHandler}
                    placeholder="Manufacture"
                    name="manufacture"
                />
            </div>
            <div>
                <div
                    style={{
                        marginBottom: "10px",
                        display: "flex",
                        flexWrap: "wrap",
                        gap: "16px",
                        marginTop: "16px",
                    }}
                >
                    {images.map((image, index) => (
                        <div
                            key={index}
                            style={{
                                border: "1px solid #ccc",
                                borderRadius: "8px",
                                boxShadow: "0 2px 8px rgba(0, 0, 0, 0.15)",
                                padding: "8px",
                                backgroundColor: "#fff",
                                display: "flex",
                                justifyContent: "center",
                                alignItems: "center",
                                width: "160px",
                                height: "160px",
                                overflow: "hidden",
                            }}
                        >
                            <Image
                                width="100%"
                                height="auto"
                                preview
                                alt={image.name}
                                src={URL.createObjectURL(image)}
                                imageStyle={{
                                    objectFit: "cover",
                                    maxHeight: "140px",
                                    borderRadius: "4px",
                                }}
                            />
                        </div>
                    ))}
                </div>
                <input
                    onChange={inputImagesHandler}
                    type="file"
                    accept="image/*"
                    multiple
                />
            </div>
            <div style={{ marginTop: "10px" }}>
                <Button onClick={submitHandler}>Add</Button>
            </div>
        </div>
    );
};

export default CreateCarPage;
