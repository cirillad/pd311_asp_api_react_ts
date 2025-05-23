import { Route, Routes } from "react-router";
import "./App.css";
import Navbar from "./Components/navbar/Navbar";
import RoleListPage from "./pages/role/RoleListPage";
import UserListPage from "./pages/user/UserListPage";
import CarListPage from "./pages/car/CarListPage";
import CreateCarPage from "./pages/car/CreateCarPage";

function App() {
    return (
        <>
            <Navbar />
            <div style={{ width: "66%", margin: "30px auto" }}>
                <Routes>
                    <Route path="/roles" element={<RoleListPage />} />
                    <Route path="/users" element={<UserListPage />} />
                    <Route path="/cars">
                        <Route index element={<CarListPage/>}/>
                        <Route path="create" element={<CreateCarPage/>}/>
                    </Route>
                </Routes>
            </div>
        </>
    );
}

export default App;
