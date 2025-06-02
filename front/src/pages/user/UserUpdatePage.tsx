import React, { useEffect, useState, type ChangeEvent, useRef } from "react";
import { InputText } from "primereact/inputtext";
import { Button } from "primereact/button";
import { Checkbox, type CheckboxChangeEvent } from "primereact/checkbox";
import { MultiSelect, type MultiSelectChangeEvent } from "primereact/multiselect";
import { Toast } from "primereact/toast";
import { useNavigate } from "react-router";

import type { CreateUser } from "../../services/user/types";
import { useGetRolesQuery } from "../../services/role/role";
import type { Role } from "../../services/role/types";
import { useCreateUserMutation } from "../../services/user/user";

const UserCreatePage: React.FC = () => {
    const [selectedRoles, setSelectedRoles] = useState<Role[]>([]);
    const [userData, setUserData] = useState<CreateUser>({
        email: "",
        userName: "",
        firstName: "",
        lastName: "",
        password: "",
        roles: [],
        image: null,
        emailConfirmed: false,
    });
    const [image, setImage] = useState<File | null>(null);

    const toast = useRef<Toast>(null);
    const navigate = useNavigate();

    const { data, isError } = useGetRolesQuery();
    const [createUser] = useCreateUserMutation();

    useEffect(() => {
        if (data?.payload) {
            const userRole = data.payload.find((r) => r.name === "user");
            if (userRole) setSelectedRoles([userRole]);
        }
    }, [data]);

    const showSuccessToast = (message: string) => {
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
                showSuccessToast(res.data.message);
            }
        } else {
            if ("data" in res.error) {
                const data = res.error.data as any;
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
        setUserData((prev) => ({ ...prev, [name]: value }));
    };

    const checkboxChangeHandler = (e: CheckboxChangeEvent) => {
        setUserData((prev) => ({ ...prev, emailConfirmed: !!e.checked }));
    };

    const uploadImageHandler = (e: ChangeEvent<HTMLInputElement>) => {
        if (e.target.files?.length) {
            setImage(e.target.files[0]);
        }
    };

    const selectChangeHandler = (e: MultiSelectChangeEvent) => {
        setSelectedRoles(e.value as Role[]);
    };

    const submitHandler = async (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault();

        const formData = new FormData();
        formData.append("email", userData.email);
        formData.append("userName", userData.userName);
        formData.append("password", userData.password);
        formData.append("firstName", userData.firstName);
        formData.append("lastName", userData.lastName);
        formData.append("emailConfirmed", userData.emailConfirmed.toString());

        if (image) {
            formData.append("image", image);  // Просто додаємо файл напряму без змін
        }

        selectedRoles.forEach((role) => {
            formData.append("roles", role.name);
        });

        const response = await createUser(formData);
        showResult(response);

        if (!response.error) {
            navigate("/users");
        }
    };
    return (
        <>
            <Toast ref={toast} />
            <div style={{ maxWidth: 400, margin: "auto", padding: 20, background: "#fff", borderRadius: 8 }}>
                <h2 style={{ textAlign: "center", marginBottom: 20 }}>Create New User</h2>

                <InputText
                    name="email"
                    value={userData.email}
                    onChange={inputChangeHandler}
                    placeholder="Email"
                    style={{ width: "100%", marginBottom: 12 }}
                />

                <InputText
                    name="userName"
                    value={userData.userName}
                    onChange={inputChangeHandler}
                    placeholder="User Name"
                    style={{ width: "100%", marginBottom: 12 }}
                />

                <InputText
                    name="password"
                    type="password"
                    value={userData.password}
                    onChange={inputChangeHandler}
                    placeholder="Password"
                    style={{ width: "100%", marginBottom: 12 }}
                />

                <InputText
                    name="firstName"
                    value={userData.firstName}
                    onChange={inputChangeHandler}
                    placeholder="First Name"
                    style={{ width: "100%", marginBottom: 12 }}
                />

                <InputText
                    name="lastName"
                    value={userData.lastName}
                    onChange={inputChangeHandler}
                    placeholder="Last Name"
                    style={{ width: "100%", marginBottom: 12 }}
                />

                <div style={{ display: "flex", alignItems: "center", marginBottom: 12 }}>
                    <Checkbox
                        inputId="emailConfirmed"
                        checked={userData.emailConfirmed}
                        onChange={checkboxChangeHandler}
                    />
                    <label htmlFor="emailConfirmed" style={{ marginLeft: 8 }}>
                        Email confirmed
                    </label>
                </div>

                {!isError && data?.payload && (
                    <MultiSelect
                        value={selectedRoles}
                        options={data.payload}
                        onChange={selectChangeHandler}
                        optionLabel="name"
                        placeholder="Select roles"
                        style={{ width: "100%", marginBottom: 12 }}
                    />
                )}

                <input type="file" accept="image/*" onChange={uploadImageHandler} style={{ marginBottom: 12 }} />

                <Button label="Create User" onClick={submitHandler} style={{ width: "100%" }} />
            </div>
        </>
    );
};

export default UserCreatePage;