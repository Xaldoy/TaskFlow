import { jwtDecode } from "jwt-decode";
import axios, { InternalAxiosRequestConfig } from "axios";
import globalStore from "../stores/GlobalStore";
import RequestError from "../models/common/RequestError";
import errorStore from "../stores/ErrorStore";

interface RequestInjector extends InternalAxiosRequestConfig {
    loading?: boolean;
    loadingTimeoutId?: ReturnType<typeof setTimeout>;
}

const isTokenExpired = (token: string) => {
    try {
        const { exp } = jwtDecode(token);
        return exp && exp * 1000 < Date.now();
    } catch (e) {
        return true;
    }
};

axios.defaults.withCredentials = true;

// Axios request interceptor
axios.interceptors.request.use(async (config: RequestInjector) => {
    if (config.loading) {
        // Set a timeout to trigger the loading state after 1 second
        config.loadingTimeoutId = setTimeout(() => {
            globalStore.incrementPageLoading();
        }, 200);
    }

    const token = localStorage.getItem("token");
    if (token && isTokenExpired(token)) {
        localStorage.removeItem("token");
        localStorage.removeItem("username");
    } else if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
});

// Axios response interceptor
axios.interceptors.response.use(
    (response) => {
        const config = response.config as RequestInjector;
        if (config.loadingTimeoutId) {
            clearTimeout(config.loadingTimeoutId);
        }
        if (config.loading) {
            globalStore.decrementPageLoading();
        }

        return response;
    },
    (error) => {
        const config = error.config as RequestInjector;
        if (config?.loadingTimeoutId) {
            clearTimeout(config.loadingTimeoutId);
        }

        if (config?.loading) {
            globalStore.decrementPageLoading();
        }

        const requestError = error.response?.data as RequestError;

        if (
            requestError.errorCode &&
            requestError.errorMessage &&
            requestError.errorType
        ) {
            errorStore.addError(requestError);
        }
        return Promise.reject(error);
    }
);

const requests = {
    get: async (endpoint: string, loading: boolean = false) => {
        const response = await axios.get(
            `${import.meta.env.VITE_REACT_API_URL}${endpoint}`,
            { loading } as RequestInjector
        );
        return response.data;
    },
    post: async (endpoint: string, data: any, loading: boolean = false) => {
        const response = await axios.post(
            `${import.meta.env.VITE_REACT_API_URL}${endpoint}`,
            data,
            { loading } as RequestInjector
        );
        return response.data;
    },
    delete: async (endpoint: string, loading: boolean = false) => {
        const response = await axios.delete(
            `${import.meta.env.VITE_REACT_API_URL}${endpoint}`,
            { loading } as RequestInjector
        );
        return response.data;
    },
    getWithLoading: async (endpoint: string) => {
        return requests.get(endpoint, true);
    },
    postWithLoading: async (endpoint: string, data: any) => {
        return requests.post(endpoint, data, true);
    },
    deleteWithLoading: async (endpoint: string) => {
        return requests.delete(endpoint, true);
    },
};

export default requests;
