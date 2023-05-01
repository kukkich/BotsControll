import axios from "axios";

const API_URL_NO_SSL = 'http://localhost:5042'
const API_URL_SSL = 'https://localhost:7126'

export const API_URL = API_URL_SSL

const $api = axios.create({
    withCredentials: true,
    baseURL: API_URL + "/api"
})

// $api.interceptors.request.use(config => {
//     config.headers.Authorization = `Bearer ${localStorage.getItem('token')}`
//     return config;
// })

export default $api;