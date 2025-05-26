export const env = {
    apiUrl: import.meta.env.VITE_API_URL as string,
    imagesUrl: import.meta.env.VITE_IMAGES_URL as string,
    defaultImage: `${import.meta.env.VITE_IMAGES_URL}default`
}