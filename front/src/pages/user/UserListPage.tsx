import React, { useState, useRef, useEffect } from "react";
import { ProgressSpinner } from "primereact/progressspinner";
import { useGetUsersQuery } from "../../services/user/user";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import type { User } from "../../services/user/types";
import { env } from "../../env";

const UserListPage: React.FC = () => {
  const { data, isLoading, isError } = useGetUsersQuery();

  const [editingUserId, setEditingUserId] = useState<string | null>(null);
  const [images, setImages] = useState<Record<string, string>>({});

  const fileInputRef = useRef<HTMLInputElement>(null);

  useEffect(() => {
    const savedImages = localStorage.getItem("userImages");
    if (savedImages) {
      setImages(JSON.parse(savedImages));
    }
  }, []);

  useEffect(() => {
    if (editingUserId && fileInputRef.current) {
      fileInputRef.current.value = "";
      fileInputRef.current.click();
    }
  }, [editingUserId]);

  const rolesBodyTemplate = (rowData: User) => (
    <span>{rowData.roles.map((r) => r.name).join(", ")}</span>
  );

  const avatarBodyTemplate = (rowData: User) => {
    const localImage = images[rowData.id];
    const imgSrc = localImage
      ? localImage
      : rowData.image
      ? env.imagesUrl + rowData.image
      : env.imageDefault;

    return (
      <div style={{ position: "relative", display: "inline-block" }}>
        <img
          width={50}
          height={50}
          alt={rowData.userName}
          src={imgSrc}
          style={{ borderRadius: "50%", objectFit: "cover" }}
        />
        <button
          onClick={() => setEditingUserId(rowData.id)}
          style={{
            position: "absolute",
            top: 0,
            right: 0,
            background: "transparent",
            border: "none",
            cursor: "pointer",
            padding: 0,
            margin: 0,
          }}
          title="Редагувати аватар"
          type="button"
        >
          ✏️
        </button>
      </div>
    );
  };

  const onFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file || !editingUserId) return;

    const reader = new FileReader();
    reader.onloadend = () => {
      if (typeof reader.result === "string") {
        const base64 = reader.result as string;
        setImages((prev) => {
          const newImages: Record<string, string> = { ...prev, [editingUserId]: base64 };
          localStorage.setItem("userImages", JSON.stringify(newImages));
          return newImages;
        });
      }
      setEditingUserId(null);
    };
    reader.readAsDataURL(file);
  };

  return (
    <>
      {isLoading ? (
        <ProgressSpinner />
      ) : !isError && data?.payload !== null ? (
        <>
          <DataTable value={data?.payload ?? []} editMode="row" dataKey="id">

            <Column field="userName" header="UserName" />
            <Column field="email" header="Email" />
            <Column field="firstName" header="First Name" />
            <Column field="lastName" header="Last Name" />
            <Column field="image" header="Avatar" body={avatarBodyTemplate} />
            <Column field="roles" header="Roles" body={rolesBodyTemplate} />
          </DataTable>

          <input
            type="file"
            accept="image/*"
            style={{ display: "none" }}
            ref={fileInputRef}
            onChange={onFileChange}
          />
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
            <h2 style={{ color: "red" }}>Failed connect to server</h2>
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