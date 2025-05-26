import React, { useRef, useState } from "react";
import { DataTable, DataTableRowEditCompleteEvent } from "primereact/datatable";
import { Column, ColumnEditorOptions } from "primereact/column";
import {
    useCreateRoleMutation,
    useDeleteRoleMutation,
    useGetRolesQuery,
    useUpdateRoleMutation,
} from "../../services/role/role";
import { ProgressSpinner } from "primereact/progressspinner";
import { Role } from "../../services/role/types";
import { InputText } from "primereact/inputtext";
import { ContextMenu } from "primereact/contextmenu";
import { Button } from "primereact/button";
import { Toast } from 'primereact/toast';

const RoleListPage: React.FC = () => {
    const { data, isError, isLoading } = useGetRolesQuery();
    const [selectedRole, setSelectedRole] = useState<Role | undefined>(
        undefined
    );
    const [roleName, setRoleName] = useState<string>("");
    const [updateRole] = useUpdateRoleMutation();
    const [deleteRole] = useDeleteRoleMutation();
    const [createRole] =
        useCreateRoleMutation();
    const cm = useRef<ContextMenu>(null);
    const toast = useRef<Toast>(null);

    const menuModel = [
        {
            label: "Delete",
            icon: "pi pi-fw pi-times",
            command: () => deleteRoleHandler(selectedRole),
        },
    ];

    // toast
    const showSuccess = (message: string) => {
        toast.current?.show({severity:'success', summary: 'Success', detail: message, life: 3000});
    }

    const showError = (message: string) => {
        toast.current?.show({severity:'error', summary: 'Error', detail: message, life: 3000});
    }

    const apiResultHanler = (result: any) => {
        if (!result.error) {
            setRoleName("");
            if('message' in result.data) {
                showSuccess(result.data.message);
            }
        } else {
            if('data' in result.error) {
                const data = result.error.data as any;
                if('message' in data) {
                    showError(data.message);
                }
            }
        }
    }

    const onRowEditComplete = async (e: DataTableRowEditCompleteEvent) => {
        const newRole = {
            id: e.newData.id,
            name: e.newData.name,
        };

        const res = await updateRole(newRole);
        apiResultHanler(res);
    };

    const deleteRoleHandler = async (role: Role | undefined) => {
        if (role === undefined || role.name === "admin") {
            return;
        }

        const res = await deleteRole(role.id);
        apiResultHanler(res);
    };

    const createRoleHandler = async () => {
        const res = await createRole(roleName);
        apiResultHanler(res);
    };

    const textEditor = (options: ColumnEditorOptions) => {
        return (
            <InputText
                type="text"
                value={options.value}
                onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                    options.editorCallback!(e.target.value)
                }
            />
        );
    };

    return (
        <div>
            {isLoading ? (
                <div style={{display: "flex", justifyContent: "center"}}>
                    <ProgressSpinner />
                </div>
            ) : (
                data?.payload !== null &&
                data !== undefined ? (
                    <>
                        <div className="card">
                            <Toast ref={toast} />
                            <ContextMenu
                                model={menuModel}
                                ref={cm}
                                onHide={() => setSelectedRole(undefined)}
                            />
                            <DataTable
                                value={data.payload}
                                onContextMenuSelectionChange={(e) =>
                                    setSelectedRole(e.value)
                                }
                                contextMenuSelection={selectedRole}
                                onRowEditComplete={onRowEditComplete}
                                onContextMenu={(e) =>
                                    cm.current?.show(e.originalEvent)
                                }
                                dataKey="id"
                                editMode="row"
                                tableStyle={{ minWidth: "50rem" }}
                            >
                                <Column field="id" header="Id"></Column>
                                <Column
                                    field="name"
                                    header="Name"
                                    editor={(options) => textEditor(options)}
                                ></Column>
                                <Column
                                    rowEditor={(rowData: Role) =>
                                        rowData.name !== "admin"
                                    }
                                ></Column>
                            </DataTable>
                        </div>
                        <div>
                            <InputText
                                style={{ marginTop: "10px" }}
                                className="p-inputtext-sm"
                                type="text"
                                value={roleName}
                                onChange={(e) => setRoleName(e.target.value)}
                            />
                            <Button
                                onClick={createRoleHandler}
                                style={{ marginLeft: "5px" }}
                                size="small"
                                label="Add"
                            />
                        </div>
                    </>
                ) : isError && (
                    <div style={{display: "flex", flexDirection: "column", alignItems: "center"}}>
                        <h1 className="text-4xl text-red-600 m-3 font-semibold">Connection failed</h1>
                        <img alt="error" style={{maxWidth: "600px"}} src="https://static.vecteezy.com/system/resources/previews/021/786/446/non_2x/electric-socket-with-a-plug-icon-in-flat-style-connection-symbol-illustration-on-isolated-background-404-error-sign-business-concept-vector.jpg"/>
                    </div>
                )
            )}
        </div>
    );
};

export default RoleListPage;
