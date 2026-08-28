import http from "@/api/http.js";

export const getAllCompanies = () => {
    return http.get('/companies/get-all');
}

export const createNewCompany = (company) => {
    return http.post('/companies/create', company);
}

export const getCompanyById = (id) => {
    return http.get('/companies/get-by-id', {
        params: {
            id: id,
        }
    })
}

export const updateCompany = (company) => {
    return http.post('companies/update', company);
}