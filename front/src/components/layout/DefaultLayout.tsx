import React from "react";
import Navbar from "../navbar/Navbar";
import { Outlet } from "react-router";

const DefaultLayout: React.FC = () => {    
    return (
        <div>
            <div>
                <Navbar />
            </div>
            <div style={{padding: "0px 200px"}}>
                <Outlet />
            </div>
        </div>
    );
};

export default DefaultLayout;
