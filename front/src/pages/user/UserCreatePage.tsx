import React, { useEffect, useState, type ChangeEvent } from "react";
import { InputText } from "primereact/inputtext";
import { Button } from "primereact/button";
import type { CreateUser } from "../../services/user/types";
import { Checkbox, type CheckboxChangeEvent } from "primereact/checkbox";
import {
    MultiSelect,
    type MultiSelectChangeEvent,
} from "primereact/multiselect";
import { useGetRolesQuery } from "../../services/role/role";
import type { Role } from "../../services/role/types";
import { useCreateUserMutation } from "../../services/user/user";

const UserCreatePage: React.FC = () => {
    const [selectedRoles, setSelectedRoles] = useState<Role[] | null>(null);
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
    const { data, isLoading, isError } = useGetRolesQuery();
    const [createUser] = useCreateUserMutation();

    let roles: Role[] = [];

    if (!isLoading && !isError && data) {
        if (data.payload) {
            roles = data.payload as Role[];
        }
    }

    const inputChangeHandler = (e: ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setUserData((prev) => {
            return { ...prev, [name]: value };
        });
    };

    const checkboxChangeHandler = (e: CheckboxChangeEvent) => {
        setUserData((prev) => {
            if (e.checked !== undefined) {
                return { ...prev, emailConfirmed: e.checked };
            }
            return prev;
        });
    };

    const uploadImageHandler = (e: ChangeEvent<HTMLInputElement>) => {
        if (e.target.files && e.target.files.length > 0) {
            setImage(e.target.files[0]);
        }
    };

    const submitHandler = async (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault();

        const formData = new FormData();
        formData.append("email", userData.email);
        formData.append("userName", userData.userName);
        formData.append("password", userData.password);
        formData.append("firstName", userData.firstName);
        formData.append("lastName", userData.lastName);
        formData.append(
            "emailConfirmed",
            userData.emailConfirmed ? "true" : "false"
        );
        formData.append("image", image ? image : "");

        if (selectedRoles) {
            for (const role of selectedRoles) {
                formData.append("roles", role.name);
            }
        }

        const response = await createUser(formData);
        console.log(response);
    };

    useEffect(() => {
        if(data?.payload) {
            const userRole = data?.payload.find(r => r.name === "user");
            if(userRole) {
                setSelectedRoles([userRole]);
            }
        }
    }, [data]);

    const selectChangeHandler = (e: MultiSelectChangeEvent) => {
        if (e.value.length === 0) {
            return;
        }
        setSelectedRoles(e.value);
    };

    return (
        <div>
            <div>
                <InputText
                    onChange={inputChangeHandler}
                    placeholder="Email"
                    name="email"
                />
            </div>
            <div>
                <InputText
                    onChange={inputChangeHandler}
                    placeholder="User Name"
                    name="userName"
                />
            </div>
            <div>
                <InputText
                    onChange={inputChangeHandler}
                    placeholder="Password"
                    type="password"
                    name="password"
                />
            </div>
            <div>
                <InputText
                    onChange={inputChangeHandler}
                    placeholder="First name"
                    type="text"
                    name="firstName"
                />
            </div>
            <div>
                <InputText
                    onChange={inputChangeHandler}
                    placeholder="Last name"
                    name="lastName"
                />
            </div>
            <div>
                <Checkbox
                    checked={userData.emailConfirmed}
                    onChange={checkboxChangeHandler}
                />
                <label style={{ marginLeft: "10px" }}>Email confirmed</label>
            </div>
            {!isError && data && selectedRoles && (
                <div>
                    <MultiSelect
                        value={selectedRoles}
                        loading={isLoading}
                        onChange={selectChangeHandler}
                        options={roles}
                        optionLabel="name"
                        placeholder="Select roles"
                        maxSelectedLabels={3}
                        className="w-full md:w-20rem"
                    />
                </div>
            )}
            <div>
                <input type="file" onChange={uploadImageHandler} name="image" />
            </div>
            <div style={{ marginTop: "10px" }}>
                <Button onClick={submitHandler}>Add</Button>
            </div>
        </div>
    );
};

export default UserCreatePage;
