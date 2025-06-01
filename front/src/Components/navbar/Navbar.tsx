import type React from "react";
import { Menubar } from 'primereact/menubar';
import { InputText } from 'primereact/inputtext';
import type { MenuItem } from "primereact/menuitem";
import { Avatar } from 'primereact/avatar'; 
import { useNavigate } from "react-router";

const Navbar: React.FC = () => {
    const navigate = useNavigate();

    const items: MenuItem[] = [
        {
            label: 'Home',
            icon: 'pi pi-home',
            command() {
               navigate('/')
            },
        },
        {
            label: 'Roles',
            icon: 'pi pi-star',
            command() {
               navigate('/roles')
            },
        },
        {
            label: 'Users',
            icon: 'pi pi-search',
            command() {
               navigate('/users')
            },
        },
        {
            label: 'Cars',
            icon: 'pi pi-search',
            command() {
               navigate('/cars')
            },
        }
    ];

    const start = <img alt="logo" src="https://primefaces.org/cdn/primereact/images/logo.png" height="40" className="mr-2"></img>;
    const end = (
        <div className="flex align-items-center gap-2">
            <InputText placeholder="Search" type="text" className="w-8rem sm:w-auto" />
            <Avatar image="https://primefaces.org/cdn/primereact/images/avatar/amyelsner.png" shape="circle" />
        </div>
    );

    return (
        <div className="card">
            <Menubar model={items} start={start} end={end} />
        </div>
    )
}

export default Navbar;