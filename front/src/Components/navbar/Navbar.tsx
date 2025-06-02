import React from "react";
import { Menubar } from "primereact/menubar";
import { InputText } from "primereact/inputtext";
import type { MenuItem } from "primereact/menuitem";
import { Avatar } from "primereact/avatar";
import { Button } from "primereact/button";
import { useNavigate } from "react-router";

const Navbar: React.FC = () => {
    const navigate = useNavigate();

    const items: MenuItem[] = [
        {
            label: "Home",
            icon: "pi pi-home",
            command: () => navigate("/"),
        },
        {
            label: "Roles",
            icon: "pi pi-star",
            command: () => navigate("/roles"),
        },
        {
            label: "Users",
            icon: "pi pi-users",
            command: () => navigate("/users"),
        },
        {
            label: "Cars",
            icon: "pi pi-car",
            command: () => navigate("/cars"),
        },
    ];

    const start = (
        <img
            alt="logo"
            src="https://primefaces.org/cdn/primereact/images/logo.png"
            height={40}
            style={{ marginRight: "0.5rem" }}
        />
    );

    const inputButtonWrapperStyle: React.CSSProperties = {
        display: "flex",
        alignItems: "center",
        gap: "1rem",
    };

    const inputStyle: React.CSSProperties = {
        height: "2.5rem",
        padding: "0 0.75rem",
        borderRadius: "6px",
    };

    const buttonStyle: React.CSSProperties = {
        height: "2.5rem",
        padding: "0 1rem",
        borderRadius: "6px",
    };

    const end = (
        <div style={inputButtonWrapperStyle}>
            <InputText placeholder="Search" style={inputStyle} />
            <Button
                label="Create User"
                icon="pi pi-user-plus"
                style={buttonStyle}
                onClick={() => navigate("/users/create")}
            />
            <Avatar
                image="https://primefaces.org/cdn/primereact/images/avatar/amyelsner.png"
                shape="circle"
            />
        </div>
    );

    return (
        <div className="card">
            <Menubar model={items} start={start} end={end} />
        </div>
    );
};

export default Navbar;