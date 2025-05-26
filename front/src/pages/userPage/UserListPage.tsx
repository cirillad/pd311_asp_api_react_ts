import React, { useRef, useState } from "react";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import {
    useGetUsersQuery,
    useDeleteUserMutation
} from "../../services/user/user";
import { ProgressSpinner } from "primereact/progressspinner";
import { ContextMenu } from "primereact/contextmenu";
import { Toast } from "primereact/toast";
import { User } from "../../services/user/types";
import { env } from "../../env";

const UserListPage: React.FC = () => {
    const { data, isError, isLoading } = useGetUsersQuery();
    const [selectedUser, setSelectedUser] = useState<User | undefined>(
        undefined
    );
    const [deleteRole] = useDeleteUserMutation();
    const cm = useRef<ContextMenu>(null);
    const toast = useRef<Toast>(null);

    const menuModel = [
        {
            label: "Delete",
            icon: "pi pi-fw pi-times",
            command: () => deleteRoleHandler(selectedUser),
        },
    ];

    // toast
    const showSuccess = (message: string) => {
        toast.current?.show({
            severity: "success",
            summary: "Success",
            detail: message,
            life: 3000,
        });
    };

    const showError = (message: string) => {
        toast.current?.show({
            severity: "error",
            summary: "Error",
            detail: message,
            life: 3000,
        });
    };

    const apiResultHanler = (result: any) => {
        if (!result.error) {
            if ("message" in result.data) {
                showSuccess(result.data.message);
            }
        } else {
            if ("data" in result.error) {
                const data = result.error.data as any;
                if ("message" in data) {
                    showError(data.message);
                }
            }
        }
    };

    const rolesDataTemplate = (rowData: User) => {
        return <span>{rowData.roles.map(r => r.name).join(', ')}</span>
    }

    const avatarDataTemplate = (rowData: User) => {
        return <img style={{borderRadius: "15%"}} width="50" src={`${rowData.image ? env.imagesUrl + rowData.image : env.defaultImage}`} alt={rowData.userName} />
    }

    const deleteRoleHandler = async (user: User | undefined) => {
        if (user !== undefined) {
            const res = await deleteRole(user.id);
            apiResultHanler(res);
        }
    };

    return (
        <div>
            {isLoading ? (
                <div style={{ display: "flex", justifyContent: "center" }}>
                    <ProgressSpinner />
                </div>
            ) : data?.payload !== null && data !== undefined ? (
                <>
                    <div className="card">
                        <Toast ref={toast} />
                        <ContextMenu
                            model={menuModel}
                            ref={cm}
                            onHide={() => setSelectedUser(undefined)}
                        />
                        <DataTable
                            value={data.payload}
                            onContextMenuSelectionChange={(e) =>
                                setSelectedUser(e.value)
                            }
                            contextMenuSelection={setSelectedUser}
                            onContextMenu={(e) =>
                                cm.current?.show(e.originalEvent)
                            }
                            dataKey="id"
                            editMode="row"
                            tableStyle={{ minWidth: "50rem" }}
                        >
                            <Column field="image" header="Avatar" body={avatarDataTemplate}></Column>
                            <Column field="userName" header="UserName"></Column>
                            <Column field="email" header="Email"></Column>
                            <Column field="firstName" header="First Name"></Column>
                            <Column field="lastName" header="Last Name"></Column>
                            <Column field="roles" header="Roles" body={rolesDataTemplate}></Column>
                        </DataTable>
                    </div>
                </>
            ) : (
                isError && (
                    <div
                        style={{
                            display: "flex",
                            flexDirection: "column",
                            alignItems: "center",
                        }}
                    >
                        <h1 className="text-4xl text-red-600 m-3 font-semibold">
                            Connection failed
                        </h1>
                        <img
                            alt="error"
                            style={{ maxWidth: "600px" }}
                            src="https://static.vecteezy.com/system/resources/previews/021/786/446/non_2x/electric-socket-with-a-plug-icon-in-flat-style-connection-symbol-illustration-on-isolated-background-404-error-sign-business-concept-vector.jpg"
                        />
                    </div>
                )
            )}
        </div>
    );
};

export default UserListPage;
