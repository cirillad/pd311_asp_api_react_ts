import React, { useRef, useState } from "react";
import {
    useCreateRoleMutation,
    useDeleteRoleMutation,
    useGetRolesQuery,
    useUpdateRoleMutation,
} from "../../services/role/role";
import { ProgressSpinner } from "primereact/progressspinner";
import {
    DataTable,
    type DataTableRowEditCompleteEvent,
} from "primereact/datatable";
import { Column, type ColumnEditorOptions } from "primereact/column";
import { ContextMenu } from "primereact/contextmenu";
import type { Role } from "../../services/role/types";
import { InputText } from "primereact/inputtext";
import { Button } from "primereact/button";
import { Toast } from "primereact/toast";

const RoleListPage: React.FC = () => {
    const [selectedRole, setSelectedRole] = useState<Role | undefined>(
        undefined
    );
    const [roleName, setRoleName] = useState<string>("");
    const { data, isLoading, isError } = useGetRolesQuery();
    const [deleteRole] = useDeleteRoleMutation();
    const [createRole] = useCreateRoleMutation();
    const [updateRole] = useUpdateRoleMutation();
    const cm = useRef<ContextMenu>(null);
    const toast = useRef<Toast>(null);

    // toast
    const showSuccesToast = (message: string) => {
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

    const menuModel = [
        {
            label: "Delete",
            icon: "pi pi-fw pi-times",
            command: () => deleteHandler(selectedRole),
        },
    ];

    const deleteHandler = async (role: Role | undefined) => {
        if (role) {
            const res = await deleteRole(role.id);
            showResult(res)
        }
    };

    const onRowEditComplete = async (e: DataTableRowEditCompleteEvent) => {
        const res = await updateRole(e.newData as Role)
        showResult(res)
    };

    const showResult = (res: any) => {
        if (!res.error) {
            if ("message" in res.data) {
                showSuccesToast(res.data.message);
            }
            setRoleName("");
        } else {
            if ("data" in res.error) {
                const data = res.error.data as any;
                if ("message" in data) {
                    showErrorToast(data.message);
                }
            }
        }
    };

    const createHandler = async () => {
        const res = await createRole({ name: roleName });
        showResult(res)
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

    const allowEdit = (rowData: Role) => {
        return rowData.name !== "admin";
    };

    return (
        <>
            {isLoading ? (
                <ProgressSpinner />
            ) : (
                !isError &&
                data?.payload !== null ? (
                    <>
                        <Toast ref={toast} />
                        <ContextMenu
                            model={menuModel}
                            ref={cm}
                            onHide={() => setSelectedRole(undefined)}
                        />
                        <DataTable
                            value={data?.payload}
                            editMode="row"
                            dataKey="id"
                            onContextMenu={(e) =>
                                cm.current?.show(e.originalEvent)
                            }
                            contextMenuSelection={selectedRole}
                            onContextMenuSelectionChange={(e) =>
                                setSelectedRole(e.value)
                            }
                            onRowEditComplete={onRowEditComplete}
                        >
                            <Column field="id" header="Id"></Column>
                            <Column
                                field="name"
                                header="Name"
                                editor={(options) => textEditor(options)}
                            ></Column>
                            <Column
                                rowEditor={allowEdit}
                                headerStyle={{ width: "10%", minWidth: "8rem" }}
                                bodyStyle={{ textAlign: "center" }}
                            ></Column>
                        </DataTable>
                        <InputText
                            className="p-inputtext-sm"
                            placeholder="Role name"
                            value={roleName}
                            onChange={(e) => setRoleName(e.target.value)}
                            style={{ margin: "10px 0px" }}
                        />
                        <Button
                            onClick={createHandler}
                            size="small"
                            style={{ margin: "10px 5px" }}
                        >
                            Add
                        </Button>
                    </>
                ) : isError && (
                    <div style={{display: "flex", alignItems: "center", flexDirection: "column"}}>
                        <h2 style={{color: "red"}}>Failed connect to server</h2>
                        <img alt="error" src="https://t4.ftcdn.net/jpg/03/58/75/95/360_F_358759526_1DbvGd9bnHops9Vns8LRxyCgpk0mrA5i.jpg"/>
                    </div>
                )
            )}
        </>
    );
};

export default RoleListPage;
