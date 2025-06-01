import { Route, Routes } from "react-router";
import "./App.css";
import Navbar from "./Components/navbar/Navbar";
import RoleListPage from "./pages/role/RoleListPage";
import UserListPage from "./pages/user/UserListPage";
import CarListPage from "./pages/car/CarListPage";
import CreateCarPage from "./pages/car/CreateCarPage";
import UserCreatePage from "./pages/user/UserCreatePage";
import UserUpdatePage from "./pages/user/UserUpdatePage";

function App() {
    return (
        <>
            <Navbar />
            <div style={{ width: "66%", margin: "30px auto" }}>
                <Routes>
                    <Route path="/roles" element={<RoleListPage />} />
                    <Route path="/users">
                        <Route index element={<UserListPage />} />
                        <Route path="create" element={<UserCreatePage />} />
                        <Route path="update/:id" element={<UserUpdatePage />} />
                    </Route>
                    <Route path="/cars">
                        <Route index element={<CarListPage />} />
                        <Route path="create" element={<CreateCarPage />} />
                    </Route>
                </Routes>
            </div>
        </>
    );
}

export default App;
