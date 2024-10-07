import { useGlobalContext } from "../context/Global.context";
import { jwtDecode } from "jwt-decode";
import AuthResult from "../models/AuthResult";
import LoginAttempt from "../models/LoginAttempt";
import RegisterAttempt from "../models/RegisterAttempt";
import accountController from "../api/controllers/AccountController";

// Utility for localStorage keys
const TOKEN_KEY = "token";
const USERNAME_KEY = "username";

const useAuth = () => {
    const globalContext = useGlobalContext();

    // Utility to handle token expiration
    const isTokenValid = (token: string): boolean => {
        try {
            const { exp }: any = jwtDecode(token);
            return exp && exp * 1000 > Date.now();
        } catch (error) {
            return false;
        }
    };

    const handleUserLoggedInCheck = () => {
        const token = getToken();
        const username = getLoggedInUsername();
        if (!username || !token || !isTokenValid(token)) {
            clearAuthData();
            globalContext.setLoggedInUser(undefined);
            return false;
        } else {
            globalContext.setLoggedInUser({
                token: token,
                userName: username
            } as AuthResult)
        }
        return true;
    }

    const getToken = (): string | null => {
        return localStorage.getItem(TOKEN_KEY);
    };

    const setToken = (token: string) => {
        localStorage.setItem(TOKEN_KEY, token);
    };

    const clearAuthData = () => {
        localStorage.removeItem(TOKEN_KEY);
        localStorage.removeItem(USERNAME_KEY);
    };

    const login = async (loginData: LoginAttempt): Promise<AuthResult> => {
        try {
            const response: AuthResult = await accountController.Login(loginData);
            if (response.userName && response.token) {
                storeUserInfo(response);
                return response;
            }
            return Promise.reject("Login failed");
        } catch (error) {
            return Promise.reject("Login failed");
        }
    };

    const logout = () => {
        clearAuthData();
        globalContext.setLoggedInUser(undefined);
    };

    const register = async (
        registerData: RegisterAttempt
    ): Promise<AuthResult> => {
        try {
            const response = await accountController.Register(registerData);
            if (response.user) {
                storeUserInfo(response.user);
                return response.user;
            }
            return Promise.reject("Registration failed");
        } catch (error) {
            return Promise.reject("Registration failed");
        }
    };

    const storeUserInfo = (authResult: AuthResult) => {
        if (authResult?.token && authResult?.userName) {
            setToken(authResult.token);
            localStorage.setItem(USERNAME_KEY, authResult.userName);
            globalContext.setLoggedInUser({ userName: authResult.userName });
        }
    };

    const getLoggedInUsername = (): string | null => {
        return localStorage.getItem(USERNAME_KEY);
    };

    const validate = (
        type: "email" | "password",
        value: string
    ): string | undefined => {
        const validators: { [key: string]: () => string | undefined } = {
            email: () => {
                const isEmailFormat = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
                return !isEmailFormat.test(value)
                    ? "Email is invalid."
                    : undefined;
            },
            password: () => {
                const hasDigit = /\d/;
                const hasLowercase = /[a-z]/;
                const hasUppercase = /[A-Z]/;
                const isValidLength = /^.{8,16}$/;

                if (!isValidLength.test(value))
                    return "Password must be between 8 and 16 characters long.";
                if (!hasDigit.test(value))
                    return "Password must contain at least one digit.";
                if (!hasLowercase.test(value))
                    return "Password must contain at least one lowercase letter.";
                if (!hasUppercase.test(value))
                    return "Password must contain at least one uppercase letter.";
            },
        };
        return validators[type]();
    };

    return {
        login,
        logout,
        register,
        getLoggedInUsername,
        handleUserLoggedInCheck,
        validateEmail: (email: string) => validate("email", email),
        validatePassword: (password: string) => validate("password", password),
    };
};

export default useAuth;
