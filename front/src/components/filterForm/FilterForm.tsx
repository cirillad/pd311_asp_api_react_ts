import React from "react";
import { InputText } from "primereact/inputtext";

const FilterForm: React.FC = () => {
    const styles = "flex justify-around w-full my-2";

    return (
        <div className="flex max-w-3/4 mx-auto mt-5 justify-end">
            <div className="grow-3" style={{ backgroundColor: "#db5c4c" }}>
                <form className="h-full">
                    <div
                        className="flex h-full px-5 py-3"
                        style={{
                            flexDirection: "column",
                            alignItems: "center",
                            justifyContent: "space-around",
                        }}
                    >
                        <div className={styles}>
                            <div className="pr-4" style={{ flexGrow: "1" }}>
                                <InputText
                                    className="w-full"
                                    keyfilter="int"
                                    placeholder="Integers"
                                />
                            </div>
                            <div className="pl-4" style={{ flexGrow: "1" }}>
                                <InputText
                                    className="w-full"
                                    keyfilter="int"
                                    placeholder="Integers"
                                />
                            </div>
                        </div>
                        <div className={styles}>
                            <div className="pr-4" style={{ flexGrow: "1" }}>
                                <InputText
                                    className="w-full"
                                    keyfilter="int"
                                    placeholder="Integers"
                                />
                            </div>
                            <div className="pl-4" style={{ flexGrow: "1" }}>
                                <InputText
                                    className="w-full"
                                    keyfilter="int"
                                    placeholder="Integers"
                                />
                            </div>
                        </div>
                        <div className={styles}>
                            <div className="pr-4" style={{ flexGrow: "1" }}>
                                <InputText
                                    className="w-full"
                                    keyfilter="int"
                                    placeholder="Integers"
                                />
                            </div>
                            <div className="pl-4" style={{ flexGrow: "1" }}>
                                <InputText
                                    className="w-full"
                                    keyfilter="int"
                                    placeholder="Integers"
                                />
                            </div>
                        </div>
                        <div className={styles}>
                            <div className="pr-4" style={{ flexGrow: "1" }}>
                                <InputText
                                    className="w-full"
                                    keyfilter="int"
                                    placeholder="Integers"
                                />
                            </div>
                            <div className="pl-4" style={{ flexGrow: "1" }}>
                                <InputText
                                    className="w-full"
                                    keyfilter="int"
                                    placeholder="Integers"
                                />
                            </div>
                        </div>
                        <div className={styles}>
                            <div className="pr-4" style={{ flexGrow: "1" }}>
                                <InputText
                                    className="w-full"
                                    keyfilter="int"
                                    placeholder="Integers"
                                />
                            </div>
                            <div className="pl-4" style={{ flexGrow: "1" }}>
                                <InputText
                                    className="w-full"
                                    keyfilter="int"
                                    placeholder="Integers"
                                />
                            </div>
                        </div>
                    </div>
                </form>
            </div>
            <div className="grow-1 mx-5">
                <img
                    width="300px"
                    height="250px"
                    src="https://cdn.riastatic.com/docs/pictures/common/2/210/21026/21026.avif"
                />
            </div>
        </div>
    );
};

export default FilterForm;
