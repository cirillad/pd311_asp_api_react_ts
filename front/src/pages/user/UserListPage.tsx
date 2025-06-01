import React from "react";
import { ProgressSpinner } from "primereact/progressspinner";
import { useGetUsersQuery } from "../../services/user/user";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import type { User } from "../../services/user/types";
import { env } from "../../env";

const UserListPage: React.FC = () => {
    const { data, isLoading, isError } = useGetUsersQuery();

    const rolesBodyTemplate = (rowData: User) => {
        return <span>{rowData.roles.map(r => r.name).join(', ')}</span>;
    };

    const avatarBodyTemplate = (rowData: User) => {
        return <img 
        width="50px"
        alt={rowData.userName} 
        src={`${rowData.image ? env.imagesUrl + rowData.image : env.imageDefault}`}/>
    }

    return (
        <>
            {isLoading ? (
                <ProgressSpinner />
            ) : !isError && data?.payload !== null ? (
                <>
                    <DataTable
                        value={data?.payload}
                        editMode="row"
                        dataKey="id"
                    >
                        <Column field="userName" header="UserName"></Column>
                        <Column field="email" header="Email"></Column>
                        <Column field="firstName" header="First Name"></Column>
                        <Column field="lastName" header="Last Name"></Column>
                        <Column field="image" header="Avatar" body={avatarBodyTemplate}></Column>
                        <Column field="roles" header="Roles" body={rolesBodyTemplate}></Column>
                    </DataTable>
                </>
            ) : (
                isError && (
                    <div
                        style={{
                            display: "flex",
                            alignItems: "center",
                            flexDirection: "column",
                        }}
                    >
                        <h2 style={{ color: "red" }}>
                            Failed connect to server
                        </h2>
                        <img
                            alt="error"
                            src="https://t4.ftcdn.net/jpg/03/58/75/95/360_F_358759526_1DbvGd9bnHops9Vns8LRxyCgpk0mrA5i.jpg"
                        />
                    </div>
                )
            )}
        </>
    );
};

export default UserListPage;
