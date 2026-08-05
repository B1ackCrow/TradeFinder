import axios from 'axios'

const http = axios.create({
    baseURL: 'http://localhost:5185/api',
    withCredentials: true
})

export default http