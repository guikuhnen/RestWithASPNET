import axios from "axios";

const api = axios.create({
    baseURL: 'http://localhost:55000/api'
});

export default api;